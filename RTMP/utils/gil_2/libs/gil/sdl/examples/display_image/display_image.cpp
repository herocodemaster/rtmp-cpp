#include <boost/shared_ptr.hpp>

#include <boost/gil/gil_all.hpp>

#include <boost/gil/extension/io/bmp_io.hpp>
#include <boost/gil/extension/sdl/sdl_wrapper.hpp>

using namespace std;
using namespace boost;
using namespace gil;
using namespace sdl;

struct my_redraw_handler
{
   my_redraw_handler( rgb8_view_t v )
   : _view( v ) {}

   void redraw( const bgra8_view_t& sdl_view )
   {
      copy_pixels( color_converted_view<bgra8_pixel_t>( _view )
                 , sdl_view                                     );
   }

private:

   rgb8_view_t _view;
};

int main( int argc, char* argv[] )
{
   // load the image
   rgb8_image_t img;
   bmp_read_image( "..\\..\\flower.bmp", img );


   // define window type
   typedef sdl_window< sdl::detail::default_keyboard_event_handler
                     , my_redraw_handler
                     > window_t;

   typedef shared_ptr< window_t > window_ptr_t;

   // create redraw handler
   typedef shared_ptr< my_redraw_handler > rh_ptr_t;
   rh_ptr_t rh_ptr( new my_redraw_handler( view( img ) ));


   // create window
   window_ptr_t win( new window_t( view( img ).width()
                                 , view( img ).height() 
                                 , rh_ptr ));

   // create service and let's roll
   sdl_service ss;
   ss.add_window( win );
   ss.run();

	return 0;
}
