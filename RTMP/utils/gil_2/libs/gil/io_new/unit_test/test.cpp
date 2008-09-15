// tiff_test.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

#include <boost/gil/extension/io_new/tiff_all.hpp>
#include <boost/gil/extension/io_new/png_all.hpp>
#include <boost/type_traits/is_same.hpp>

#define BOOST_TEST_MAIN
#include <boost/test/unit_test.hpp>

using namespace std;
using namespace boost::gil;

typedef tiff_tag tag_t;

namespace tiff_test {

BOOST_AUTO_TEST_CASE( read_image_info_test )
{
    std::string filename( "..\\test_images\\tiff\\libtiffpic\\depth\\flower-minisblack-06.tif" );

    {
        typedef bit_aligned_image1_type< 6, gray_layout_t >::type image_t;
        typedef image_t::view_t view_t;
        typedef view_t::x_iterator x_iterator;

        image_t img;

        read_image( filename
                  , img
                  , tag_t()
                  );

        gray8_image_t gray_img( view( img ).dimensions() );
        copy_pixels( view( img ), view( gray_img ));

        write_view( "single_test_2.tiff"
                  , view( gray_img )
                  , png_tag()
                  );
    }
}

} // namespace tiff_test