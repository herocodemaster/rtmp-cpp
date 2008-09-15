#include <boost/shared_ptr.hpp>

#include <boost/gil/gil_all.hpp>

#include <boost/gil/extension/io/bmp_io.hpp>
#include <boost/gil/extension/sdl/sdl_wrapper.hpp>

using namespace std;
using namespace boost;
using namespace gil;
using namespace sdl;

int main( int argc, char* argv[] )
{
    typedef sdl_window<> window_t;
    typedef shared_ptr< window_t > window_ptr_t;

    window_ptr_t win( new window_t( 640
                             , 480 ));

    sdl_service ss;
    ss.add_window( win );
    ss.run();

    return 0;
}
