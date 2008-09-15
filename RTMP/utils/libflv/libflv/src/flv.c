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
#include <stdlib.h>
#include <string.h>
#include <sys/types.h>
#include <sys/stat.h>
#include <unistd.h>
#include <fcntl.h>
#include <sys/mman.h>
#include <flv.h>
#include <errno.h>

int errno;

struct FLVStream_s {
	int version;
	int length;
	int pos;
	unsigned char *data;
};

#define FLVGROWBY 4096
inline void grow(FLVStream *flv) {
	flv->length += FLVGROWBY;
	flv->data = realloc(flv->data, flv->length);
}

static inline void __putChar(unsigned char *data, u_int32_t val) {
	*data = (val & 0x000000ff);
}

void writeData(FLVStream *flv, unsigned char *data, int size) {
	while(flv->pos + size >= flv->length)
		grow(flv);
	
	memcpy(flv->data + flv->pos, data, size);
	flv->pos += size;
}

static inline void putChar(FLVStream *flv, u_int32_t val) {
	if( flv->pos + 1 >= flv->length ) 
		grow(flv);
	
	__putChar(flv->data + flv->pos++, val);
}

static inline void putUI16(FLVStream *flv, u_int32_t val) {
	putChar(flv, (val >> 8));
	putChar(flv, val);;
}

static inline void putUI24(FLVStream *flv, u_int32_t val) {
	putChar(flv, (val >> 16));
	putUI16(flv, val);
}


static inline void putUI32(FLVStream *flv, u_int32_t val) {
	putChar(flv, (val >> 24));
	putUI24(flv, val);
}



void writeTagData(FLVStream *flv, struct FLVDataTag *tagData) {
	if(!tagData)
		return;
	
	putChar(flv, tagData->codecId | tagData->frameType);
	writeData(flv, tagData->data, tagData->size);
}

void FLV_writeTag(FLVStream *flv, struct FLVTag *tag) {
	if(!flv || !tag)
		return;
		
	putChar(flv, tag->tagType);
	putUI24(flv, tag->dataSize);
	putUI24(flv, tag->timeStamp);
	putUI32(flv, 0);
	
	writeTagData(flv, &(tag->tagData));
	putUI32(flv, tag->dataSize + 11);

}


void writeHeader(FLVStream *flv, unsigned char version, unsigned char flags) {
	if(!flv)
		return;

	putChar(flv, 'F');
	putChar(flv, 'L');
	putChar(flv, 'V');
	putChar(flv, version);
	putChar(flv, flags);
	putUI32(flv, 9);
	putUI32(flv, 0);
}

FLVStream *newFLVStream(unsigned char version, unsigned char flags) {
	FLVStream *flv;

	flv = (FLVStream *)malloc(sizeof(struct FLVStream_s));
	flv->version = version;

	flv->length = FLVGROWBY;
	flv->data = (unsigned char *)malloc(flv->length);
	flv->pos = 0;

	writeHeader(flv, version, flags);

	return flv;
}

int FLVStream_write(FLVStream *flv, const char *name) {
	int fd;

	fd = open(name, O_WRONLY | O_CREAT | O_TRUNC, S_IRWXU);
	if(fd < 0) {
		perror("Write failed");
		return -1;
	}

	return (int)write(fd, flv->data, flv->pos);
}


inline unsigned char getChar(FLVStream *flv) {
	unsigned char c = 0;
	if(flv->pos < flv->length) {
		c = *(flv->data + flv->pos);
		flv->pos++;
	}
	return c;
}


inline unsigned int getUI24(FLVStream *flv) {
	unsigned int ui24;
	unsigned int c;

	c = getChar(flv);
	ui24 = (c<<16);
	c = getChar(flv);
	ui24 |= (c<<8);
	c = getChar(flv);
	ui24 |= c;
	
	return ui24;
}


inline unsigned int getUI32(FLVStream *flv) {
	unsigned int ui32;
	unsigned int c;

	c = getChar(flv);
	ui32 = (c<<24);
	c = getChar(flv);
	ui32 = (c<<16);
	c = getChar(flv);
	ui32 |= (c<<8);
	c = getChar(flv);
	ui32 |= c;
	
	return ui32;
}

static void dumpHeader(FLVStream *flv) {
	unsigned char c1,c2,c3;

	c1 = getChar(flv);
	c2 = getChar(flv);
	c3 = getChar(flv);
	if (c1 != 'F' && c2 != 'L' && c3 != 'V') {
		printf("Not a flv-file\n");
		return ;
	}
	
	printf("version: %i\n", getChar(flv));
	printf("flags %x \n", getChar(flv));
	printf("offset %i \n", getUI32(flv));
	printf("%i\n", getUI32(flv));
} 

void dumpHex(FLVStream *flv, int size, int width) {
	unsigned int c;
	
	while(size > 0) {
		c = getChar(flv);
		printf("%.2x ", c);
		if(size % width == 0)
			printf("\n");
		size--;
	}

	printf("\n\n");
}		
		

void dumpVideoData(FLVStream *flv, int size) {
	unsigned char c;

	c = getChar(flv);

	printf("VideoDataHeader %x\n", c);

	dumpHex(flv, size -1, 8);
}

int dumpFLVTag(FLVStream *flv) {
	unsigned int nul, len;
	struct FLVTag tag;

	tag.tagType = getChar(flv);
	switch (tag.tagType)
	{
		case FLVTAGTYPE_AUDIO:
			printf("AudioTag:\n");
			break;
		case FLVTAGTYPE_VIDEO:
			printf("VideoTag:\n");
			break;
		default:
			printf("Unknown TagType %x\n ", tag.tagType);
			return 0;
	}
	
	tag.dataSize = getUI24(flv);
	printf("dataSize %i\n", tag.dataSize);
	tag.timeStamp = getUI24(flv);
	printf("timeStamp %i\n", tag.timeStamp);
	
	nul = getUI32(flv);
	if(nul)
		return 0;

	switch (tag.tagType)
	{
		case FLVTAGTYPE_AUDIO:
			printf("AudioData not implemented\n");
			flv->pos += tag.dataSize;
			break;
		case FLVTAGTYPE_VIDEO:
			dumpVideoData(flv, tag.dataSize);
			break;
		default:
			return 0;
	}

	len = getUI32(flv);
	printf("tag-len %i\n\n\n", len);
	
	return 1;
}


void dumpFLVFile(const char *name) {
	struct stat buf;
	FLVStream *flv;
	int fd, tagCount = 0;
	
	if(stat(name, &buf) < 0) {
		perror("Can not open flv-file");
		return;
	}

	flv = (FLVStream*)malloc(sizeof(struct FLVStream_s));
	flv->length = buf.st_size;
	flv->pos = 0;
	
	fd = open(name, O_RDONLY, 0);
	flv->data = mmap(flv->data, flv->length, PROT_READ, MAP_PRIVATE, fd, 0);
	if((int)flv->data < 0) {
		perror("Can not open file");
		return;
	}

	dumpHeader(flv);

	do {
		printf("TAG: %i \n", tagCount++);
	} while (dumpFLVTag(flv));
}		


