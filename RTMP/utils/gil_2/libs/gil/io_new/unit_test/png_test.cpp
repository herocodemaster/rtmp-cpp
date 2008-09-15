// png_test.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

#include <boost/gil/gil_all.hpp>
#include <boost/gil/extension/io_new/png_all.hpp>

#include <boost/test/unit_test.hpp>

#include <boost/type_traits/is_same.hpp>

using namespace std;
using namespace boost::gil;

typedef png_tag tag_t;

namespace png_test {

BOOST_AUTO_TEST_CASE( read_image_info_using_string )
{
    std::string filename( "..\\test_images\\png\\wikipedia\\test.png" );

    {
        image_read_info< png_tag > info = read_image_info( filename
                                                         , tag_t() );
        BOOST_CHECK_EQUAL( info._width , 320 );
        BOOST_CHECK_EQUAL( info._height, 240 );
    }

    {
        ifstream in( filename.c_str(), ios::in | ios::binary );

        image_read_info< tag_t > info = read_image_info( in
                                                       , tag_t() );

        BOOST_CHECK_EQUAL( info._width , 320 );
        BOOST_CHECK_EQUAL( info._height, 240 );
    }

    {
        FILE* file = fopen( filename.c_str(), "rb" );
        
        image_read_info< tag_t > info = read_image_info( file
                                                       , tag_t() );

        BOOST_CHECK_EQUAL( info._width , 320 );
        BOOST_CHECK_EQUAL( info._height, 240 );
    }
}

BOOST_AUTO_TEST_CASE( read_image_test )
{
    std::string filename( "..\\test_images\\png\\wikipedia\\test.png" );

    {
        rgba8_image_t img;
        read_image( filename, img, tag_t() );

        BOOST_CHECK_EQUAL( img.width() , 320 );
        BOOST_CHECK_EQUAL( img.height(), 240 );
    }

    {
        ifstream in( filename.c_str(), ios::in | ios::binary );

        rgba8_image_t img;
        read_image( in, img, tag_t() );

        BOOST_CHECK_EQUAL( img.width() , 320 );
        BOOST_CHECK_EQUAL( img.height(), 240 );
    }

    {
        FILE* file = fopen( filename.c_str(), "rb" );
        
        rgba8_image_t img;
        read_image( file, img, tag_t() );

        BOOST_CHECK_EQUAL( img.width() , 320 );
        BOOST_CHECK_EQUAL( img.height(), 240 );
    }
}

BOOST_AUTO_TEST_CASE( read_and_convert_image_test )
{
    std::string filename( "..\\test_images\\png\\wikipedia\\test.png" );

    {
        rgb8_image_t img;
        read_and_convert_image( filename, img, tag_t() );

        BOOST_CHECK_EQUAL( img.width() , 320 );
        BOOST_CHECK_EQUAL( img.height(), 240 );
    }

    {
        ifstream in( filename.c_str(), ios::in | ios::binary );

        rgb8_image_t img;
        read_and_convert_image( in, img, tag_t() );

        BOOST_CHECK_EQUAL( img.width() , 320 );
        BOOST_CHECK_EQUAL( img.height(), 240 );
    }

    {
        FILE* file = fopen( filename.c_str(), "rb" );
        
        rgb8_image_t img;
        read_and_convert_image( file, img, tag_t() );

        BOOST_CHECK_EQUAL( img.width() , 320 );
        BOOST_CHECK_EQUAL( img.height(), 240 );
    }
}

BOOST_AUTO_TEST_CASE( read_view_test )
{
    std::string filename( "..\\test_images\\png\\wikipedia\\test.png" );

    {
        rgba8_image_t img( 320, 240 );
        read_view( filename, view( img ), tag_t() );
    }

    {
        ifstream in( filename.c_str(), ios::in | ios::binary );

        rgba8_image_t img( 320, 240 );
        read_view( in, view( img ), tag_t() );
    }

    {
        FILE* file = fopen( filename.c_str(), "rb" );
        
        rgba8_image_t img( 320, 240 );
        read_view( file, view( img ), tag_t() );
    }
}

BOOST_AUTO_TEST_CASE( read_and_convert_view_test )
{
    std::string filename( "..\\test_images\\png\\wikipedia\\test.png" );

    {
        rgb8_image_t img( 320, 240 );

        ///@todo: weird compiler error
        // see readme.txt
        //read_and_convert_view( filename, view( img ), tag_t() );
    }

    {
        ifstream in( filename.c_str(), ios::in | ios::binary );

        rgb8_image_t img( 320, 240 );

        ///@todo: weird compiler error
        // see readme.txt
        //read_and_convert_view( in, view( img ), tag_t() );
    }

    {
        FILE* file = fopen( filename.c_str(), "rb" );
        
        rgb8_image_t img( 320, 240 );

        /*
        image_read_info< tag_t > info = read_and_convert_view( file
                                                             , view( img )
                                                             , tag_t()
                                                             );
        */
    }
}

BOOST_AUTO_TEST_CASE( write_view_test )
{
    {
        string filename( "..\\test\\png\\test1.png" );

        gray8_image_t img( 320, 240 );
        write_view( filename, view( img ), tag_t() );
    }

    {
        string filename( "..\\test\\png\\test2.png" );

        ofstream out( filename.c_str(), ios::out | ios::binary );

        rgb8_image_t img( 320, 240 );
        write_view( out, view( img ), tag_t() );
    }

    {
        string filename( "..\\test\\png\\test3.png" );

        FILE* file = fopen( filename.c_str(), "wb" );
        
        rgb8_image_t img( 320, 240 );
        write_view( file, view( img ), tag_t() );
    }
}

} // namespace png_test