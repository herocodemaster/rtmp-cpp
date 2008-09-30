include $(SUBDIR)../config.mak

NAME = avformat
FFLIBS = avcodec avutil

OBJS = allformats.o cutils.o os_support.o sdp.o utils.o

HEADERS = avformat.h avio.h rtsp.h rtspcodes.h

# muxers/demuxers
OBJS-$(CONFIG_FLV_DEMUXER)               += flvdec.o
OBJS-$(CONFIG_FLV_MUXER)                 += flvenc.o avc.o

#OBJS-$(CONFIG_H263_DEMUXER)              += raw.o
#OBJS-$(CONFIG_H263_MUXER)                += raw.o

OBJS-$(CONFIG_IMAGE2_DEMUXER)            += img2.o
OBJS-$(CONFIG_IMAGE2_MUXER)              += img2.o

OBJS-$(CONFIG_MJPEG_DEMUXER)             += raw.o
OBJS-$(CONFIG_MJPEG_MUXER)               += raw.o

# protocols I/O
OBJS+= avio.o aviobuf.o

OBJS-$(CONFIG_FILE_PROTOCOL)             += file.o

include $(SUBDIR)../subdir.mak
