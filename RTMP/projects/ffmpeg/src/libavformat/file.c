/*
 * Buffered file io for ffmpeg system
 * Copyright (c) 2001 Fabrice Bellard
 *
 * This file is part of FFmpeg.
 *
 * FFmpeg is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * FFmpeg is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with FFmpeg; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA
 */

#include "libavutil/avstring.h"
#include "avformat.h"
#include <fcntl.h>
#include <unistd.h>
#include <sys/time.h>
#include <stdlib.h>
#include "os_support.h"

//Fernando: 20080908
//#define LogStr(str)  printf ( "************************** %s: %s - %s-%d **************************\n", __func__, str, __FILE__, __LINE__)
//#define LogStr(str) av_log(NULL, AV_LOG_ERROR, "************************** %s: %s - %s-%d **************************\n", __func__, str, __FILE__, __LINE__);
#define LogStr(str) ;





/* standard file protocol */

static int file_open( URLContext *h, const char *filename, int flags )
{

    LogStr ("Init");

    //Fernando: 20080909
    char temp[1024];
    av_strlcpy(temp, "*filename: ", sizeof(temp));
    //av_strlcat(temp, "*filename: ", sizeof(temp));
    av_strlcat(temp, filename, sizeof(temp));
    LogStr (temp);



    int access;
    int fd;

    av_strstart(filename, "file:", &filename);

    if (flags & URL_RDWR)
    {
        access = O_CREAT | O_TRUNC | O_RDWR;
    }
    else if (flags & URL_WRONLY)
    {
        access = O_CREAT | O_TRUNC | O_WRONLY;
    }
    else
    {
        access = O_RDONLY;
    }
#ifdef O_BINARY
    access |= O_BINARY;
#endif
    fd = open(filename, access, 0666);
    if (fd < 0)
    {
        LogStr ("Exit");
        return AVERROR(ENOENT);
    }
    h->priv_data = (void *) (size_t) fd;

    LogStr ("Exit");
    return 0;
}

static int file_read( URLContext *h, unsigned char *buf, int size )
{
    LogStr ("Init");

    int fd = (size_t) h->priv_data;

    LogStr ("Exit");
    return read(fd, buf, size);
}

static int file_write( URLContext *h, unsigned char *buf, int size )
{
    LogStr ("Init");

    int fd = (size_t) h->priv_data;

    LogStr ("Exit");
    return write(fd, buf, size);
}

/* XXX: use llseek */
static offset_t file_seek( URLContext *h, offset_t pos, int whence )
{
    LogStr ("Init");

    int fd = (size_t) h->priv_data;

    LogStr ("Exit");
    return lseek(fd, pos, whence);
}

static int file_close( URLContext *h )
{
    LogStr ("Init");

    int fd = (size_t) h->priv_data;

    LogStr ("Exit");
    return close(fd);
}

URLProtocol file_protocol = { "file", file_open, file_read, file_write, file_seek, file_close, };

/* pipe protocol */

static int pipe_open( URLContext *h, const char *filename, int flags )
{
    LogStr ("Init");

    //Fernando: 20080909
    char temp[1024];
    av_strlcpy(temp, "*filename: ", sizeof(temp));
    av_strlcat(temp, filename, sizeof(temp));
    LogStr (temp);


    int fd;
    char *final;
    av_strstart(filename, "pipe:", &filename);

    fd = strtol(filename, &final, 10);
    if ((filename == final) || *final)
    {/* No digits found, or something like 10ab */
        if (flags & URL_WRONLY)
        {
            fd = 1;
        }
        else
        {
            fd = 0;
        }
    }
#ifdef O_BINARY
    setmode(fd, O_BINARY);
#endif
    h->priv_data = (void *) (size_t) fd;
    h->is_streamed = 1;

    LogStr ("Exit");
    return 0;
}

URLProtocol pipe_protocol = { "pipe", pipe_open, file_read, file_write, };
