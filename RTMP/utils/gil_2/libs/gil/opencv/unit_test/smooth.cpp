#include "stdafx.h"

#include <boost\test\unit_test.hpp>

#include <boost\gil\extension\opencv\smooth.hpp>

#include <boost\gil\extension\io_new\png_all.hpp>

using namespace boost::gil;
using namespace boost::gil::opencv;

BOOST_AUTO_TEST_CASE( test_smooth_gaussian )
{
    rgb8_image_t src;
    read_image( "..\\in\\in.png", src, png_tag() ); 

    rgb8_image_t dst( view( src ).dimensions() );

    smooth( view( src )
          , view( src )
          , gaussian()
          );

    write_view( "..\\out\\smooth_gaussian.png", view( dst ), png_tag() );
}
