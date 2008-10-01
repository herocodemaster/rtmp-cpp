//Fernando: 20080908
//#define LogStr(str)  printf ( "************************** %s: %s - %s-%d **************************\n", __func__, str, __FILE__, __LINE__)
#define LogStr(str) av_log(NULL, AV_LOG_ERROR, "************************** %s: %s - %s-%d **************************\n", __func__, str, __FILE__, __LINE__);


/* needed for usleep() */
#define _XOPEN_SOURCE 500

#include "config.h"
#include <ctype.h>
#include <string.h>
#include <math.h>
#include <stdlib.h>
#include <errno.h>
#include <signal.h>
#include <limits.h>
#include <unistd.h>
#include "../libavformat/avformat.h"
#include "../libavdevice/avdevice.h"
#include "../libswscale/swscale.h"
#include "../libavformat/framehook.h"
#include "../libavcodec/opt.h"
#include "../libavcodec/audioconvert.h"
#include "../libavutil/fifo.h"
#include "../libavutil/avstring.h"
#include "../libavformat/os_support.h"

#ifdef HAVE_SYS_RESOURCE_H
#include <sys/types.h>
#include <sys/resource.h>
#elif defined(HAVE_GETPROCESSTIMES)
#include <windows.h>
#endif

#if defined(HAVE_TERMIOS_H)
#include <fcntl.h>
#include <sys/ioctl.h>
#include <sys/time.h>
#include <termios.h>
#elif defined(HAVE_CONIO_H)
#include <conio.h>
#endif
#undef time //needed because HAVE_AV_CONFIG_H is defined on top
#include <time.h>

#include "cmdutils.h"

#undef NDEBUG
#include <assert.h>

#undef exit


//Fernando: 20080911
#include <inttypes.h>


const char program_name[] = "FFmpeg";
const int program_birth_year = 2000;

/* select an input stream for an output stream */
typedef struct AVStreamMap
{
    int file_index;
    int stream_index;
    int sync_file_index;
    int sync_stream_index;
} AVStreamMap;

/** select an input file for an output file */
typedef struct AVMetaDataMap
{
    int out_file;
    int in_file;
} AVMetaDataMap;

static const OptionDef options[];

#define MAX_FILES 20

static AVFormatContext *input_files[MAX_FILES];
static int64_t input_files_ts_offset[MAX_FILES];
static double input_files_ts_scale[MAX_FILES][MAX_STREAMS];
static int nb_input_files = 0;

static AVFormatContext *output_files[MAX_FILES];
static int nb_output_files = 0;

static AVStreamMap stream_maps[MAX_FILES * MAX_STREAMS];
static int nb_stream_maps;

static AVMetaDataMap meta_data_maps[MAX_FILES];
static int nb_meta_data_maps;

static AVInputFormat *file_iformat;
static AVOutputFormat *file_oformat;
static int frame_width = 0;
static int frame_height = 0;
static float frame_aspect_ratio = 0;
static enum PixelFormat frame_pix_fmt = PIX_FMT_NONE;
static enum SampleFormat audio_sample_fmt = SAMPLE_FMT_NONE;
static int frame_padtop = 0;
static int frame_padbottom = 0;
static int frame_padleft = 0;
static int frame_padright = 0;
static int padcolor[3] = { 16, 128, 128 }; /* default to black */
static int frame_topBand = 0;
static int frame_bottomBand = 0;
static int frame_leftBand = 0;
static int frame_rightBand = 0;
static int max_frames[4] = { INT_MAX, INT_MAX, INT_MAX, INT_MAX };
static AVRational frame_rate;
static float video_qscale = 0;
static uint16_t *intra_matrix = NULL;
static uint16_t *inter_matrix = NULL;
#if 0 //experimental, (can be removed)
static float video_rc_qsquish=1.0;
static float video_rc_qmod_amp=0;
static int video_rc_qmod_freq=0;
#endif
static const char *video_rc_override_string = NULL;
static int video_disable = 0;
static int video_discard = 0;
static char *video_codec_name = NULL;
static int video_codec_tag = 0;
static int same_quality = 0;
static int do_deinterlace = 0;
static int top_field_first = -1;
static int me_threshold = 0;
static int intra_dc_precision = 8;
static int loop_input = 0;
static int loop_output = AVFMT_NOOUTPUTLOOP;
static int qp_hist = 0;

