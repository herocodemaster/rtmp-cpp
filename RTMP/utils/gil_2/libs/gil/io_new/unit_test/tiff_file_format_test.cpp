// tiff_test.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

#include <boost/gil/extension/io_new/tiff_read.hpp>
#include <boost/gil/extension/io_new/png_write.hpp>

#include <boost/test/unit_test.hpp>

using namespace std;
using namespace boost::gil;

typedef tiff_tag tag_t;

namespace tiff_test {

// 73x43 2-bit minisblack gray image
BOOST_AUTO_TEST_CASE( two_bit_minisblack_gray_image_test )
{
    std::string filename( "..\\test_images\\tiff\\libtiffpic\\depth\\flower-minisblack-02.tif" );

    {
        typedef bit_aligned_image1_type< 2, gray_layout_t >::type image_t;
        image_t img;

        read_image( filename
                  , img
                  , tag_t()
                  );

        write_view( "..\\test\\png\\test4.png"
                  , color_converted_view< gray8_pixel_t >( view( img ))
                  , png_tag()
                  );
    }
}

// flower-minisblack-04.tif	73x43 4-bit minisblack gray image
BOOST_AUTO_TEST_CASE( four_bit_minisblack_gray_image_test )
{
    std::string filename( "..\\test_images\\tiff\\libtiffpic\\depth\\flower-minisblack-04.tif" );

    {
        typedef bit_aligned_image1_type< 4, gray_layout_t >::type image_t;
        image_t img;

        read_image( filename
                  , img
                  , tag_t()
                  );

        write_view( "..\\test\\png\\test5.png"
                  , color_converted_view< gray8_pixel_t >( view( img ))
                  , png_tag()
                  );
    }
}

// flower-minisblack-06.tif 73x43 6-bit minisblack gray image
BOOST_AUTO_TEST_CASE( six_bit_minisblack_gray_image_test )
{
    std::string filename( "..\\test_images\\tiff\\libtiffpic\\depth\\flower-minisblack-06.tif" );

    {
        typedef bit_aligned_image1_type< 6, gray_layout_t >::type image_t;
        image_t img;

        read_image( filename
                  , img
                  , tag_t()
                  );

        write_view( "..\\test\\png\\test6.png"
                  , color_converted_view< gray8_pixel_t >( view( img ))
                  , png_tag()
                  );
    }
}

// flower-minisblack-08.tif	73x43 8-bit minisblack gray image
BOOST_AUTO_TEST_CASE( eight_bit_minisblack_gray_image_test )
{
    std::string filename( "..\\test_images\\tiff\\libtiffpic\\depth\\flower-minisblack-08.tif" );

    {
        typedef gray8_image_t image_t;
        image_t img;

        read_image( filename
                  , img
                  , tag_t()
                  );

        write_view( "..\\test\\png\\test7.png"
                  , color_converted_view< gray8_pixel_t >( view( img ))
                  , png_tag()
                  );
    }
}

// flower-minisblack-10.tif	73x43 10-bit minisblack gray image
BOOST_AUTO_TEST_CASE( ten_bit_minisblack_gray_image_test )
{
    std::string filename( "..\\test_images\\tiff\\libtiffpic\\depth\\flower-minisblack-10.tif" );

    {
        typedef bit_aligned_image1_type< 10, gray_layout_t >::type image_t;
        image_t img;

        read_image( filename
                  , img
                  , tag_t()
                  );

        write_view( "..\\test\\png\\test8.png"
                  , color_converted_view< gray8_pixel_t >( view( img ))
                  , png_tag()
                  );
    }
}

// flower-minisblack-12.tif	73x43 12-bit minisblack gray image
BOOST_AUTO_TEST_CASE( twelve_bit_minisblack_gray_image_test )
{
    std::string filename( "..\\test_images\\tiff\\libtiffpic\\depth\\flower-minisblack-12.tif" );

    {
        typedef bit_aligned_image1_type< 12, gray_layout_t >::type image_t;
        image_t img;

        read_image( filename
                  , img
                  , tag_t()
                  );

        write_view( "..\\test\\png\\test9.png"
                  , color_converted_view< gray8_pixel_t >( view( img ))
                  , png_tag()
                  );
    }
}

// flower-minisblack-14.tif	73x43 14-bit minisblack gray image
BOOST_AUTO_TEST_CASE( fourteen_bit_minisblack_gray_image_test )
{
    std::string filename( "..\\test_images\\tiff\\libtiffpic\\depth\\flower-minisblack-14.tif" );

    {
        typedef bit_aligned_image1_type< 14, gray_layout_t >::type image_t;
        image_t img;

        read_image( filename
                  , img
                  , tag_t()
                  );

        write_view( "..\\test\\png\\test10.png"
                  , color_converted_view< gray8_pixel_t >( view( img ))
                  , png_tag()
                  );
    }
}

// flower-minisblack-16.tif	73x43 16-bit minisblack gray image
BOOST_AUTO_TEST_CASE( sixteen_bit_minisblack_gray_image_test )
{
    std::string filename( "..\\test_images\\tiff\\libtiffpic\\depth\\flower-minisblack-16.tif" );

    {
        typedef gray16_image_t image_t;
        image_t img;

        read_image( filename
                  , img
                  , tag_t()
                  );

        write_view( "..\\test\\png\\test11.png"
                  , color_converted_view< gray8_pixel_t >( view( img ))
                  , png_tag()
                  );
    }
}

// flower-minisblack-24.tif	73x43 24-bit minisblack gray image
BOOST_AUTO_TEST_CASE( twentyfour_bit_minisblack_gray_image_test )
{
    std::string filename( "..\\test_images\\tiff\\libtiffpic\\depth\\flower-minisblack-24.tif" );

    {
        typedef bit_aligned_image1_type< 24, gray_layout_t >::type image_t;
        image_t img;

        read_image( filename
                  , img
                  , tag_t()
                  );

        write_view( "..\\test\\png\\test12.png"
                  , color_converted_view< gray8_pixel_t >( view( img ))
                  , png_tag()
                  );
    }
}

// flower-minisblack-32.tif	73x43 32-bit minisblack gray image
BOOST_AUTO_TEST_CASE( thirtytwo_bit_minisblack_gray_image_test )
{
    std::string filename( "..\\test_images\\tiff\\libtiffpic\\depth\\flower-minisblack-32.tif" );

    {
        typedef gray32_image_t image_t;
        image_t img;

        read_image( filename
                  , img
                  , tag_t()
                  );

        write_view( "..\\test\\png\\test13.png"
                  , color_converted_view< gray8_pixel_t >( view( img ))
                  , png_tag()
                  );
    }
}

// flower-palette-02.tif 73x43 4-entry colormapped image
BOOST_AUTO_TEST_CASE( four_entry_colormapped_image_test )
{
    std::string filename( "..\\test_images\\tiff\\libtiffpic\\depth\\flower-palette-02.tif" );

    {
        typedef rgb16_image_t image_t;
        image_t img;

        read_image( filename
                  , img
                  , tag_t()
                  );

        write_view( "..\\test\\png\\test13.png"
                  , color_converted_view< rgb8_pixel_t >( view( img ))
                  , png_tag()
                  );
    }
}

// flower-palette-04.tif	73x43 16-entry colormapped image
BOOST_AUTO_TEST_CASE( sixteen_entry_colormapped_image_test )
{
    std::string filename( "..\\test_images\\tiff\\libtiffpic\\depth\\flower-palette-04.tif" );

    {
        typedef rgb16_image_t image_t;
        image_t img;

        read_image( filename
                  , img
                  , tag_t()
                  );

        write_view( "..\\test\\png\\test14.png"
                  , color_converted_view< rgb8_pixel_t >( view( img ))
                  , png_tag()
                  );
    }
}

// flower-palette-08.tif	73x43 256-entry colormapped image
BOOST_AUTO_TEST_CASE( twohundred_twenty_five_entry_colormapped_image_test )
{
    std::string filename( "..\\test_images\\tiff\\libtiffpic\\depth\\flower-palette-08.tif" );

    {
        typedef rgb16_image_t image_t;
        image_t img;

        read_image( filename
                  , img
                  , tag_t()
                  );

        write_view( "..\\test\\png\\test15.png"
                  , color_converted_view< rgb8_pixel_t >( view( img ))
                  , png_tag()
                  );
    }
}

// flower-palette-16.tif	73x43 65536-entry colormapped image
BOOST_AUTO_TEST_CASE( sixtyfive_thousand_entry_colormapped_image_test )
{
    std::string filename( "..\\test_images\\tiff\\libtiffpic\\depth\\flower-palette-16.tif" );

    {
        typedef rgb16_image_t image_t;
        image_t img;

        read_image( filename
                  , img
                  , tag_t()
                  );

        write_view( "..\\test\\png\\test16.png"
                  , color_converted_view< rgb8_pixel_t >( view( img ))
                  , png_tag()
                  );
    }
}

// flower-rgb-contig-02.tif	73x43 2-bit contiguous RGB image
BOOST_AUTO_TEST_CASE( two_bit_contiguous_RGB_image_test )
{
    std::string filename( "..\\test_images\\tiff\\libtiffpic\\depth\\flower-rgb-contig-02.tif" );

    {
        typedef bit_aligned_image3_type< 2, 2, 2, rgb_layout_t >::type image_t;
        image_t img;

        read_image( filename
                  , img
                  , tag_t()
                  );

        write_view( "..\\test\\png\\test17.png"
                  , color_converted_view< rgb8_pixel_t >( view( img ))
                  , png_tag()
                  );
    }
}

// flower-rgb-contig-04.tif	73x43 4-bit contiguous RGB image
BOOST_AUTO_TEST_CASE( four_bit_contiguous_RGB_image_test )
{
    std::string filename( "..\\test_images\\tiff\\libtiffpic\\depth\\flower-rgb-contig-04.tif" );

    {
        typedef bit_aligned_image3_type< 4, 4, 4, rgb_layout_t >::type image_t;
        image_t img;

        read_image( filename
                  , img
                  , tag_t()
                  );

        write_view( "..\\test\\png\\test18.png"
                  , color_converted_view< rgb8_pixel_t >( view( img ))
                  , png_tag()
                  );
    }
}

// flower-rgb-contig-08.tif	73x43 8-bit contiguous RGB image
BOOST_AUTO_TEST_CASE( eight_bit_contiguous_RGB_image_test )
{
    std::string filename( "..\\test_images\\tiff\\libtiffpic\\depth\\flower-rgb-contig-08.tif" );

    {
        typedef rgb8_image_t image_t;
        image_t img;

        read_image( filename
                  , img
                  , tag_t()
                  );

        write_view( "..\\test\\png\\test19.png"
                  , color_converted_view< rgb8_pixel_t >( view( img ))
                  , png_tag()
                  );
    }
}

// flower-rgb-contig-10.tif	73x43 10-bit contiguous RGB image
BOOST_AUTO_TEST_CASE( ten_bit_contiguous_RGB_image_test )
{
    std::string filename( "..\\test_images\\tiff\\libtiffpic\\depth\\flower-rgb-contig-10.tif" );

    {
        typedef bit_aligned_image3_type< 10, 10, 10, rgb_layout_t >::type image_t;
        image_t img;

        read_image( filename
                  , img
                  , tag_t()
                  );

        write_view( "..\\test\\png\\test20.png"
                  , color_converted_view< rgb8_pixel_t >( view( img ))
                  , png_tag()
                  );
    }
}

// flower-rgb-contig-12.tif	73x43 12-bit contiguous RGB image
BOOST_AUTO_TEST_CASE( twelve_bit_contiguous_RGB_image_test )
{
    std::string filename( "..\\test_images\\tiff\\libtiffpic\\depth\\flower-rgb-contig-12.tif" );

    {
        typedef bit_aligned_image3_type< 12, 12, 12, rgb_layout_t >::type image_t;
        image_t img;

        read_image( filename
                  , img
                  , tag_t()
                  );

        write_view( "..\\test\\png\\test21.png"
                  , color_converted_view< rgb8_pixel_t >( view( img ))
                  , png_tag()
                  );
    }
}

// flower-rgb-contig-14.tif	73x43 14-bit contiguous RGB image
BOOST_AUTO_TEST_CASE( fourteen_bit_contiguous_RGB_image_test )
{
    std::string filename( "..\\test_images\\tiff\\libtiffpic\\depth\\flower-rgb-contig-14.tif" );

    {
        typedef bit_aligned_image3_type< 14, 14, 14, rgb_layout_t >::type image_t;
        image_t img;

        read_image( filename
                  , img
                  , tag_t()
                  );

        write_view( "..\\test\\png\\test22.png"
                  , color_converted_view< rgb8_pixel_t >( view( img ))
                  , png_tag()
                  );
    }
}

// flower-rgb-contig-16.tif	73x43 16-bit contiguous RGB image
BOOST_AUTO_TEST_CASE( sixteen_bit_contiguous_RGB_image_test )
{
    std::string filename( "..\\test_images\\tiff\\libtiffpic\\depth\\flower-rgb-contig-16.tif" );

    {
        typedef rgb16_image_t image_t;
        image_t img;

        read_image( filename
                  , img
                  , tag_t()
                  );

        write_view( "..\\test\\png\\test23.png"
                  , color_converted_view< rgb8_pixel_t >( view( img ))
                  , png_tag()
                  );
    }
}

// flower-rgb-contig-24.tif	73x43 24-bit contiguous RGB image
BOOST_AUTO_TEST_CASE( twenty_four_bit_contiguous_RGB_image_test )
{
    std::string filename( "..\\test_images\\tiff\\libtiffpic\\depth\\flower-rgb-contig-24.tif" );

    {
        typedef bit_aligned_image3_type< 24, 24, 24, rgb_layout_t >::type image_t;
        image_t img;

        read_image( filename
                  , img
                  , tag_t()
                  );

        write_view( "..\\test\\png\\test24.png"
                  , color_converted_view< rgb8_pixel_t >( view( img ))
                  , png_tag()
                  );
    }
}

// flower-rgb-contig-32.tif	73x43 32-bit contiguous RGB image
BOOST_AUTO_TEST_CASE( thirty_two_bit_contiguous_RGB_image_test )
{
    std::string filename( "..\\test_images\\tiff\\libtiffpic\\depth\\flower-rgb-contig-32.tif" );

    {
        typedef rgb32_image_t image_t;
        image_t img;

        read_image( filename
                  , img
                  , tag_t()
                  );

        write_view( "..\\test\\png\\test25.png"
                  , color_converted_view< rgb8_pixel_t >( view( img ))
                  , png_tag()
                  );
    }
}

// flower-rgb-planar-02.tif	73x43 2-bit seperated RGB image
BOOST_AUTO_TEST_CASE( two_bit_seperated_RGB_image_test )
{
    std::string filename( "..\\test_images\\tiff\\libtiffpic\\depth\\flower-rgb-planar-02.tif" );

    {
/*
        typedef bit_aligned_image3_type< 2, 2, 2, rgb_layout_t >::type image_t;
        image_t img;

        read_image( filename
                  , img
                  , tag_t()
                  );

        write_view( "..\\test\\png\\test26.png"
                  , color_converted_view< rgb8_pixel_t >( view( img ))
                  , png_tag()
                  );
*/
    }
}

// flower-rgb-planar-04.tif	73x43 4-bit seperated RGB image
BOOST_AUTO_TEST_CASE( four_bit_seperated_RGB_image_test )
{
    std::string filename( "..\\test_images\\tiff\\libtiffpic\\depth\\flower-rgb-planar-04.tif" );

    {
/*
        typedef bit_aligned_image3_type< 4, 4, 4, rgb_layout_t >::type image_t;
        image_t img;

        read_image( filename
                  , img
                  , tag_t()
                  );

        write_view( "..\\test\\png\\test27.png"
                  , color_converted_view< rgb8_pixel_t >( view( img ))
                  , png_tag()
                  );
*/
    }
}

// flower-rgb-planar-08.tif	73x43 8-bit seperated RGB image
BOOST_AUTO_TEST_CASE( eight_bit_seperated_RGB_image_test )
{
    std::string filename( "..\\test_images\\tiff\\libtiffpic\\depth\\flower-rgb-planar-08.tif" );

    {
        typedef rgb8_planar_image_t image_t;
        image_t img;

        read_image( filename
                  , img
                  , tag_t()
                  );

        write_view( "..\\test\\png\\test28.png"
                  , color_converted_view< rgb8_pixel_t >( view( img ))
                  , png_tag()
                  );
    }
}

// flower-rgb-planar-10.tif	73x43 10-bit seperated RGB image
BOOST_AUTO_TEST_CASE( ten_bit_seperated_RGB_image_test )
{
    std::string filename( "..\\test_images\\tiff\\libtiffpic\\depth\\flower-rgb-planar-10.tif" );

    {
/*
        typedef bit_aligned_image3_type< 10, 10, 10, rgb_layout_t >::type image_t;
        image_t img;

        read_image( filename
                  , img
                  , tag_t()
                  );

        write_view( "..\\test\\png\\test29.png"
                  , color_converted_view< rgb8_pixel_t >( view( img ))
                  , png_tag()
                  );
*/
    }
}

// flower-rgb-planar-12.tif	73x43 12-bit seperated RGB image
BOOST_AUTO_TEST_CASE( twelve_bit_seperated_RGB_image_test )
{
    std::string filename( "..\\test_images\\tiff\\libtiffpic\\depth\\flower-rgb-planar-12.tif" );

    {
/*
        typedef bit_aligned_image3_type< 12, 12, 12, rgb_layout_t >::type image_t;
        image_t img;

        read_image( filename
                  , img
                  , tag_t()
                  );

        write_view( "..\\test\\png\\test30.png"
                  , color_converted_view< rgb8_pixel_t >( view( img ))
                  , png_tag()
                  );
*/
    }
}

// flower-rgb-planar-14.tif	73x43 14-bit seperated RGB image
BOOST_AUTO_TEST_CASE( fourteen_bit_seperated_RGB_image_test )
{
    std::string filename( "..\\test_images\\tiff\\libtiffpic\\depth\\flower-rgb-planar-14.tif" );

    {
/*
        typedef bit_aligned_image3_type< 14, 14, 14, rgb_layout_t >::type image_t;
        image_t img;

        read_image( filename
                  , img
                  , tag_t()
                  );

        write_view( "..\\test\\png\\test31.png"
                  , color_converted_view< rgb8_pixel_t >( view( img ))
                  , png_tag()
                  );
*/
    }
}

// flower-rgb-planar-16.tif	73x43 16-bit seperated RGB image
BOOST_AUTO_TEST_CASE( sixteen_bit_seperated_RGB_image_test )
{
    std::string filename( "..\\test_images\\tiff\\libtiffpic\\depth\\flower-rgb-planar-16.tif" );

    {
        typedef rgb16_planar_image_t image_t;
        image_t img;

        read_image( filename
                  , img
                  , tag_t()
                  );

        write_view( "..\\test\\png\\test32.png"
                  , color_converted_view< rgb8_pixel_t >( view( img ))
                  , png_tag()
                  );
    }
}

// flower-rgb-planar-24.tif	73x43 24-bit seperated RGB image
BOOST_AUTO_TEST_CASE( twenty_four_bit_seperated_RGB_image_test )
{
    std::string filename( "..\\test_images\\tiff\\libtiffpic\\depth\\flower-rgb-planar-24.tif" );

    {
/*
        typedef bit_aligned_image3_type< 24, 24, 24, rgb_layout_t >::type image_t;
        image_t img;

        read_image( filename
                  , img
                  , tag_t()
                  );

        write_view( "..\\test\\png\\test33.png"
                  , color_converted_view< rgb8_pixel_t >( view( img ))
                  , png_tag()
                  );
*/
    }
}

// flower-rgb-planar-32.tif	73x43 32-bit seperated RGB image
BOOST_AUTO_TEST_CASE( thirty_two_bit_seperated_RGB_image_test )
{
    std::string filename( "..\\test_images\\tiff\\libtiffpic\\depth\\flower-rgb-planar-32.tif" );

    {
        typedef rgb32_planar_image_t image_t;
        image_t img;

        read_image( filename
                  , img
                  , tag_t()
                  );

        write_view( "..\\test\\png\\test34.png"
                  , color_converted_view< rgb8_pixel_t >( view( img ))
                  , png_tag()
                  );
    }
}

// flower-separated-contig-08.tif	73x43 8-bit contiguous CMYK image
BOOST_AUTO_TEST_CASE( eight_bit_contiguous_CMYK_image_test )
{
    std::string filename( "..\\test_images\\tiff\\libtiffpic\\depth\\flower-separated-contig-08.tif" );

    {
        typedef cmyk8_image_t image_t;
        image_t img;

        read_image( filename
                  , img
                  , tag_t()
                  );

        write_view( "..\\test\\png\\test35.png"
                  , color_converted_view< rgb8_pixel_t >( view( img ))
                  , png_tag()
                  );
    }
}

// flower-separated-contig-16.tif	73x43 16-bit contiguous CMYK image
BOOST_AUTO_TEST_CASE( sixteen_bit_contiguous_CMYK_image_test )
{
    std::string filename( "..\\test_images\\tiff\\libtiffpic\\depth\\flower-separated-contig-16.tif" );

    {
        typedef rgba16_image_t image_t;
        image_t img;

        read_image( filename
                  , img
                  , tag_t()
                  );

        write_view( "..\\test\\png\\test36.png"
                  , color_converted_view< rgb8_pixel_t >( view( img ))
                  , png_tag()
                  );
    }
}

// flower-separated-planar-08.tif	73x43 8-bit separated CMYK image
BOOST_AUTO_TEST_CASE( eight_bit_separated_CMYK_image_test )
{
    std::string filename( "..\\test_images\\tiff\\libtiffpic\\depth\\flower-separated-planar-08.tif" );

    {
        typedef rgba8_planar_image_t image_t;
        image_t img;

        read_image( filename
                  , img
                  , tag_t()
                  );

        write_view( "..\\test\\png\\test37.png"
                  , color_converted_view< rgb8_pixel_t >( view( img ))
                  , png_tag()
                  );
    }
}

// flower-separated-planar-16.tif	73x43 16-bit separated CMYK image
BOOST_AUTO_TEST_CASE( sixteen_bit_separated_CMYK_image_test )
{
    std::string filename( "..\\test_images\\tiff\\libtiffpic\\depth\\flower-separated-planar-16.tif" );

    {
        typedef rgba16_planar_image_t image_t;
        image_t img;

        read_image( filename
                  , img
                  , tag_t()
                  );

        write_view( "..\\test\\png\\test38.png"
                  , color_converted_view< rgb8_pixel_t >( view( img ))
                  , png_tag()
                  );
    }
}

} // namespace tiff_test
