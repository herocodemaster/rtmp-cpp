include $(SUBDIR)../config.mak

NAME = avcodec
FFLIBS = avutil

OBJS = allcodecs.o \
       audioconvert.o \
       bitstream.o \
       bitstream_filter.o \
       dsputil.o \
       eval.o \
       faanidct.o \
       imgconvert.o \
       jrevdct.o \
       opt.o \
       parser.o \
       raw.o \
       resample.o \
       resample2.o \
       simple_idct.o \
       utils.o \


HEADERS = avcodec.h opt.h

OBJS-$(CONFIG_ENCODERS)                += faandct.o jfdctfst.o jfdctint.o

OBJS-$(CONFIG_FLV_DECODER)             += h263dec.o h263.o mpeg12data.o mpegvideo.o error_resilience.o
OBJS-$(CONFIG_FLV_ENCODER)             += mpegvideo_enc.o motion_est.o ratecontrol.o h263.o mpeg12data.o mpegvideo.o error_resilience.o

OBJS-$(CONFIG_H263_DECODER)            += h263dec.o h263.o h263_parser.o mpeg12data.o mpegvideo.o error_resilience.o
OBJS-$(CONFIG_H263I_DECODER)           += h263dec.o h263.o mpeg12data.o mpegvideo.o error_resilience.o
OBJS-$(CONFIG_H263_ENCODER)            += mpegvideo_enc.o motion_est.o ratecontrol.o h263.o mpeg12data.o mpegvideo.o error_resilience.o
OBJS-$(CONFIG_H263P_ENCODER)           += mpegvideo_enc.o motion_est.o ratecontrol.o h263.o mpeg12data.o mpegvideo.o error_resilience.o

OBJS-$(CONFIG_MJPEG_DECODER)           += mjpegdec.o mjpeg.o
OBJS-$(CONFIG_MJPEG_ENCODER)           += mjpegenc.o mjpeg.o mpegvideo_enc.o motion_est.o ratecontrol.o mpeg12data.o mpegvideo.o









OBJS-$(CONFIG_H263_PARSER)             += h263_parser.o

OBJS-$(CONFIG_MJPEG_PARSER)            += mjpeg_parser.o

#OBJS-$(HAVE_BEOSTHREADS)               += beosthread.o
#OBJS-$(HAVE_OS2THREADS)                += os2thread.o
OBJS-$(HAVE_PTHREADS)                  += pthread.o
OBJS-$(HAVE_W32THREADS)                += w32thread.o

#OBJS-$(HAVE_XVMC)                      += xvmcvideo.o

ifndef CONFIG_SWSCALE
OBJS += imgresample.o
endif

# processor-specific code
ifdef HAVE_MMX
OBJS += i386/fdct_mmx.o \
        i386/cpuid.o \
        i386/dsputil_mmx.o \
        i386/mpegvideo_mmx.o \
        i386/motion_est_mmx.o \
        i386/simple_idct_mmx.o \
        i386/idct_mmx_xvid.o \
        i386/idct_sse2_xvid.o \

OBJS-$(HAVE_YASM)                      += i386/fft_mmx.o \
                                          i386/fft_sse.o \
                                          i386/fft_3dn.o \
                                          i386/fft_3dn2.o \
                                          i386/dsputil_yasm.o \

OBJS-$(CONFIG_GPL)                     += i386/idct_mmx.o

OBJS-$(CONFIG_ENCODERS)                += i386/dsputilenc_mmx.o


endif

ASM_OBJS-$(ARCH_ARMV4L)                += armv4l/jrevdct_arm.o     \
                                          armv4l/simple_idct_arm.o \
                                          armv4l/dsputil_arm_s.o   \

OBJS-$(ARCH_ARMV4L)                    += armv4l/dsputil_arm.o   \
                                          armv4l/mpegvideo_arm.o \