static int intra_only = 0;
static int audio_sample_rate = 44100;
#define QSCALE_NONE -99999
static float audio_qscale = QSCALE_NONE;
static int audio_disable = 0;
static int audio_channels = 1;
static char *audio_codec_name = NULL;
static int audio_codec_tag = 0;
static char *audio_language = NULL;

static int subtitle_disable = 0;
static char *subtitle_codec_name = NULL;
static char *subtitle_language = NULL;

static float mux_preload = 0.5;
static float mux_max_delay = 0.7;

static int64_t recording_time = INT64_MAX;
static int64_t start_time = 0;
static int64_t rec_timestamp = 0;
static int64_t input_ts_offset = 0;
static int file_overwrite = 0;
static char *str_title = NULL;
static char *str_author = NULL;
static char *str_copyright = NULL;
static char *str_comment = NULL;
static char *str_genre = NULL;
static char *str_album = NULL;
static int do_benchmark = 0;
static int do_hex_dump = 0;
static int do_pkt_dump = 0;
static int do_psnr = 0;
static int do_pass = 0;
static char *pass_logfilename = NULL;
static int audio_stream_copy = 0;
static int video_stream_copy = 0;
static int subtitle_stream_copy = 0;
static int video_sync_method = -1;
static int audio_sync_method = 0;
static float audio_drift_threshold = 0.1;
static int copy_ts = 0;
static int opt_shortest = 0; //
static int video_global_header = 0;
static char *vstats_filename;
static FILE *vstats_file;
static int opt_programid = 0;

static int rate_emu = 0;

static int video_channel = 0;
static char *video_standard;

static int audio_volume = 256;

static int using_stdin = 0;
static int using_vhook = 0;
static int verbose = 1;
static int thread_count = 1;
static int q_pressed = 0;
static int64_t video_size = 0;
static int64_t audio_size = 0;
static int64_t extra_size = 0;
static int nb_frames_dup = 0;
static int nb_frames_drop = 0;
static int input_sync;
static uint64_t limit_filesize = 0; //

static int pgmyuv_compatibility_hack = 0;
static float dts_delta_threshold = 10;

static unsigned int sws_flags = SWS_BICUBIC;

static const char **opt_names;
static int opt_name_count;
static AVCodecContext *avctx_opts[CODEC_TYPE_NB];
static AVFormatContext *avformat_opts;
static struct SwsContext *sws_opts;
static int64_t timer_start;

static AVBitStreamFilterContext *video_bitstream_filters = NULL;
static AVBitStreamFilterContext *audio_bitstream_filters = NULL;
static AVBitStreamFilterContext *subtitle_bitstream_filters = NULL;
static AVBitStreamFilterContext *bitstream_filters[MAX_FILES][MAX_STREAMS];

#define DEFAULT_PASS_LOGFILENAME "ffmpeg2pass"

struct AVInputStream;

typedef struct AVOutputStream
{
    int file_index; /* file index */
    int index; /* stream index in the output file */
    int source_index; /* AVInputStream index */
    AVStream *st; /* stream in the output file */
    int encoding_needed; /* true if encoding needed for this stream */
    int frame_number;
    /* input pts and corresponding output pts
     for A/V sync */
    //double sync_ipts;        /* dts from the AVPacket of the demuxer in second units */
    struct AVInputStream *sync_ist; /* input stream to sync against */
    int64_t sync_opts; /* output frame counter, could be changed to some true timestamp *///FIXME look at frame_number
    /* video only */
    int video_resample;
    AVFrame pict_tmp; /* temporary image for resampling */
    struct SwsContext *img_resample_ctx; /* for image resampling */
    int resample_height;

    int video_crop;
    int topBand; /* cropping area sizes */
    int leftBand;

    int video_pad;
    int padtop; /* padding area sizes */
    int padbottom;
    int padleft;
    int padright;

    /* audio only */
    int audio_resample;
    ReSampleContext *resample; /* for audio resampling */
    int reformat_pair;
    AVAudioConvert *reformat_ctx;
    AVFifoBuffer fifo; /* for compression: one audio fifo per codec */
    FILE *logfile;
} AVOutputStream;

