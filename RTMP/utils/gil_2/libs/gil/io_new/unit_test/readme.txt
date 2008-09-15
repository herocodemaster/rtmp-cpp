
The problem with read_and_convert_view is probably not because of an interface error, but
rather the fact that the compiler screws up. The following test app serves as a proof:

#include <boost/gil/gil_all.hpp>
#include <boost/gil/extension/io_new/tiff_read.hpp>

using namespace std;
using namespace boost;
using namespace gil;

int main(int argc, char *argv[])
{
    string filename( ".\\test_images\\tiff\\test.tif" );

    rgb8_image_t img( 200, 133 );
    read_and_convert_view( filename, view( img ), tiff_tag() );

   return 0;
}

Compiling will produce errors using MSVC 8. 

1>c:\gil\gil_2\boost\gil\extension\io_new\detail\tiff_io_read.hpp(103) : error C2039: 'get_property' : is not a member of 'std::basic_string<_Elem,_Traits,_Ax>'
[snip]

The reason is that the compiler will use the wrong functions. For some reasons the enable_if
statement don't prevent the compiler from using the wrong function.

Workaround is to change tiff_read.hpp and comment out 

/*
#include "detail/read_image_info.hpp"
#include "detail/read_view.hpp"
#include "detail/read_image.hpp"
#include "detail/read_and_convert_image.hpp"
*/
#include "detail/read_and_convert_view.hpp"

Note the only header that's not commented out is the one we need right now.
Now everything works as expected. The compiler is probably confused by too much metaprogramming from
the former includes.

