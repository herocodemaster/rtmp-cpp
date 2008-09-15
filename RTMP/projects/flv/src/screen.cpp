/*
 * Copyright 2004 Klaus Rechert
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */


#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/stat.h>
#include <fcntl.h>
#include <zlib.h>
#include <flv.hpp>
#include <assert.h>
#include <screen.hpp>

#define BLOCK_DIRTY 1

struct ScreenVideo_s {
	int blockSize;			
	int imageWidth;
	int imageHeight;
	int rows;			
	int cols;
	int frame;				/* frame number */
	int frameSize;				/* bytes used in framebuf */
	unsigned int timeOffset;
	
	char *blockMap;
	struct BGR *image;
	unsigned char *framebuf;
};


static inline void RGB2GBR(struct RGB *src, struct BGR *dest) {
	dest->r = src->r;
	dest->g = src->g;
	dest->b = src->b;
}

static inline int compareRGB_BGR(struct RGB *p1, struct BGR *p2) {
	if(
			p1->r != p2->r ||
			p1->g != p2->g ||
			p1->b != p2->b
	){
		return 1;
	}
	else
		return 0;
}

static inline void __putChar(unsigned char *data, unsigned int val)  //u_int32_t
{
	*data = (val & 0x000000ff);
}

static inline void markBlock(ScreenVideo *video, int x, int y) {
	int row, col;
	
	col = x / video->blockSize;
	row = y / video->blockSize;

	video->blockMap[row * video->cols + col] = BLOCK_DIRTY;
}	

static inline void resetBlockMap(ScreenVideo *video) {
	memset(video->blockMap, 0, video->cols * video->rows);
}

static inline void markAllBlocks(ScreenVideo *video) {
	memset(video->blockMap, 1, video->cols * video->rows);
}

ScreenVideo *newScreenVideo(int width, int height, int blockSize) {
	ScreenVideo *video;
	int imageSize;
	
	if(blockSize < 16 || blockSize > 256) {
		printf("blocksize must lie between 16 and 256");
		return NULL;
	}

	video = (ScreenVideo *)malloc(sizeof(struct ScreenVideo_s));
	
	video->frame = 0;
	video->timeOffset = 0;
	video->imageWidth = width;
	video->imageHeight = height;
	video->blockSize = blockSize;
	video->rows = height / blockSize;
	if(height % blockSize)
		video->rows++;

	video->cols = width / blockSize;
	if(width % blockSize)
		video->cols++;

	video->blockMap = (char *)malloc(video->cols * video->rows);
	resetBlockMap(video);
	
	imageSize = sizeof(struct BGR) * width * height;
	video->image = (struct BGR *)malloc(imageSize);
	memset(video->image, 0, imageSize);

	/* 
	 * framebuf used to hold frame payload 
	 *
	 * to avoid dynamic growing and sizechecks
	 * buffer is init to imageSize * 1.5 bytes
	 * since it holds compress imageBlocks it is
	 * impossible to grow bigger then imageSize
	 */
	video->framebuf = (unsigned char *)malloc(imageSize * 1.5);
	memset(video->image, 0, imageSize * 1.5); 

	video->frameSize = 0;
	
	return video;
}

static int getImageBlock(ScreenVideo *video, int row, int col, struct BGR *block) {
	int blockSize = video->blockSize;
	int x_l, x_h, y_l, y_h;
	int x, y, line, pos;
	int size = 0;

	x_l = col * blockSize;
	y_l = row * blockSize;

	if(col == video->cols - 1) 
		x_h = video->imageWidth;
	else
		x_h = x_l + blockSize;

	if(row == video->rows - 1)
		y_h = video->imageHeight;
	else
		y_h = y_l + blockSize;


	for(y = y_l; y < y_h; y++) {
		line = y * video->imageWidth;
		for(x = x_l; x < x_h; x++) {
			pos = line + x;
			block[size++] = video->image[pos];
		}
	}
	
	return size;
}

static inline void writeDataSize(ScreenVideo *video, unsigned int size) {
	unsigned char c;

	c = (0xff & (size >> 8));
	__putChar(video->framebuf + video->frameSize++, c);

	c = (0xff & size);
	__putChar(video->framebuf + video->frameSize++, c);
}	

static inline int writeEmptyImageBlock(ScreenVideo *video) {
	writeDataSize(video, 0);
	return 2;
}
	

