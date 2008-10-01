//Fernando: 20080908
//#define LogStr(str)  printf ( "************************** %s: %s - %s-%d **************************\n", __func__, str, __FILE__, __LINE__)
#define LogStr(str) av_log(NULL, AV_LOG_ERROR, "************************** %s: %s - %s-%d **************************\n", __func__, str, __FILE__, __LINE__);

#include "libavfunc/ffmpeg_func.h"



int main( int argc, char **argv )
{
    ffmpeg_func(argc, argv);
    return av_exit(0);
}
