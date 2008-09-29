//TODO: Fernando: Loggear

/*
 * Unbuffered io for ffmpeg system
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
#include "libavcodec/opt.h"
#include "avformat.h"


//Fernando: 20080908
//#undef printf
//#define LogStr(str)  printf ( "************************** %s: %s - %s-%d **************************\n", __func__, str, __FILE__, __LINE__)
//#define LogStr(str) av_log(NULL, AV_LOG_ERROR, "************************** %s: %s - %s-%d **************************\n", __func__, str, __FILE__, __LINE__);
#define LogStr(str) ;





#if LIBAVFORMAT_VERSION_MAJOR >= 53
/** @name Logging context. */
/*@{*/
static const char *urlcontext_to_name(void *ptr)
{
    LogStr ("Init");

    URLContext *h = (URLContext *)ptr;
    if(h->prot)
    {
        LogStr ("Exit");
        return h->prot->name;
    }
    else
    {
        LogStr ("Exit");
        return "NULL";
    }
}
static const AVOption options[] =
{
    {   NULL}};
static const AVClass urlcontext_class =
{   "URLContext", urlcontext_to_name, options};
/*@}*/
#endif

static int default_interrupt_cb( void );

URLProtocol *first_protocol = NULL;
URLInterruptCB *url_interrupt_cb = default_interrupt_cb;

URLProtocol *av_protocol_next( URLProtocol *p )
{
    LogStr ("Init");

    if (p)
    {
        LogStr ("Exit");
        return p->next;
    }
    else
    {
        LogStr ("Exit");
        return first_protocol;
    }
}

int register_protocol( URLProtocol *protocol )
{
    LogStr ("Init");

    URLProtocol **p;
    p = &first_protocol;

    while (*p != NULL)
    {
        p = &(*p)->next;
    }

    *p = protocol;
    protocol->next = NULL;

    LogStr ("Exit");
    return 0;
}

int url_open_protocol( URLContext **puc, struct URLProtocol *up, const char *filename, int flags )
{
    LogStr ("Init");

    URLContext *uc;
    int err;

    uc = av_malloc(sizeof(URLContext) + strlen(filename) + 1);
    if (!uc)
    {
        err = AVERROR(ENOMEM);
        goto fail;
    }
#if LIBAVFORMAT_VERSION_MAJOR >= 53
    uc->av_class = &urlcontext_class;
#endif
    uc->filename = (char *) &uc[1];
    strcpy(uc->filename, filename);



    //Fernando: 20080909
    char temp[1024];
    av_strlcpy(temp, "uc->filename: ", sizeof(temp));
    av_strlcat(temp, uc->filename, sizeof(temp));
    LogStr (temp);



    uc->prot = up;
    uc->flags = flags;
    uc->is_streamed = 0; /* default = not streamed */
    uc->max_packet_size = 0; /* default: stream file */
    err = up->url_open(uc, filename, flags);
    if (err < 0)
    {
        av_free(uc);
        *puc = NULL;
        LogStr ("Exit");
        return err;
    }

    //We must be carefull here as url_seek() could be slow, for example for http
    if ((flags & (URL_WRONLY | URL_RDWR)) || !strcmp(up->name, "file"))
    {
        LogStr ("IF 1");
        if (!uc->is_streamed && url_seek(uc, 0, SEEK_SET) < 0)
        {
            LogStr ("IF 2");
            uc->is_streamed = 1;
        }
    }

    *puc = uc;

    LogStr ("Exit");
    return 0;
    fail: *puc = NULL;

    LogStr ("Exit");
    return err;
}

int url_open( URLContext **puc, const char *filename, int flags )
{
    LogStr ("Init");

    URLProtocol *up;
    const char *p;
    char proto_str[128], *q;

    p = filename;
    q = proto_str;
    while (*p != '\0' && *p != ':')
    {
        /* protocols can only contain alphabetic chars */
        if (!isalpha(*p))
        {
            //LogStr ("GOTO");
            goto file_proto;
        }

        if ((q - proto_str) < sizeof(proto_str) - 1)
        {
            *q++ = *p;
        }
        p++;
    }



    /* if the protocol has length 1, we consider it is a dos drive */
    if (*p == '\0' || (q - proto_str) <= 1)
    {
        file_proto: strcpy(proto_str, "file");
    }
    else
    {
        *q = '\0';
    }

    /*
    //Fernando: 20080909
    char temp[1024];
    av_strlcpy(temp, "proto_str: ", sizeof(temp));
    av_strlcat(temp, proto_str, sizeof(temp));
    LogStr (temp);
    */


    //LogStr ("FUERA DEL WHILE");

    up = first_protocol;
    while (up != NULL)
    {
        //LogStr ("DENTRO DEL WHILE");

        //Fernando: 20080909
        char temp[1024];
        av_strlcpy(temp, "up->name: ", sizeof(temp));
        av_strlcat(temp, up->name, sizeof(temp));
        LogStr (temp);

        if (!strcmp(proto_str, up->name))
        {
            LogStr ("Exit");
            return url_open_protocol(puc, up, filename, flags);
        }
        up = up->next;
    }
    *puc = NULL;
    LogStr ("FIN DEL WHILE");

    LogStr ("Exit");
    return AVERROR(ENOENT);
}