typedef struct AVInputStream
{
    int file_index;
    int index;
    AVStream *st;
    int discard; /* true if stream data should be discarded */
    int decoding_needed; /* true if the packets must be decoded in 'raw_fifo' */
    int64_t sample_index; /* current sample */

    int64_t start; /* time when read started */
    unsigned long frame; /* current frame */
    int64_t next_pts; /* synthetic pts for cases where pkt.pts
     is not defined */
    int64_t pts; /* current pts */
    int is_start; /* is 1 at the start and after a discontinuity */
} AVInputStream;

typedef struct AVInputFile
{
    int eof_reached; /* true if eof reached */
    int ist_index; /* index of first stream in ist_table */
    int buffer_size; /* current total buffer size */
    int nb_streams; /* nb streams we are aware of */
} AVInputFile;

#ifdef HAVE_TERMIOS_H

/* init terminal so that we can grab keys */
static struct termios oldtty;
#endif





static volatile sig_atomic_t received_sigterm = 0;
#define MAX_AUDIO_PACKET_SIZE (128 * 1024)

/* we begin to correct av delay at this threshold */
#define AV_DELAY_MAX 0.100






static int bit_buffer_size = 1024 * 256;
static uint8_t *bit_buffer = NULL;

#define SCALEBITS 10
#define ONE_HALF  (1 << (SCALEBITS - 1))
#define FIX(x)    ((int) ((x) * (1<<SCALEBITS) + 0.5))

#define RGB_TO_Y(r, g, b) \
((FIX(0.29900) * (r) + FIX(0.58700) * (g) + \
  FIX(0.11400) * (b) + ONE_HALF) >> SCALEBITS)

#define RGB_TO_U(r1, g1, b1, shift)\
(((- FIX(0.16874) * r1 - FIX(0.33126) * g1 +         \
     FIX(0.50000) * b1 + (ONE_HALF << shift) - 1) >> (SCALEBITS + shift)) + 128)

#define RGB_TO_V(r1, g1, b1, shift)\
(((FIX(0.50000) * r1 - FIX(0.41869) * g1 -           \
   FIX(0.08131) * b1 + (ONE_HALF << shift) - 1) >> (SCALEBITS + shift)) + 128)








static void term_exit( void );
static void sigterm_handler( int sig );
static void term_init( void );

/* read a key without blocking */
static int read_key( void );
static int decode_interrupt_cb( void );
static int av_exit( int ret );
static int read_ffserver_streams( AVFormatContext *s, const char *filename );
static double get_sync_ipts( const AVOutputStream *ost );
static void write_frame( AVFormatContext *s, AVPacket *pkt, AVCodecContext *avctx, AVBitStreamFilterContext *bsfc );
static void do_audio_out( AVFormatContext *s, AVOutputStream *ost, AVInputStream *ist, unsigned char *buf, int size );
static void pre_process_video_frame( AVInputStream *ist, AVPicture *picture, void **bufp );
static void do_subtitle_out( AVFormatContext *s, AVOutputStream *ost, AVInputStream *ist, AVSubtitle *sub, int64_t pts );
static void do_video_out( AVFormatContext *s, AVOutputStream *ost, AVInputStream *ist, AVFrame *in_picture, int *frame_size );
static double psnr( double d );
static void do_video_stats( AVFormatContext *os, AVOutputStream *ost, int frame_size );
static void print_report( AVFormatContext **output_files, AVOutputStream **ost_table, int nb_ostreams, int is_last_report );
/* pkt = NULL means EOF (needed to flush decoder buffers) */
static int output_packet( AVInputStream *ist, int ist_index, AVOutputStream **ost_table, int nb_ostreams, const AVPacket *pkt );
static void print_sdp( AVFormatContext **avc, int n );
static int stream_index_from_inputs( AVFormatContext **input_files, int nb_input_files, AVInputFile *file_table, AVInputStream **ist_table, enum CodecType type, int programid );
/*
 * The following code is the main loop of the file converter
 */
static int av_encode( AVFormatContext **output_files, int nb_output_files, AVFormatContext **input_files, int nb_input_files, AVStreamMap *stream_maps, int nb_stream_maps );
#if 0
int file_read(const char *filename);
#endif

