typedef struct AVFormatContext
{

    int year;
    int track;
    int ctx_flags;
    int bit_rate;
    int cur_len;
    int index_built;
    int mux_rate;
    int packet_size;
    int preload;
    int max_delay;
    int loop_output;
    int flags;
    int loop_input;
    int debug;
    int max_analyze_duration;
    int keylen;

    unsigned int nb_streams;
    unsigned int probesize;
    unsigned int nb_programs;
    unsigned int max_index_size;
    unsigned int max_picture_buffer;
    unsigned int nb_chapters;

    int64_t timestamp;
    int64_t start_time;
    int64_t duration;
    int64_t file_size;
    int64_t data_offset;

    const uint8_t *cur_ptr;
    const uint8_t *key;


    char filename[1024];
    char title[512];
    char author[512];
    char copyright[512];
    char comment[512];
    char album[512];
    char genre[32];



    enum CodecID video_codec_id;
    enum CodecID audio_codec_id;
    enum CodecID subtitle_codec_id;



    struct AVInputFormat *iformat;
    struct AVOutputFormat *oformat;
    const AVClass *av_class;
    ByteIOContext *pb;
    AVStream *streams[MAX_STREAMS];
    struct AVPacketList *packet_buffer;
    AVStream *cur_st;
    AVProgram **programs;
    AVChapter **chapters;
    struct AVPacketList *raw_packet_buffer;
    struct AVPacketList *raw_packet_buffer_end;
    struct AVPacketList *packet_buffer_end;

    AVPacket cur_pkt;


    void *priv_data;


} AVFormatContext;



typedef struct AVInputFormat
{
    const char *name;
    const char *long_name;
    const char *extensions;

    int priv_data_size;
    int flags;
    int value;

    const struct AVCodecTag * const *codec_tag;
    struct AVInputFormat *next;



    int (*read_probe)(AVProbeData *);
    int (*read_header)(struct AVFormatContext *, AVFormatParameters *ap);
    int (*read_packet)(struct AVFormatContext *, AVPacket *pkt);
    int (*read_close)(struct AVFormatContext *);
    int (*read_seek)(struct AVFormatContext *, int stream_index, int64_t timestamp, int flags);
    int64_t (*read_timestamp)(struct AVFormatContext *s, int stream_index, int64_t *pos, int64_t pos_limit);
    int (*read_play)(struct AVFormatContext *);
    int (*read_pause)(struct AVFormatContext *);

} AVInputFormat;


typedef struct AVOutputFormat
{
    const char *name;
    const char *long_name;
    const char *mime_type;
    const char *extensions;

    int priv_data_size;
    int flags;
    enum CodecID audio_codec;
    enum CodecID video_codec;
    enum CodecID subtitle_codec;

    const struct AVCodecTag * const *codec_tag;
    struct AVOutputFormat *next;

    int (*write_header)(struct AVFormatContext *);
    int (*write_packet)(struct AVFormatContext *, AVPacket *pkt);
    int (*write_trailer)(struct AVFormatContext *);
    int (*set_parameters)(struct AVFormatContext *, AVFormatParameters *);
    int (*interleave_packet)(struct AVFormatContext *, AVPacket *out, AVPacket *in, int flush);

} AVOutputFormat;





typedef struct AVStream
{
    int index;
    int id;
    int pts_wrap_bits;
    int stream_copy;
    int disposition;
    int last_IP_duration;
    int nb_index_entries;
    unsigned int index_entries_allocated_size;

    float quality;

    char language[4];
    char *filename;

    int64_t cur_dts;
    int64_t last_IP_pts;
    int64_t first_dts;
    int64_t start_time;
    int64_t duration;
    int64_t nb_frames;
    int64_t unused[4+1];
    int64_t pts_buffer[MAX_REORDER_DELAY+1];



    void *priv_data;

    enum AVDiscard discard;
    struct AVCodecParserContext *parser;
    enum AVStreamParseType need_parsing;
    struct AVFrac pts;
    AVRational time_base;
    AVIndexEntry *index_entries;
    AVCodecContext *codec;
    AVRational r_frame_rate;
    AVProbeData probe_data;
    AVRational sample_aspect_ratio;
} AVStream;