int url_read( URLContext *h, unsigned char *buf, int size )
{
    LogStr ("Init");

    int ret;
    if (h->flags & URL_WRONLY)
    {
        LogStr ("Exit");
        return AVERROR(EIO);
    }

    ret = h->prot->url_read(h, buf, size);

    LogStr ("Exit");
    return ret;
}

int url_write( URLContext *h, unsigned char *buf, int size )
{
    LogStr ("Init");

    int ret;
    if (!(h->flags & (URL_WRONLY | URL_RDWR)))
    {
        LogStr ("Exit");
        return AVERROR(EIO);
    }

    /* avoid sending too big packets */
    if (h->max_packet_size && size > h->max_packet_size)
    {
        LogStr ("Exit");
        return AVERROR(EIO);
    }
    ret = h->prot->url_write(h, buf, size);

    LogStr ("Exit");
    return ret;
}

offset_t url_seek( URLContext *h, offset_t pos, int whence )
{
    LogStr ("Init");

    offset_t ret;

    if (!h->prot->url_seek)
    {
        LogStr ("Exit");
        return AVERROR(EPIPE);
    }
    ret = h->prot->url_seek(h, pos, whence);

    LogStr ("Exit");
    return ret;
}

int url_close( URLContext *h )
{
    LogStr ("Init");

    int ret = 0;
    if (!h)
    {
        LogStr ("Exit");
        return 0; /* can happen when url_open fails */
    }

    if (h->prot->url_close)
        ret = h->prot->url_close(h);
    av_free(h);

    LogStr ("Exit");
    return ret;
}

int url_exist( const char *filename )
{
    LogStr ("Init");

    //Fernando: 20080909
    char temp[1024];
    av_strlcpy(temp, "*filename: ", sizeof(temp));
    av_strlcat(temp, filename, sizeof(temp));
    LogStr (temp);


    URLContext *h;

    if (url_open(&h, filename, URL_RDONLY) < 0)
    {
        LogStr ("Exit");
        return 0;
    }
    url_close(h);

    LogStr ("Exit");
    return 1;
}

offset_t url_filesize( URLContext *h )
{
    LogStr ("Init");

    offset_t pos, size;

    size = url_seek(h, 0, AVSEEK_SIZE);
    if (size < 0)
    {
        pos = url_seek(h, 0, SEEK_CUR);
        if ((size = url_seek(h, -1, SEEK_END)) < 0)
        {
            LogStr ("Exit");
            return size;
        }

        size++;
        url_seek(h, pos, SEEK_SET);
    }
    LogStr ("Exit");
    return size;
}

int url_get_max_packet_size( URLContext *h )
{
    LogStr ("Init");
    LogStr ("Exit");
    return h->max_packet_size;
}

void url_get_filename( URLContext *h, char *buf, int buf_size )
{
    LogStr ("Init");

    av_strlcpy(buf, h->filename, buf_size);

    LogStr ("Exit");
}

static int default_interrupt_cb( void )
{
    LogStr ("Init");
    LogStr ("Exit");
    return 0;
}

void url_set_interrupt_cb( URLInterruptCB *interrupt_cb )
{
    LogStr ("Init");

    if (!interrupt_cb)
    {
        interrupt_cb = default_interrupt_cb;
    }
    url_interrupt_cb = interrupt_cb;

    LogStr ("Exit");
}

int av_url_read_pause( URLContext *h, int pause )
{
    LogStr ("Init");

    if (!h->prot->url_read_pause)
    {
        LogStr ("Exit");
        return AVERROR(ENOSYS);
    }

    LogStr ("Exit");
    return h->prot->url_read_pause(h, pause);
}

offset_t av_url_read_seek( URLContext *h, int stream_index, int64_t timestamp, int flags )
{
    LogStr ("Init");

    if (!h->prot->url_read_seek)
    {
        LogStr ("Exit");
        return AVERROR(ENOSYS);
    }

    LogStr ("Exit");
    return h->prot->url_read_seek(h, stream_index, timestamp, flags);
}