OBJS-$(HAVE_IWMMXT)                    += armv4l/dsputil_iwmmxt.o   \
                                          armv4l/mpegvideo_iwmmxt.o \

OBJS-$(HAVE_ARMV5TE)                   += armv4l/mpegvideo_armv5te.o \

OBJS-$(HAVE_ARMVFP)                    += armv4l/float_arm_vfp.o \

ASM_OBJS-$(HAVE_ARMV5TE)               += armv4l/simple_idct_armv5te.o \

ASM_OBJS-$(HAVE_ARMV6)                 += armv4l/simple_idct_armv6.o \

OBJS-$(HAVE_VIS)                       += sparc/dsputil_vis.o \
                                          sparc/simple_idct_vis.o \

#OBJS-$(CONFIG_MLIB)                    += mlib/dsputil_mlib.o \

OBJS-$(ARCH_ALPHA)                     += alpha/dsputil_alpha.o     \
                                          alpha/motion_est_alpha.o  \
                                          alpha/mpegvideo_alpha.o   \
                                          alpha/simple_idct_alpha.o \

ASM_OBJS-$(ARCH_ALPHA)                 += alpha/dsputil_alpha_asm.o  \
                                          alpha/motion_est_mvi_asm.o \

OBJS-$(ARCH_POWERPC)                   += ppc/dsputil_ppc.o   \

OBJS-$(HAVE_MMI)                       += ps2/dsputil_mmi.o   \
                                          ps2/idct_mmi.o      \
                                          ps2/mpegvideo_mmi.o \

OBJS-$(ARCH_SH4)                       += sh4/idct_sh4.o      \
                                          sh4/dsputil_align.o \
                                          sh4/dsputil_sh4.o   \

ALTIVEC-OBJS-yes                       += ppc/dsputil_altivec.o      \
                                          ppc/fdct_altivec.o         \
                                          ppc/fft_altivec.o          \
                                          ppc/float_altivec.o        \
                                          ppc/gmc_altivec.o          \
                                          ppc/idct_altivec.o         \
                                          ppc/int_altivec.o          \
                                          ppc/mpegvideo_altivec.o    \

ALTIVEC-OBJS-$(CONFIG_H264_DECODER)    += ppc/h264_altivec.o
ALTIVEC-OBJS-$(CONFIG_SNOW_DECODER)    += ppc/snow_altivec.o
ALTIVEC-OBJS-$(CONFIG_VC1_DECODER)     += ppc/vc1dsp_altivec.o
ALTIVEC-OBJS-$(CONFIG_WMV3_DECODER)    += ppc/vc1dsp_altivec.o

# -maltivec is needed in order to build AltiVec code.
$(addprefix $(SUBDIR),$(ALTIVEC-OBJS-yes)): CFLAGS += -maltivec -mabi=altivec

# check_altivec must be built without -maltivec
OBJS-$(HAVE_ALTIVEC)                   += $(ALTIVEC-OBJS-yes)       \
                                          ppc/check_altivec.o

OBJS-$(ARCH_BFIN)                      += bfin/dsputil_bfin.o \
                                          bfin/mpegvideo_bfin.o \
                                          bfin/vp3_bfin.o \

ASM_OBJS-$(ARCH_BFIN)                  += bfin/pixels_bfin.o \
                                          bfin/fdct_bfin.o   \
                                          bfin/idct_bfin.o   \
                                          bfin/vp3_idct_bfin.o   \

TESTS = $(addsuffix -test$(EXESUF), cabac dct eval fft h264 imgresample rangecoder snow)
TESTS-$(ARCH_X86) += i386/cpuid-test$(EXESUF) motion-test$(EXESUF)

CLEANFILES = apiexample$(EXESUF)
DIRS = alpha armv4l bfin i386 mlib ppc ps2 sh4 sparc

include $(SUBDIR)../subdir.mak

$(SUBDIR)dct-test$(EXESUF): $(SUBDIR)fdctref.o