static void opt_format( const char *arg );
static int opt_default( const char *opt, const char *arg );
static void opt_video_rc_override_string( const char *arg );
static int opt_me_threshold( const char *opt, const char *arg );
static int opt_verbose( const char *opt, const char *arg );
static int opt_frame_rate( const char *opt, const char *arg );
static int opt_bitrate( const char *opt, const char *arg );
static void opt_frame_crop_top( const char *arg );
static void opt_frame_crop_bottom( const char *arg );
static void opt_frame_crop_left( const char *arg );
static void opt_frame_crop_right( const char *arg );
static void opt_frame_size( const char *arg );
static void opt_pad_color( const char *arg );
static void opt_frame_pad_top( const char *arg );
static void opt_frame_pad_bottom( const char *arg );
static void opt_frame_pad_left( const char *arg );
static void opt_frame_pad_right( const char *arg );
static void list_fmts( void(*get_fmt_string)( char *buf, int buf_size, int fmt ), int nb_fmts );
static void opt_frame_pix_fmt( const char *arg );
static void opt_frame_aspect_ratio( const char *arg );
static void opt_qscale( const char *arg );
static void opt_top_field_first( const char *arg );
static int opt_thread_count( const char *opt, const char *arg );
static void opt_audio_sample_fmt( const char *arg );
static int opt_audio_rate( const char *opt, const char *arg );
static int opt_audio_channels( const char *opt, const char *arg );
static void opt_video_channel( const char *arg );
static void opt_video_standard( const char *arg );
static void opt_codec( int *pstream_copy, char **pcodec_name, int codec_type, const char *arg );
static void opt_audio_codec( const char *arg );
static void opt_audio_tag( const char *arg );
static void opt_video_tag( const char *arg );

#ifdef CONFIG_VHOOK
static void add_frame_hooker(const char *arg);
#endif
static void opt_video_codec( const char *arg );
static void opt_subtitle_codec( const char *arg );
static void opt_map( const char *arg );
static void opt_map_meta_data( const char *arg );
static void opt_input_ts_scale( const char *arg );
static int opt_recording_time( const char *opt, const char *arg );
static int opt_start_time( const char *opt, const char *arg );
static int opt_rec_timestamp( const char *opt, const char *arg );
static int opt_input_ts_offset( const char *opt, const char *arg );
static enum CodecID find_codec_or_die( const char *name, int type, int encoder );
static void set_context_opts( void *ctx, void *opts_ctx, int flags );
static void opt_input_file( const char *filename );
static void check_audio_video_sub_inputs( int *has_video_ptr, int *has_audio_ptr, int *has_subtitle_ptr );
static void new_video_stream( AVFormatContext *oc );
static void new_audio_stream( AVFormatContext *oc );
static void new_subtitle_stream( AVFormatContext *oc );
static void opt_new_audio_stream( void );
static void opt_new_video_stream( void );
static void opt_new_subtitle_stream( void );
static void opt_output_file( const char *filename );
/* same option as mencoder */
static void opt_pass( const char *pass_str );
static int64_t getutime( void );
static void parse_matrix_coeffs( uint16_t *dest, const char *str );
static void opt_inter_matrix( const char *arg );
static void opt_intra_matrix( const char *arg );
/**
 * Trivial log callback.
 * Only suitable for show_help and similar since it lacks prefix handling.
 */
static void log_callback_help( void* ptr, int level, const char* fmt, va_list vl );
//Fernando:
static void log_callback_file( void* ptr, int level, const char* fmt, va_list vl );
static void show_help( void );
static void opt_target( const char *arg );
static void opt_vstats_file( const char *arg );
static void opt_vstats( void );
static int opt_bsf( const char *opt, const char *arg );
static int opt_preset( const char *opt, const char *arg );
//Fernando: 20080919
//void hex_dump(const uint8_t *buf, int buf_size);
void dumpPacketBuffer(AVPacketList *packet_buffer);
void dump_AVOutputFormat(struct AVOutputFormat *outputFormat);
void dump_AVInputFormat(struct AVInputFormat *inputFormat);
void dump_AVFormatContext(struct AVFormatContext *formatContext);
void dump_AVFormatContextArray(struct AVFormatContext **formatContextArray, int lenght);
//Fernando:
static AVPacket *add_to_pktbuf( AVPacketList **packet_buffer, AVPacket *pkt, AVPacketList **plast_pktl );
//Fernando:
void addAnotherImage(char* fileName);




