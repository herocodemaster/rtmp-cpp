// plasma.cpp : Defines the entry point for the console application.
//

#include <iostream>
#include <tchar.h>

#include "boost/gil/gil_all.hpp"

#include "boost/gil/extension/sdl/sdl_wrapper.hpp"
#include "boost/gil/extension/toolbox/hsv.hpp"

using namespace std;
using namespace boost;
using namespace gil;
using namespace sdl;

typedef point2<ptrdiff_t> point_t;

// taken from http://student.kuleuven.be/~m0216922/CG/plasma.html

inline float plasma_func( float x, float y )
{
   return (   128.f + ( 128.f * sin( x / 16.f ))
            + 128.f + ( 128.f * sin( y / 8.f  ))
            + 128.f + ( 128.f * sin((x + y) / 16.f ))
            + 128.f + ( 128.f * sin( sqrt( float( x * x + y * y )) / 8.f ))
        ) / 4.f;
}

struct create_palette
{
   create_palette()
   : _step( 1 / 256.f )
   , _current_value( 0.f )
   {}

   void operator()( bgra8_pixel_t& p )
   {
      color_convert( hsv32f_pixel_t( _current_value, 1.f, 1.f ), p );

      _current_value += _step;
   }

private:

   float _step;
   float _current_value;
};

struct my_redraw_event_handler
{
   my_redraw_event_handler( const bgra8_view_t& v )
   : _view( v )
   {
   }

   void redraw( const bgra8_view_t& sdl_view )
   {
      copy_pixels( _view
                 , sdl_view );
   }

private:

   bgra8_view_t _view;
};

class my_timer_handler
{
public:

   my_timer_handler()
   : _step( 0 )
   , _palette( 256, 1 )
   , _view_palette( view( _palette ))
   , _buffer()
   , _view_buffer( view( _buffer ))
   , _counter( 0 )
   {
      for_each_pixel( _view_palette, create_palette() );
   }

   bool time_elapsed()
   {
      for( int y = 0; y < _view.height(); ++y )
      {
         bgra8_view_t::x_iterator x_it = _view.row_begin( y );

         for( int x = 0; x < _view.width(); ++x, ++x_it )
         {
            bits8 index = at_c<0>( _view_buffer( x, y ) );

            index += _step;
            if( index >= 256 ) index -= 256;

            *x_it = _view_palette( index, 0 );
         }
      }

      _step++;

      // true for redraw
      return true;
   }

   void set_img( bgra8_view_t v )
   {
      _view = v;

      _buffer.recreate( _view.dimensions() );
      _view_buffer = view( _buffer );

      for( int y = 0; y < _view_buffer.height(); ++y )
      {
         gray8_view_t::x_iterator x_it = _view_buffer.row_begin( y );

         for( int x = 0; x < _view_buffer.width(); ++x, ++x_it )
         {
            *x_it = static_cast<bits8>( plasma_func( x, y ));
         }
      }
   }

private:

   bits8 _step;

   bgra8_view_t _view;

   bgra8_image_t _palette;
   bgra8_view_t  _view_palette;

   gray8_image_t _buffer;
   gray8_view_t  _view_buffer;

   unsigned int _counter;
};

int _tmain(int argc, _TCHAR* argv[])
{
   point_t dims(640,480);

   bgra8_image_t img( dims );
   fill_pixels( view( img )
              , bgra8_pixel_t( 0, 0, 0, 0 ));


   sdl_service ss;

   typedef sdl_window< sdl::detail::default_keyboard_event_handler
                     , my_redraw_event_handler
                     , my_timer_handler
                     > window_t;

   typedef shared_ptr< window_t > window_ptr_t;

   typedef shared_ptr< my_redraw_event_handler > rh_ptr_t;
   rh_ptr_t rh_ptr( new my_redraw_event_handler( view( img ) ));

   window_ptr_t win( new window_t( dims.x
                                 , dims.y
                                 , rh_ptr ));

   win->my_timer_handler::set_img( view( img ));
   win->set_timer( 20 );


   ss.add_window( win );
   ss.run();

	return 0;
}
