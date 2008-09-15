// jpeg_test.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

#include <boost/test/unit_test.hpp>

#include <boost/gil/gil_all.hpp>
#include <boost/gil/extension/io_new/jpeg_io_old.hpp>

using namespace std;
using namespace boost::gil;

namespace jpeg_test {

BOOST_AUTO_TEST_CASE( jpeg_read_dimensions_test )
{
    std::string filename( "..\\test_images\\jpg\\found online\\test.jpg" );

    {
        point2< ptrdiff_t > dim = jpeg_read_dimensions( filename );

        BOOST_CHECK_EQUAL( dim.x, 136 );
        BOOST_CHECK_EQUAL( dim.y, 98  );
    }
}

BOOST_AUTO_TEST_CASE( jpeg_read_image_test )
{
    std::string filename( "..\\test_images\\jpg\\found online\\test.jpg" );

    {
        rgb8_image_t img;
        jpeg_read_image( filename, img );

        BOOST_CHECK_EQUAL( img.width() , 136 );
        BOOST_CHECK_EQUAL( img.height(), 98 );
    }
}

BOOST_AUTO_TEST_CASE( jpeg_read_and_convert_image_test )
{
    std::string filename( "..\\test_images\\jpg\\found online\\test.jpg" );

    {
        rgb8_image_t img;
        jpeg_read_and_convert_image( filename, img );

        BOOST_CHECK_EQUAL( img.width() , 136 );
        BOOST_CHECK_EQUAL( img.height(), 98 );
    }
}

BOOST_AUTO_TEST_CASE( jpeg_read_view_test )
{
    std::string filename( "..\\test_images\\jpg\\found online\\test.jpg" );

    {
        rgb8_image_t img( 136, 98 );
        jpeg_read_view( filename, view( img ) );
    }
}

BOOST_AUTO_TEST_CASE( jpeg_read_and_convert_view_test )
{
    std::string filename( "..\\test_images\\jpg\\found online\\test.jpg" );

    {
        rgb8_image_t img( 136, 98 );
        // see readme.txt
        //jpeg_read_and_convert_view( filename, view( img ) );
    }
}

BOOST_AUTO_TEST_CASE( jpeg_write_view_test )
{
    {
        string filename( "..\\test\\jpg\\test1.jpg" );

        gray8_image_t img( 136, 98 );
        jpeg_write_view( filename, view( img ) );
    }
}

} // namespace jpeg_test