// tiff_test.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

#include <boost/gil/extension/io_new/tiff_all.hpp>
#include <boost/type_traits/is_same.hpp>

#define BOOST_TEST_MAIN
#include <boost/test/unit_test.hpp>

using namespace std;
using namespace boost::gil;

typedef tiff_tag tag_t;

namespace tiff_test {

BOOST_AUTO_TEST_CASE( read_image_info_test )
{
    string filename( "..\\test_images\\tiff\\test.tif" );

    {
        image_read_info< tag_t > info = read_image_info( filename
                                                       , tag_t() );

        BOOST_CHECK_EQUAL( info._width , 200 );
        BOOST_CHECK_EQUAL( info._height, 133 );
    }

    {
        // An ifstream cannot be used to construct a device_type.
        // There is no way to get a TIFF* from a ifstream.

/*
        ifstream in( filename.c_str(), ios::in | ios::binary );
        image_read_info< tag_t > info = read_image_info( in
                                                       , tag_t() );
*/
    }

    {
        TIFF* file = TIFFOpen( filename.c_str(), "r" );

        image_read_info< tag_t > info = read_image_info( file
                                                       , tag_t() );
        
        BOOST_CHECK_EQUAL( info._width , 200 );
        BOOST_CHECK_EQUAL( info._height, 133 );
    }
}

BOOST_AUTO_TEST_CASE( read_image_test )
{
    string filename( "..\\test_images\\tiff\\test.tif" );

    {
        rgb8_image_t img;
        read_image( filename, img, tag_t() );

        BOOST_CHECK_EQUAL( img.width() , 200 );
        BOOST_CHECK_EQUAL( img.height(), 133 );
    }

    {
        TIFF* file = TIFFOpen( filename.c_str(), "r" );

        rgb8_image_t img;
        read_image( file, img, tag_t() );

        BOOST_CHECK_EQUAL( img.width() , 200 );
        BOOST_CHECK_EQUAL( img.height(), 133 );
    }
}

BOOST_AUTO_TEST_CASE( read_and_convert_image_test )
{
    string filename( "..\\test_images\\tiff\\test.tif" );

    {
        rgb8_image_t img;
        read_and_convert_image( filename, img, tag_t() );

        BOOST_CHECK_EQUAL( img.width() , 200 );
        BOOST_CHECK_EQUAL( img.height(), 133 );
    }

    {
        TIFF* file = TIFFOpen( filename.c_str(), "r" );

        rgb8_image_t img;
        read_and_convert_image( file, img, tag_t() );

        BOOST_CHECK_EQUAL( img.width() , 200 );
        BOOST_CHECK_EQUAL( img.height(), 133 );
    }
}

BOOST_AUTO_TEST_CASE( read_view_test )
{
    string filename( "..\\test_images\\tiff\\test.tif" );

    {
        rgb8_image_t img( 200, 133 );
        read_view( filename, view( img ), tag_t() );
    }

    {
        TIFF* file = TIFFOpen( filename.c_str(), "r" );

        rgb8_image_t img( 200, 133 );
        read_view( file, view( img ), tag_t() );
    }
}

BOOST_AUTO_TEST_CASE( read_and_convert_view_test )
{
    string filename( "..\\test_images\\tiff\\test.tif" );

    {
        rgb8_image_t img( 200, 133 );
        // see readme.txt
        //read_and_convert_view( filename, view( img ), tag_t() );
    }

    {
        TIFF* file = TIFFOpen( filename.c_str(), "r" );

        rgb8_image_t img( 200, 133 );
        // see readme.txt
        //read_and_convert_view( file, view( img ), tag_t() );
    }
}

BOOST_AUTO_TEST_CASE( write_view_test )
{

    {
        string filename( "..\\test\\tiff\\test1.tif" );

        gray8_image_t img( 320, 240 );
        write_view( filename, view( img ), tiff_tag() );
    }

    {
        // An ofstream cannot be used to construct a device_type.
        // There is no way to get a TIFF* from a ofstream.
    }

    {
        string filename( "..\\test\\tiff\\test2.tif" );

        TIFF* file = TIFFOpen( filename.c_str(), "w" );
        
        rgb8_image_t img( 320, 240 );
        write_view( file, view( img ), tag_t() );
    }
}

} // namespace tiff_test