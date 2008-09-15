#include "stdafx.h"

#define ADD_FS_PATH_SUPPORT
#define ENABLE_GRAY_ALPHA

#include <boost/gil/extension/io_new/png_all.hpp>

#include <boost/test/unit_test.hpp>

using namespace std;
using namespace boost::gil;
namespace fs = boost::filesystem;

typedef png_tag tag_t;

namespace png_test {

// Test will loop through the "in" folder to read and convert
// the png's to rgb8_image_t's. Which then will be written in
// the "out" folder.
// 
// The file name structure is as followed:
// 
// g04i2c08.png
// || |||+---- bit-depth
// || ||+----- color-type (descriptive)
// || |+------ color-type (numerical)
// || +------- interlaced or non-interlaced
// |+--------- parameter of test (in this case gamma-value)
// +---------- test feature (in this case gamma)

BOOST_AUTO_TEST_CASE( my_png_file_format_test )
{
   string in ( "..\\test_images\\png\\PngSuite\\" );
   string out( "..\\test\\png\\" );

   fs::path in_path = fs::system_complete( fs::path( in, fs::native ) );

   if ( fs::is_directory( in_path ) )
   {
      fs::directory_iterator end_iter;
      for( fs::directory_iterator dir_itr( in_path )
         ; dir_itr != end_iter
         ; ++dir_itr
         )
      {
         if ( fs::is_regular( dir_itr->status() ) 
            && ( fs::extension( dir_itr->path() ) == ".PNG" ))
         {
            rgb8_image_t img;
            string filename = in + dir_itr->path().leaf();
            read_and_convert_image( filename, img, tag_t() );

            write_view( out + fs::basename( dir_itr->path() ) + ".png"
                      , view( img )
                      , png_tag()
                      );
         }
      }
   }
}

} // namespace png