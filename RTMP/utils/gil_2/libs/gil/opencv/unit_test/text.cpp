#include "stdafx.h"

#include <boost\test\unit_test.hpp>

#include <boost\gil\extension\opencv\text.hpp>

#include <boost\gil\extension\io_new\png_all.hpp>

using namespace boost::gil;
using namespace boost::gil::opencv;

BOOST_AUTO_TEST_CASE( test_text )
{
    rgb8_image_t src;
    read_image( "..\\in\\in.png", src, png_tag() ); 

    ipl_font_wrapper font = create_ipl_font( font_hershey_plain()
                                           , 1
                                           , 1
                                           , four_connected_line()
                                           );

    putText( view( src )
           , std::string( "Hello World!" )
           , boost::gil::opencv::point_t( 10,100 )
           , font
           , rgb8_pixel_t( 0, 100, 88 )
           );


    boost::gil::opencv::point_t size;
    int                         baseline;
    getTextSize( std::string( "Hello World!" )
               , font
               , size
               , baseline
               );

    BOOST_CHECK_EQUAL( size.x  , 96 );
    BOOST_CHECK_EQUAL( size.y  , 10 );
    BOOST_CHECK_EQUAL( baseline,  5 );

    write_view( "..\\out\\text.png", view( src ), png_tag() );
}

