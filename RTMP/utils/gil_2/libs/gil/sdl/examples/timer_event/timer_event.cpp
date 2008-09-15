#include <boost/random.hpp>
#include <boost/shared_ptr.hpp>

#include <boost/gil/gil_all.hpp>

#include <boost/gil/extension/io/bmp_io.hpp>
#include <boost/gil/extension/sdl/sdl_wrapper.hpp>

using namespace std;
using namespace boost;
using namespace gil;
using namespace sdl;

class my_timer_handler
{
public:

   my_timer_handler()
   : _color( 0, 0, 0 )
   , _range( 0, 255 )
   , _die( _random_generator, _range )
   {}

   bool time_elapsed()
   {
      get_color( _color, red_t()   ) = _die();
      get_color( _color, green_t() ) = _die();
      get_color( _color, blue_t()  ) = _die();

      fill_pixels( _view
                 , _color );

      // true for redraw
      return true;
   }

   void set_img( rgb8_view_t v )
   {
      _view = v;
   }

private:

   rgb8_view_t _view;
   rgb8_pixel_t _color;

   mt19937        _random_generator;
   uniform_int<>  _range;

   variate_generator< mt19937&
                    , uniform_int<>
                    > _die;

};

struct my_redraw_handler
{
   my_redraw_handler( rgb8_view_t v )
   : _view( v ) {}

   void redraw( const bgra8_view_t& sdl_view )
   {
      copy_pixels( color_converted_view<bgra8_pixel_t>( _view )
                 , sdl_view                                     );

      std::cout << "redraw" << std::endl;
   }

private:

   rgb8_view_t _view;
};

int main( int argc, char* argv[] )
{
   sdl_service ss;

   // create black image
   rgb8_image_t img( 640, 480 );
   fill_pixels( view( img ), rgb8_pixel_t( 0, 0, 0 ));

   // define window type
   typedef sdl_window< sdl::detail::default_keyboard_event_handler
                     , my_redraw_handler
                     , my_timer_handler
                     > window_t;

   typedef shared_ptr< window_t > window_ptr_t;

   // create redraw handler
   typedef shared_ptr< my_redraw_handler > rh_ptr_t;
   rh_ptr_t rh_ptr( new my_redraw_handler( view( img ) ));


   // create window
   window_ptr_t win( new window_t( view( img ).width()
                                 , view( img ).height() 
                                 , rh_ptr ));

   // set the image and clock the timer
   win->my_timer_handler::set_img( view( img ));
   win->set_timer( 100 );

   // create service and let's roll
   ss.add_window( win );
   ss.run();

	return 0;
}
