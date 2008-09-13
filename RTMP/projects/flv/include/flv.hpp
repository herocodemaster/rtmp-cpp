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

#ifndef _FLV_H
#define _FLV_H

#include <libflv.hpp>

#define FLVTAGTYPE_AUDIO 8
#define FLVTAGTYPE_VIDEO 9

#define FLVFRAME_KEY 0x10
#define FLVFRAME_INTER 0x20
#define FLVFRAME_DISP 0x30

#define FLVCODEC_SORENSEN 0x02
#define FLVCODEC_SCREEN 0x03



struct FLVDataTag 
{
	unsigned char codecId;
	unsigned char frameType;
	unsigned int size;
	unsigned char *data;
};	

struct FLVTag 
{
	unsigned char tagType;
	unsigned int timeStamp;
	unsigned int dataSize;
	struct FLVDataTag tagData;
};

void FLV_writeTag(FLVStream *flv, struct FLVTag *tag);

#endif
