// test.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

#include <boost/gil/extension/toolbox/hsl.hpp>
#include <boost/gil/extension/toolbox/hsv.hpp>
#include <boost/gil/extension/toolbox/lab.hpp>
#include <boost/gil/extension/toolbox/xyz.hpp>

using namespace std;
using namespace boost;
using namespace gil;

int main(int argc, char* argv[])
{
   const rgb8_pixel_t rgb_src( 128, 78, 99 );

   // hsl
   {
      hsl32f_pixel_t h32;
      color_convert( rgb_src, h32 );

      rgb8_pixel_t rgb_dst;
      color_convert( h32, rgb_dst );

      assert( rgb_src == rgb_dst );
   }

   // hsv
   {
      hsv32f_pixel_t h32;
      color_convert( rgb_src, h32 );

      rgb8_pixel_t rgb_dst;
      color_convert( h32, rgb_dst );

      assert( rgb_src == rgb_dst );
   }

   // lab
   {
      lab32f_pixel_t l32;
      color_convert( rgb_src, l32 );

      rgb8_pixel_t rgb_dst;
      color_convert( l32, rgb_dst );

      assert( rgb_src == rgb_dst );
   }

   // xyz
   {
      xyz32f_pixel_t x32;
      color_convert( rgb_src, x32 );

      rgb8_pixel_t rgb_dst;
      color_convert( x32, rgb_dst );

      assert( rgb_src == rgb_dst );
   }

    return 0;
}