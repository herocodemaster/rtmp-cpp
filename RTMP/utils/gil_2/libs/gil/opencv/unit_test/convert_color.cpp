#include "stdafx.h"

#include <boost\test\unit_test.hpp>

#include <boost\gil\extension\opencv\convert_color.hpp>

#include <boost\gil\extension\io_new\png_all.hpp>

using namespace boost::gil;
using namespace boost::gil::opencv;

BOOST_AUTO_TEST_CASE( test_convert_color )
{
    rgb32f_image_t src;
    read_and_convert_image( "..\\in\\in.png", src, png_tag() ); 

    xyz32f_image_t dst( src.dimensions() );

    cvtcolor( view( src )
            , view( dst )
            );

    rgb32f_image_t out( src.dimensions() );

    cvtcolor( view( dst )
            , view( out )
            );

    write_view( "..\\out\\convert_color.png"
              , color_converted_view< rgb8_pixel_t >( view( out ))
              , png_tag()
              );
}

BOOST_AUTO_TEST_CASE( test_convert_color_bit_aligned )
{
    bgr565_image_t src;
    rgba8_image_t dst( src.dimensions() );

/*
    // @todo: not implemented yet
    cvtcolor( view( src )
            , view( dst )
            );
*/
}
/*
BOOST_AUTO_TEST_CASE( test_convert_color_using_xyz_colorspace )
{
    xyz32f_image_t src;
    rgb32f_image_t dst( src.dimensions() );

    cvtcolor( view( src )
            , view( dst )
            );
}
*/