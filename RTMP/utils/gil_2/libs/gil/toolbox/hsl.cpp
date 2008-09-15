// hsl.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

#include <boost/gil/extension/toolbox/hsl.hpp>
#include <boost/gil/extension/toolbox/hsv.hpp>

using namespace std;
using namespace boost;
using namespace gil;
using namespace toolbox;



int _tmain(int argc, _TCHAR* argv[])
{
   {
      gray32f_pixel_t g( 1.f );

      float red_1 = channel_convert< bits32f >( 1.f );
      float red_2 = channel_convert< bits32f >( (bits32f) 1.f );

      int i = 9;
   }


   {
      rgb8_pixel_t p( 128, 0, 128 );

      hsl32f_pixel_t h;

      color_convert( p, h );
      color_convert( h, p );

      int i = 9;
   }

   {
      size_t width  = 640;
      size_t height = 480;

      hsl32f_image_t hsl_img( width, height );
      hsv32f_image_t hsv_img( width, height );

      for( size_t y = 0; y < height; y++ )
      {
         hsl32f_view_t::x_iterator hsl_x_it = view( hsl_img ).row_begin( y );
         hsv32f_view_t::x_iterator hsv_x_it = view( hsv_img ).row_begin( y );

         float v = static_cast<float>( height -  y ) 
                 / height;

         for( size_t x = 0; x < width; x++ )
         {
            float hue = ( x + 1.f ) / width;

            hsl_x_it[x] = hsl32f_pixel_t( hue, 1.0, v );
            hsv_x_it[x] = hsv32f_pixel_t( hue, 1.0, v );
         }
      }

      bmp_write_view( ".\\hsl_test.bmp"
                    , color_converted_view<rgb8_pixel_t>( color_converted_view<rgb32f_pixel_t>( view( hsl_img ))));

      bmp_write_view( ".\\hsv_test.bmp"
                    , color_converted_view<rgb8_pixel_t>( color_converted_view<rgb32f_pixel_t>( view( hsv_img ))));
   }


   {
      rgb8_image_t rgb_img;
      bmp_read_image( ".\\flower.bmp", rgb_img );

      hsl32f_image_t hsl_img( view( rgb_img ).dimensions() );

      copy_pixels( color_converted_view<hsl32f_pixel_t>( view( rgb_img ))
                 , view( hsl_img ));

      bmp_write_view( ".\\flower_hsl.bmp"
                    , color_converted_view<rgb8_pixel_t>(view( rgb_img )));
      
   }

   {
      rgb8_image_t rgb_img;
      bmp_read_image( ".\\flower.bmp", rgb_img );

      hsv32f_image_t hsv_img( view( rgb_img ).dimensions() );

      copy_pixels( color_converted_view<hsv32f_pixel_t>( view( rgb_img ))
                 , view( hsv_img ));

      bmp_write_view( ".\\flower_hsv.bmp"
                    , color_converted_view<rgb8_pixel_t>(view( rgb_img )));
      
   }

	return 0;
}

