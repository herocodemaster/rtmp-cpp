#include "stdafx.h"

#include <boost\test\unit_test.hpp>

#include <boost\gil\extension\opencv\ipl_image_wrapper.hpp>

using namespace boost::gil;
using namespace boost::gil::opencv;

BOOST_AUTO_TEST_CASE( test_ipl_image )
{
    rgb8_image_t img( 640, 480 );

    ipl_image_wrapper ipl_img = create_ipl_image( view( img ));
}
