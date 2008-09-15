#include "stdafx.h"

#include <boost\test\unit_test.hpp>

#include <boost\gil\extension\opencv\opencv_all.hpp>

#include <boost\gil\extension\io_new\png_all.hpp>

using namespace boost::gil;
using namespace boost::gil::opencv;

BOOST_AUTO_TEST_CASE( test_convert_scale )
{
    rgb32f_image_t src( 640, 480 );
    fill_pixels( view( src ), rgb32f_pixel_t( 1000.0, 0.0, 34.0 ) );

    rgb16_image_t dst( view( src ).dimensions() );

    convert_scale( view( src )
                 , view( dst )
                 );

    write_view( "..\\out\\convert_scale.png", view( dst ), png_tag() );
}

BOOST_AUTO_TEST_CASE( test_convert_scale_abs )
{
    rgb32f_image_t src( 640, 480 );
    fill_pixels( view( src ), rgb32f_pixel_t( 1000.0, 0.0, 34.0 ) );

    rgb8_image_t dst( view( src ).dimensions() );

    convert_scale( view( src )
                 , view( dst )
                 );

    write_view( "..\\out\\convert_scale_abs.png", view( dst ), png_tag() );
}
