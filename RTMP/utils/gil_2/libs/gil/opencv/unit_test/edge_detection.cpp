#include "stdafx.h"

#include <boost\test\unit_test.hpp>

#include <boost\gil\extension\opencv\edge_detection.hpp>

#include <boost\gil\extension\io_new\png_all.hpp>

using namespace boost::gil;
using namespace boost::gil::opencv;

BOOST_AUTO_TEST_CASE( test_sobel )
{
    gray8_image_t src;
    read_and_convert_image( "..\\in\\in.png", src, png_tag() ); 

    gray8_image_t dst( view( src ).dimensions() );

    sobel( view( src )
         , view( dst )
         , aperture3()
         );

    write_view( "..\\out\\sobel.png", view( dst ), png_tag() );
}

BOOST_AUTO_TEST_CASE( test_laplace )
{
    rgb8_image_t src;
    read_image( "..\\in\\in.png", src, png_tag() ); 

    rgb32f_image_t dst( view( src ).dimensions() );

    laplace( view( src )
           , view( dst )
           , aperture3()
           );

    write_view( "..\\out\\laplace.png"
              , color_converted_view< rgb8_pixel_t >( view( dst ))
              , png_tag()
              );
}

BOOST_AUTO_TEST_CASE( test_canny )
{
    gray8_image_t src;
    read_and_convert_image( "..\\in\\in.png", src, png_tag() ); 

    gray8_image_t edges( view( src ).dimensions() );

    canny( view( src   )
         , view( edges )
         , 60
         , 180
         , aperture3()
         );

    write_view( "..\\out\\canny.png", view( edges ), png_tag() );
}

BOOST_AUTO_TEST_CASE( test_pre_corner_detect )
{
    gray8_image_t src;
    read_and_convert_image( "..\\in\\in.png", src, png_tag() ); 

    gray32f_image_t corners( view( src ).dimensions() );

    precorner_detect( view( src     )
                    , view( corners )
                    , aperture3()
                    );

    write_view( "..\\out\\precorner_detect.png"
              , color_converted_view< rgb8_pixel_t >( view( corners ))
              , png_tag()
              );

    // try this for testing
    /*
    // assume that the image is floating-point
    IplImage* corners = cvCloneImage(image);
    IplImage* dilated_corners = cvCloneImage(image);
    IplImage* corner_mask = cvCreateImage( cvGetSize(image), 8, 1 );
    cvPreCornerDetect( image, corners, 3 );
    cvDilate( corners, dilated_corners, 0, 1 );
    cvSubS( corners, dilated_corners, corners );
    cvCmpS( corners, 0, corner_mask, CV_CMP_GE );
    cvReleaseImage( &corners );
    cvReleaseImage( &dilated_corners );
    */
}

BOOST_AUTO_TEST_CASE( test_corner_eigen_vals_and_vecs )
{
    gray8_image_t src;
    read_and_convert_image( "..\\in\\in.png", src, png_tag() ); 

    gray32f_image_t eigen( view( src ).dimensions().x * 6
                       , view( src ).dimensions().y
                       );

    corner_eigen_vals_and_vecs( view( src   )
                              , view( eigen )
                              , 3
                              , aperture3()
                              );

    write_view( "..\\out\\corner_eigen_vals_and_vecs.png"
              , color_converted_view< rgb8_pixel_t >( view( eigen ))
              , png_tag()
              );
}