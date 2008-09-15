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


#ifndef _LIBFLV
#define _LIBFLV

#define FLVVERSION_1 1

#define FLVFLAG_VIDEO 0x1
#define FLVFLAG_AUDIO 0x4


struct FLVStream_s;
typedef struct FLVStream_s FLVStream;


/*
 * creates a new flvstream
 * @version FLV Version ( FLVVERSION_1 )
 * @flags FLVFLAG_*; add multiple flags with bitwise OR
 * @return FLVSTream
 */
FLVStream *newFLVStream(unsigned char version, unsigned char flags);

/* 
 * dump stream to file
 * @flv FLVStream
 * @name filename
 */
int FLVStream_write(FLVStream *flv, const char *name);

/*
 * dumps a FLV file 
 * @name filename
*/
void dumpFLVFile(const char *name);


/*** Screen Video Codec ***/

enum {
	BOTTOMUP,
	TOPDOWN
};

struct PixelData{
	/* number of pixels per row */
	int width;

	/* number of rows */
	int height;

	/* padding (in bytes) at row-end */
	int rowPadding;
	
	/* number of channels per pixel 
	 * RGB == 3
	 * RGB + Alpha = 4
	 */
	int n_channels;

	/* rowOrder BOTTOMUP or TOPDOWN */
	char rowOrder;
	
	/* pixel data. [r, g, b [,alpha]) */
	unsigned char *data;
};

struct ScreenVideo_s;
typedef struct ScreenVideo_s ScreenVideo;

/*
 * create a new ScreenVideo 
 * @width screen-width
 * @height screnn-height
 * @blockSize 	block size of video packets (in pixel)
 * 		>= 16 && <= 256 
 * 		and multiple of 16 
 */
ScreenVideo *newScreenVideo(int width, int height, int blockSize);


/* 
 * Sets zlib compression. 
 * Valid valuas are 0-9.
 * See zlib doc for details
 * @video ScreenVideo
 * @level compression level
 * @return int
 */
int ScreenVideo_setCompression(ScreenVideo *video, int level);

/*
 * adds a new videoFrame to a FLVStream
 * @video ScreenVideo
 * @flv FLVStream
 * @pixelData struct PixelData *
 * @timestamp timestamp in msec
 */
int
ScreenVideo_addFrame(
		ScreenVideo *video, 
		FLVStream *flv, 
		struct PixelData *pixelData,
		unsigned int timeStamp
		);



#endif