static int writeImageBlock(ScreenVideo *video, int row, int col) 
{
	//struct BGR src[video->blockSize * video->blockSize];
	struct BGR *src = new struct BGR[video->blockSize * video->blockSize];

	//unsigned char dest[video->blockSize * video->blockSize * 4];
	unsigned char *dest = new unsigned char[video->blockSize * video->blockSize * 4];


	int size, ret;
	unsigned long zSize;
	zSize = video->blockSize * video->blockSize * 4;
	
	size = getImageBlock(video, row, col, src);
	ret = compress2((Bytef *)dest, &zSize, (Bytef *)src, size * sizeof(struct BGR), 9);

	if(ret != Z_OK) 
	{
		printf("compression failed %i\n", ret);
		return -1;
	}

	writeDataSize(video, zSize);
	memcpy(video->framebuf + video->frameSize, dest, zSize);
	video->frameSize += zSize;
	return zSize + 2;
}



static inline int writeVideoPacketHeader(ScreenVideo *video) {
	unsigned char c;
	unsigned char blockSize;

	blockSize = (video->blockSize / 16) - 1;
	
	c = (blockSize & 0x0f) << 4;			/* UB[4] blockWidth */
	c |= ( (video->imageWidth & 0x0f00) >> 8 ) & 0x0f;	/* UB[12] imageWidth */
	__putChar(video->framebuf + video->frameSize++, c);
	__putChar(video->framebuf + video->frameSize++, video->imageWidth);
	
	c = (blockSize & 0x0f) << 4;  			/* UB[4] blockHeight */
	c |= ( (video->imageHeight & 0x0f00) >> 8 ) & 0x0f;	/* UB[12] imageHeight */
	__putChar(video->framebuf + video->frameSize++, c);
	__putChar(video->framebuf + video->frameSize++, video->imageHeight);

	return 4;
}




static void writeVideoData(ScreenVideo *video, struct FLVDataTag *dataTag) 
{
	int row, col, pos;
	
	writeVideoPacketHeader(video);
	for(row = 0; row < video->rows; row++) 
	{
		for(col = 0; col < video->cols; col++) 
		{
			pos = row * video->cols + col;
			if(video->blockMap[pos] == BLOCK_DIRTY) 
			{
				writeImageBlock(video, row, col);
			}
			else
			{
				writeEmptyImageBlock(video);
			}
		}
	}

	dataTag->size = video->frameSize;
	dataTag->data = video->framebuf;
}



struct FLVTag *getFLVTag(ScreenVideo *video, int timeStamp) 
{	
	struct FLVTag *tag;
	struct FLVDataTag *dataTag;
	
	tag = (struct FLVTag *)malloc(sizeof(struct FLVTag));

	dataTag = &(tag->tagData);
	
	tag->tagType = FLVTAGTYPE_VIDEO;
	tag->timeStamp = timeStamp;
	dataTag->codecId = FLVCODEC_SCREEN;

	if(video->frame == 0)
	{
		dataTag->frameType = FLVFRAME_KEY;
	}
	else
	{
		dataTag->frameType = FLVFRAME_INTER;
	}

	writeVideoData(video, dataTag);
	tag->dataSize = dataTag->size + 1;
	
	return tag;
}




static inline int clearFrame(ScreenVideo *video) 
{
	resetBlockMap(video);
	video->frame++;
	video->frameSize = 0;
	return video->frame;
}

int ScreenVideo_addFrame(ScreenVideo *video, FLVStream *flv, struct PixelData *pixelData, unsigned int timeStamp ) 
{
	int x, y, row = 0;
	struct FLVTag *tag;
	int _timeStamp;
	struct RGB *rgb;
	struct BGR *bgr;
	int size = 0;
	int rowstride;
	
	if(!pixelData || !video || !flv)
	{
		return -1;
	}

	if(video->frame == 0) 
	{
		video->timeOffset = timeStamp;
		_timeStamp = 0;
		markAllBlocks(video);
	}
	else 
	{
		_timeStamp = timeStamp - video->timeOffset;
	}
	
	if(video->imageWidth != pixelData->width || video->imageHeight != pixelData->height) 
	{
		printf("resize not supported\n");
		return -1;
	}	

	
	rowstride = pixelData->width * pixelData->n_channels + pixelData->rowPadding;
	
	for( y = 0; y < pixelData->height; y++) 
	{
		if(pixelData->rowOrder == TOPDOWN)
		{
			row = (pixelData->height - y - 1) * rowstride;
		}
		else if (pixelData->rowOrder == BOTTOMUP) 
		{
			row = y * rowstride;
		}
		
		for( x = 0; x < pixelData->width; x++) 
		{	
			rgb = (struct RGB*)(pixelData->data + row + x * pixelData->n_channels);
			bgr = video->image + size++;
			
			if(compareRGB_BGR(rgb, bgr)) 
			{
				RGB2GBR(rgb, bgr);
				markBlock(video, x, y);
			}
		}
	}
	
	tag = getFLVTag(video, _timeStamp);
	FLV_writeTag(flv, tag);
	
	if(tag != NULL)
	{
		free(tag);
	}

	return clearFrame(video);
}