static const OptionDef
        options[] = {
        /* main options */
        { "L", OPT_EXIT, { (void*) show_license }, "show license" }, { "h", OPT_EXIT, { (void*) show_help }, "show help" }, { "version", OPT_EXIT, { (void*) show_version }, "show version" }, { "formats", OPT_EXIT, { (void*) show_formats }, "show available formats, codecs, protocols, ..." }, { "f", HAS_ARG, { (void*) opt_format }, "force format", "fmt" }, { "i", HAS_ARG, { (void*) opt_input_file }, "input file name", "filename" }, { "y", OPT_BOOL, { (void*) &file_overwrite }, "overwrite output files" }, { "map", HAS_ARG
                | OPT_EXPERT, { (void*) opt_map }, "set input stream mapping", "file:stream[:syncfile:syncstream]" }, { "map_meta_data", HAS_ARG
                | OPT_EXPERT, { (void*) opt_map_meta_data }, "set meta data information of outfile from infile", "outfile:infile" }, { "t", OPT_FUNC2
                | HAS_ARG, { (void*) opt_recording_time }, "record or transcode \"duration\" seconds of audio/video", "duration" }, { "fs", HAS_ARG
                | OPT_INT64, { (void*) &limit_filesize }, "set the limit file size in bytes", "limit_size" }, //
        { "ss", OPT_FUNC2 | HAS_ARG, { (void*) opt_start_time }, "set the start time offset", "time_off" }, { "itsoffset", OPT_FUNC2
                | HAS_ARG, { (void*) opt_input_ts_offset }, "set the input ts offset", "time_off" }, { "itsscale", HAS_ARG, { (void*) opt_input_ts_scale }, "set the input ts scale", "stream:scale" }, { "title", HAS_ARG
                | OPT_STRING, { (void*) &str_title }, "set the title", "string" }, { "timestamp", OPT_FUNC2
                | HAS_ARG, { (void*) &opt_rec_timestamp }, "set the timestamp", "time" }, { "author", HAS_ARG
                | OPT_STRING, { (void*) &str_author }, "set the author", "string" }, { "copyright", HAS_ARG
                | OPT_STRING, { (void*) &str_copyright }, "set the copyright", "string" }, { "comment", HAS_ARG
                | OPT_STRING, { (void*) &str_comment }, "set the comment", "string" }, { "genre", HAS_ARG
                | OPT_STRING, { (void*) &str_genre }, "set the genre", "string" }, { "album", HAS_ARG
                | OPT_STRING, { (void*) &str_album }, "set the album", "string" }, { "dframes", OPT_INT
                | HAS_ARG, { (void*) &max_frames[CODEC_TYPE_DATA] }, "set the number of data frames to record", "number" }, { "benchmark", OPT_BOOL
                | OPT_EXPERT, { (void*) &do_benchmark }, "add timings for benchmarking" }, { "dump", OPT_BOOL
                | OPT_EXPERT, { (void*) &do_pkt_dump }, "dump each input packet" }, { "hex", OPT_BOOL
                | OPT_EXPERT, { (void*) &do_hex_dump }, "when dumping packets, also dump the payload" }, { "re", OPT_BOOL
                | OPT_EXPERT, { (void*) &rate_emu }, "read input at native frame rate", "" }, { "loop_input", OPT_BOOL
                | OPT_EXPERT, { (void*) &loop_input }, "loop (current only works with images)" }, { "loop_output", HAS_ARG
                | OPT_INT | OPT_EXPERT, { (void*) &loop_output }, "number of times to loop output in formats that support looping (0 loops forever)", "" }, { "v", HAS_ARG
                | OPT_FUNC2, { (void*) opt_verbose }, "set the logging verbosity level", "number" }, { "target", HAS_ARG, { (void*) opt_target }, "specify target file type (\"vcd\", \"svcd\", \"dvd\", \"dv\", \"dv50\", \"pal-vcd\", \"ntsc-svcd\", ...)", "type" }, { "threads", OPT_FUNC2
                | HAS_ARG | OPT_EXPERT, { (void*) opt_thread_count }, "thread count", "count" }, { "vsync", HAS_ARG
                | OPT_INT | OPT_EXPERT, { (void*) &video_sync_method }, "video sync method", "" }, { "async", HAS_ARG
                | OPT_INT | OPT_EXPERT, { (void*) &audio_sync_method }, "audio sync method", "" }, { "adrift_threshold", HAS_ARG
                | OPT_FLOAT | OPT_EXPERT, { (void*) &audio_drift_threshold }, "audio drift threshold", "threshold" }, { "vglobal", HAS_ARG
                | OPT_INT | OPT_EXPERT, { (void*) &video_global_header }, "video global header storage type", "" }, { "copyts", OPT_BOOL
                | OPT_EXPERT, { (void*) &copy_ts }, "copy timestamps" }, { "shortest", OPT_BOOL
                | OPT_EXPERT, { (void*) &opt_shortest }, "finish encoding within shortest input" }, //
        { "dts_delta_threshold", HAS_ARG | OPT_FLOAT | OPT_EXPERT, { (void*) &dts_delta_threshold }, "timestamp discontinuity delta threshold", "threshold" }, { "programid", HAS_ARG
                | OPT_INT | OPT_EXPERT, { (void*) &opt_programid }, "desired program number", "" },

        /* video options */
        { "b", OPT_FUNC2 | HAS_ARG | OPT_VIDEO, { (void*) opt_bitrate }, "set bitrate (in bits/s)", "bitrate" }, { "vb", OPT_FUNC2
                | HAS_ARG | OPT_VIDEO, { (void*) opt_bitrate }, "set bitrate (in bits/s)", "bitrate" }, { "vframes", OPT_INT
                | HAS_ARG | OPT_VIDEO, { (void*) &max_frames[CODEC_TYPE_VIDEO] }, "set the number of video frames to record", "number" }, { "r", OPT_FUNC2
                | HAS_ARG | OPT_VIDEO, { (void*) opt_frame_rate }, "set frame rate (Hz value, fraction or abbreviation)", "rate" }, { "s", HAS_ARG
                | OPT_VIDEO, { (void*) opt_frame_size }, "set frame size (WxH or abbreviation)", "size" }, { "aspect", HAS_ARG
                | OPT_VIDEO, { (void*) opt_frame_aspect_ratio }, "set aspect ratio (4:3, 16:9 or 1.3333, 1.7777)", "aspect" }, { "pix_fmt", HAS_ARG
                | OPT_EXPERT | OPT_VIDEO, { (void*) opt_frame_pix_fmt }, "set pixel format, 'list' as argument shows all the pixel formats supported", "format" }, { "croptop", HAS_ARG
                | OPT_VIDEO, { (void*) opt_frame_crop_top }, "set top crop band size (in pixels)", "size" }, { "cropbottom", HAS_ARG
                | OPT_VIDEO, { (void*) opt_frame_crop_bottom }, "set bottom crop band size (in pixels)", "size" }, { "cropleft", HAS_ARG
                | OPT_VIDEO, { (void*) opt_frame_crop_left }, "set left crop band size (in pixels)", "size" }, { "cropright", HAS_ARG
                | OPT_VIDEO, { (void*) opt_frame_crop_right }, "set right crop band size (in pixels)", "size" }, { "padtop", HAS_ARG
                | OPT_VIDEO, { (void*) opt_frame_pad_top }, "set top pad band size (in pixels)", "size" }, { "padbottom", HAS_ARG
                | OPT_VIDEO, { (void*) opt_frame_pad_bottom }, "set bottom pad band size (in pixels)", "size" }, { "padleft", HAS_ARG
                | OPT_VIDEO, { (void*) opt_frame_pad_left }, "set left pad band size (in pixels)", "size" }, { "padright", HAS_ARG
                | OPT_VIDEO, { (void*) opt_frame_pad_right }, "set right pad band size (in pixels)", "size" }, { "padcolor", HAS_ARG
                | OPT_VIDEO, { (void*) opt_pad_color }, "set color of pad bands (Hex 000000 thru FFFFFF)", "color" }, { "intra", OPT_BOOL
                | OPT_EXPERT | OPT_VIDEO, { (void*) &intra_only }, "use only intra frames" }, { "vn", OPT_BOOL
                | OPT_VIDEO, { (void*) &video_disable }, "disable video" }, { "vdt", OPT_INT
                | HAS_ARG | OPT_EXPERT | OPT_VIDEO, { (void*) &video_discard }, "discard threshold", "n" }, { "qscale", HAS_ARG
                | OPT_EXPERT | OPT_VIDEO, { (void*) opt_qscale }, "use fixed video quantizer scale (VBR)", "q" }, { "rc_override", HAS_ARG
                | OPT_EXPERT | OPT_VIDEO, { (void*) opt_video_rc_override_string }, "rate control override for specific intervals", "override" }, { "vcodec", HAS_ARG
                | OPT_VIDEO, { (void*) opt_video_codec }, "force video codec ('copy' to copy stream)", "codec" }, { "me_threshold", HAS_ARG
                | OPT_FUNC2 | OPT_EXPERT | OPT_VIDEO, { (void*) opt_me_threshold }, "motion estimaton threshold", "threshold" }, { "sameq", OPT_BOOL
                | OPT_VIDEO, { (void*) &same_quality }, "use same video quality as source (implies VBR)" }, { "pass", HAS_ARG
                | OPT_VIDEO, { (void*) &opt_pass }, "select the pass number (1 or 2)", "n" }, { "passlogfile", HAS_ARG
                | OPT_STRING | OPT_VIDEO, { (void*) &pass_logfilename }, "select two pass log file name", "file" }, { "deinterlace", OPT_BOOL
                | OPT_EXPERT | OPT_VIDEO, { (void*) &do_deinterlace }, "deinterlace pictures" }, { "psnr", OPT_BOOL
                | OPT_EXPERT | OPT_VIDEO, { (void*) &do_psnr }, "calculate PSNR of compressed frames" }, { "vstats", OPT_EXPERT
                | OPT_VIDEO, { (void*) &opt_vstats }, "dump video coding statistics to file" }, { "vstats_file", HAS_ARG
                | OPT_EXPERT | OPT_VIDEO, { (void*) opt_vstats_file }, "dump video coding statistics to file", "file" },
#ifdef CONFIG_VHOOK
                {   "vhook", HAS_ARG | OPT_EXPERT | OPT_VIDEO,
                    {   (void*)add_frame_hooker}, "insert video processing module", "module"},
#endif
                { "intra_matrix", HAS_ARG | OPT_EXPERT | OPT_VIDEO, { (void*) opt_intra_matrix }, "specify intra matrix coeffs", "matrix" }, { "inter_matrix", HAS_ARG
                        | OPT_EXPERT | OPT_VIDEO, { (void*) opt_inter_matrix }, "specify inter matrix coeffs", "matrix" }, { "top", HAS_ARG
                        | OPT_EXPERT | OPT_VIDEO, { (void*) opt_top_field_first }, "top=1/bottom=0/auto=-1 field first", "" }, { "dc", OPT_INT
                        | HAS_ARG | OPT_EXPERT | OPT_VIDEO, { (void*) &intra_dc_precision }, "intra_dc_precision", "precision" }, { "vtag", HAS_ARG
                        | OPT_EXPERT | OPT_VIDEO, { (void*) opt_video_tag }, "force video tag/fourcc", "fourcc/tag" }, { "newvideo", OPT_VIDEO, { (void*) opt_new_video_stream }, "add a new video stream to the current output stream" }, { "qphist", OPT_BOOL
                        | OPT_EXPERT | OPT_VIDEO, { (void *) &qp_hist }, "show QP histogram" },

                /* audio options */
                { "ab", OPT_FUNC2 | HAS_ARG | OPT_AUDIO, { (void*) opt_bitrate }, "set bitrate (in bits/s)", "bitrate" }, { "aframes", OPT_INT
                        | HAS_ARG | OPT_AUDIO, { (void*) &max_frames[CODEC_TYPE_AUDIO] }, "set the number of audio frames to record", "number" }, { "aq", OPT_FLOAT
                        | HAS_ARG | OPT_AUDIO, { (void*) &audio_qscale }, "set audio quality (codec-specific)", "quality", }, { "ar", HAS_ARG
                        | OPT_FUNC2 | OPT_AUDIO, { (void*) opt_audio_rate }, "set audio sampling rate (in Hz)", "rate" }, { "ac", HAS_ARG
                        | OPT_FUNC2 | OPT_AUDIO, { (void*) opt_audio_channels }, "set number of audio channels", "channels" }, { "an", OPT_BOOL
                        | OPT_AUDIO, { (void*) &audio_disable }, "disable audio" }, { "acodec", HAS_ARG
                        | OPT_AUDIO, { (void*) opt_audio_codec }, "force audio codec ('copy' to copy stream)", "codec" }, { "atag", HAS_ARG
                        | OPT_EXPERT | OPT_AUDIO, { (void*) opt_audio_tag }, "force audio tag/fourcc", "fourcc/tag" }, { "vol", OPT_INT
                        | HAS_ARG | OPT_AUDIO, { (void*) &audio_volume }, "change audio volume (256=normal)", "volume" }, //
                { "newaudio", OPT_AUDIO, { (void*) opt_new_audio_stream }, "add a new audio stream to the current output stream" }, { "alang", HAS_ARG
                        | OPT_STRING | OPT_AUDIO, { (void *) &audio_language }, "set the ISO 639 language code (3 letters) of the current audio stream", "code" }, { "sample_fmt", HAS_ARG
                        | OPT_EXPERT | OPT_AUDIO, { (void*) opt_audio_sample_fmt }, "set sample format, 'list' as argument shows all the sample formats supported", "format" },

                /* subtitle options */
                { "sn", OPT_BOOL | OPT_SUBTITLE, { (void*) &subtitle_disable }, "disable subtitle" }, { "scodec", HAS_ARG
                        | OPT_SUBTITLE, { (void*) opt_subtitle_codec }, "force subtitle codec ('copy' to copy stream)", "codec" }, { "newsubtitle", OPT_SUBTITLE, { (void*) opt_new_subtitle_stream }, "add a new subtitle stream to the current output stream" }, { "slang", HAS_ARG
                        | OPT_STRING | OPT_SUBTITLE, { (void *) &subtitle_language }, "set the ISO 639 language code (3 letters) of the current subtitle stream", "code" },

                /* grab options */
                { "vc", HAS_ARG | OPT_EXPERT | OPT_VIDEO | OPT_GRAB, { (void*) opt_video_channel }, "set video grab channel (DV1394 only)", "channel" }, { "tvstd", HAS_ARG
                        | OPT_EXPERT | OPT_VIDEO | OPT_GRAB, { (void*) opt_video_standard }, "set television standard (NTSC, PAL (SECAM))", "standard" }, { "isync", OPT_BOOL
                        | OPT_EXPERT | OPT_GRAB, { (void*) &input_sync }, "sync read on input", "" },

                /* muxer options */
                { "muxdelay", OPT_FLOAT | HAS_ARG | OPT_EXPERT, { (void*) &mux_max_delay }, "set the maximum demux-decode delay", "seconds" }, { "muxpreload", OPT_FLOAT
                        | HAS_ARG | OPT_EXPERT, { (void*) &mux_preload }, "set the initial demux-decode delay", "seconds" },

                { "absf", OPT_FUNC2 | HAS_ARG | OPT_AUDIO | OPT_EXPERT, { (void*) opt_bsf }, "", "bitstream_filter" }, { "vbsf", OPT_FUNC2
                        | HAS_ARG | OPT_VIDEO | OPT_EXPERT, { (void*) opt_bsf }, "", "bitstream_filter" }, { "sbsf", OPT_FUNC2
                        | HAS_ARG | OPT_SUBTITLE | OPT_EXPERT, { (void*) opt_bsf }, "", "bitstream_filter" },

                { "apre", OPT_FUNC2 | HAS_ARG | OPT_AUDIO | OPT_EXPERT, { (void*) opt_preset }, "", "preset" }, { "vpre", OPT_FUNC2
                        | HAS_ARG | OPT_VIDEO | OPT_EXPERT, { (void*) opt_preset }, "", "preset" }, { "spre", OPT_FUNC2
                        | HAS_ARG | OPT_SUBTITLE | OPT_EXPERT, { (void*) opt_preset }, "", "preset" },

                { "default", OPT_FUNC2 | HAS_ARG | OPT_AUDIO | OPT_VIDEO
                        | OPT_EXPERT, { (void*) opt_default }, "generic catch all option", "" }, { NULL, }, };






int ffmpeg_func( int argc, char **argv );
