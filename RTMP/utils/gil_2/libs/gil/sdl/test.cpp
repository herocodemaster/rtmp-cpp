// test.cpp : Defines the entry point for the console application.
//
#include "stdafx.h"

#include "boost/gil/gil_all.hpp"

#include "boost/gil/extension/io/bmp_io.hpp"
#include "boost/gil/extension/sdl/sdl_wrapper.hpp"

#include "boost/gil/extension/toolbox/hsl.hpp"
#include "boost/gil/extension/toolbox/hsl_algorithms.hpp"

using namespace std;
using namespace boost;
using namespace gil;
using namespace sdl;

struct my_timer_event_handler
{
   // Return true to trigger redraw.
   bool time_elapsed()
   {
      std::cout << "time elapsed" << std::endl;

      return false;
   }
};

class my_keyboard_event_handler
{
public:

   my_keyboard_event_handler() {}

   bool key_up()
   {
      toolbox::shift_hue( view( _hsl_img ), 0.1f );

      copy_pixels( color_converted_view<rgb8_pixel_t>( view( _hsl_img ))
                 , _view );

      return true;
   }

   void set_img( rgb8_view_t v )
   {
      _view = v;

      _hsl_img.recreate( _view.dimensions() );

      copy_pixels( color_converted_view<hsl32f_pixel_t>( _view )
                 , view( _hsl_img ));
   }

private:

   rgb8_view_t    _view;
   hsl32f_image_t _hsl_img;

};

struct my_redraw_event_handler
{
   my_redraw_event_handler( rgb8_view_t v )
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
   sdl_service ss;

   rgb8_image_t img;
   bmp_read_image( "flower.bmp", img );

   typedef sdl_window< my_keyboard_event_handler
                     , my_redraw_event_handler
                     , my_timer_event_handler
                     > window_t;

   typedef shared_ptr< window_t > window_ptr_t;

   typedef shared_ptr< my_redraw_event_handler > rh_ptr_t;
   rh_ptr_t rh_ptr( new my_redraw_event_handler( view( img ) ));

   window_ptr_t win( new window_t( view( img ).width()
                                 , view( img ).height() 
                                 , rh_ptr ));

   win->set_img( view( img ));

   win->set_timer( 1000 );

   ss.add_window( win );
   ss.run();

	return 0;
}
