/*
    Copyright 2007-2008 Christian Henning
    Use, modification and distribution are subject to the Boost Software License,
    Version 1.0. (See accompanying file LICENSE_1_0.txt or copy at
    http://www.boost.org/LICENSE_1_0.txt).
*/

/*************************************************************************************************/

#ifndef BOOST_GIL_EXTENSION_IO_TIFF_SUPPORTED_READ_TYPES_HPP_INCLUDED
#define BOOST_GIL_EXTENSION_IO_TIFF_SUPPORTED_READ_TYPES_HPP_INCLUDED

////////////////////////////////////////////////////////////////////////////////////////
/// \file               
/// \brief 
/// \author Christian Henning \n
///         
/// \date   2007-2008 \n
///
////////////////////////////////////////////////////////////////////////////////////////

#include <boost/gil/gil_all.hpp>

namespace boost { namespace gil { namespace detail {

struct not_supported { typedef void view_t; };

template< int SamplesPerPixel
        , int BitsPerSample
        , int SampleFormat
        , bool IsPlanar
        >
struct tiff_format
{
    typedef not_supported type;
};


// no planar for bit aligned
// only uint for bit aligned
template<> struct tiff_format< 1,  1, 1, false > { typedef bit_aligned_image1_type<  1, gray_layout_t >::type type; };
template<> struct tiff_format< 1,  2, 1, false > { typedef bit_aligned_image1_type<  2, gray_layout_t >::type type; };
template<> struct tiff_format< 1,  4, 1, false > { typedef bit_aligned_image1_type<  4, gray_layout_t >::type type; };
template<> struct tiff_format< 1,  6, 1, false > { typedef bit_aligned_image1_type<  6, gray_layout_t >::type type; };
template<> struct tiff_format< 1, 10, 1, false > { typedef bit_aligned_image1_type< 10, gray_layout_t >::type type; };
template<> struct tiff_format< 1, 12, 1, false > { typedef bit_aligned_image1_type< 12, gray_layout_t >::type type; };
template<> struct tiff_format< 1, 14, 1, false > { typedef bit_aligned_image1_type< 14, gray_layout_t >::type type; };
template<> struct tiff_format< 1, 24, 1, false > { typedef bit_aligned_image1_type< 24, gray_layout_t >::type type; };

#ifdef ENABLE_GRAY_ALPHA
template<> struct tiff_format< 2,  1, 1, false > { typedef bit_aligned_image2_type<  1,  1, gray_alpha_layout_t >::type type; };
template<> struct tiff_format< 2,  2, 1, false > { typedef bit_aligned_image2_type<  2,  2, gray_alpha_layout_t >::type type; };
template<> struct tiff_format< 2,  4, 1, false > { typedef bit_aligned_image2_type<  4,  4, gray_alpha_layout_t >::type type; };
template<> struct tiff_format< 2,  6, 1, false > { typedef bit_aligned_image2_type<  6,  6, gray_alpha_layout_t >::type type; };
template<> struct tiff_format< 2, 10, 1, false > { typedef bit_aligned_image2_type< 10, 10, gray_alpha_layout_t >::type type; };
template<> struct tiff_format< 2, 12, 1, false > { typedef bit_aligned_image2_type< 12, 12, gray_alpha_layout_t >::type type; };
template<> struct tiff_format< 2, 14, 1, false > { typedef bit_aligned_image2_type< 14, 14, gray_alpha_layout_t >::type type; };
template<> struct tiff_format< 2, 24, 1, false > { typedef bit_aligned_image2_type< 24, 24, gray_alpha_layout_t >::type type; };
#endif // ENABLE_GRAY_ALPHA

template<> struct tiff_format< 3,  1, 1, false > { typedef bit_aligned_image3_type<  1,  1,  1, rgb_layout_t >::type type; };
template<> struct tiff_format< 3,  2, 1, false > { typedef bit_aligned_image3_type<  2,  2,  2, rgb_layout_t >::type type; };
template<> struct tiff_format< 3,  4, 1, false > { typedef bit_aligned_image3_type<  4,  4,  4, rgb_layout_t >::type type; };
template<> struct tiff_format< 3,  6, 1, false > { typedef bit_aligned_image3_type<  6,  6,  6, rgb_layout_t >::type type; };
template<> struct tiff_format< 3, 10, 1, false > { typedef bit_aligned_image3_type< 10, 10, 10, rgb_layout_t >::type type; };
template<> struct tiff_format< 3, 12, 1, false > { typedef bit_aligned_image3_type< 12, 12, 12, rgb_layout_t >::type type; };
template<> struct tiff_format< 3, 14, 1, false > { typedef bit_aligned_image3_type< 14, 14, 14, rgb_layout_t >::type type; };
template<> struct tiff_format< 3, 24, 1, false > { typedef bit_aligned_image3_type< 24, 24, 24, rgb_layout_t >::type type; };

template<> struct tiff_format< 4,  1, 1, false > { typedef bit_aligned_image4_type<  1,  1,  1,  1, rgba_layout_t >::type type; };
template<> struct tiff_format< 4,  2, 1, false > { typedef bit_aligned_image4_type<  2,  2,  2,  2, rgba_layout_t >::type type; };
template<> struct tiff_format< 4,  4, 1, false > { typedef bit_aligned_image4_type<  4,  4,  4,  4, rgba_layout_t >::type type; };
template<> struct tiff_format< 4,  6, 1, false > { typedef bit_aligned_image4_type<  6,  6,  6,  6, rgba_layout_t >::type type; };
template<> struct tiff_format< 4, 10, 1, false > { typedef bit_aligned_image4_type< 10, 10, 10, 10, rgba_layout_t >::type type; };
template<> struct tiff_format< 4, 12, 1, false > { typedef bit_aligned_image4_type< 12, 12, 12, 12, rgba_layout_t >::type type; };
template<> struct tiff_format< 4, 14, 1, false > { typedef bit_aligned_image4_type< 14, 14, 14, 14, rgba_layout_t >::type type; };
template<> struct tiff_format< 4, 24, 1, false > { typedef bit_aligned_image4_type< 24, 24, 24, 24, rgba_layout_t >::type type; };

template<> struct tiff_format< 1,  8, 1, false > { typedef gray8_image_t type; };
template<> struct tiff_format< 1, 16, 1, false > { typedef gray16_image_t type; };
template<> struct tiff_format< 1, 32, 1, false > { typedef gray32_image_t type; };
template<> struct tiff_format< 1,  8, 2, false > { typedef gray8s_image_t type; };
template<> struct tiff_format< 1, 16, 2, false > { typedef gray16s_image_t type; };
template<> struct tiff_format< 1, 32, 2, false > { typedef gray32s_image_t type; };
template<> struct tiff_format< 1, 32, 3, false > { typedef gray32f_image_t type; };
template<> struct tiff_format< 1, 64, 3, false > { typedef gray64f_image_t type; };

#ifdef ENABLE_GRAY_ALPHA
template<> struct tiff_format< 2,  8, 1, false > { typedef gray_alpha8_image_t type; };
template<> struct tiff_format< 2, 16, 1, false > { typedef gray_alpha16_image_t type; };
template<> struct tiff_format< 2, 32, 1, false > { typedef gray_alpha32_image_t type; };
template<> struct tiff_format< 2,  8, 2, false > { typedef gray_alpha8s_image_t type; };
template<> struct tiff_format< 2, 16, 2, false > { typedef gray_alpha16s_image_t type; };
template<> struct tiff_format< 2, 32, 2, false > { typedef gray_alpha32s_image_t type; };
template<> struct tiff_format< 2, 32, 3, false > { typedef gray_alpha32f_image_t type; };
template<> struct tiff_format< 2, 64, 3, false > { typedef gray_alpha64f_image_t type; };

template<> struct tiff_format< 2,  8, 1, true > { typedef gray_alpha8_planar_image_t type; };
template<> struct tiff_format< 2, 16, 1, true > { typedef gray_alpha16_planar_image_t type; };
template<> struct tiff_format< 2, 32, 1, true > { typedef gray_alpha32_planar_image_t type; };
template<> struct tiff_format< 2,  8, 2, true > { typedef gray_alpha8s_planar_image_t type; };
template<> struct tiff_format< 2, 16, 2, true > { typedef gray_alpha16s_planar_image_t type; };
template<> struct tiff_format< 2, 32, 2, true > { typedef gray_alpha32s_planar_image_t type; };
template<> struct tiff_format< 2, 32, 3, true > { typedef gray_alpha32f_planar_image_t type; };
template<> struct tiff_format< 2, 64, 3, true > { typedef gray_alpha64f_planar_image_t type; };
#endif // ENABLE_GRAY_ALPHA

template<> struct tiff_format< 3,  8, 1, false > { typedef rgb8_image_t type; };
template<> struct tiff_format< 3, 16, 1, false > { typedef rgb16_image_t type; };
template<> struct tiff_format< 3, 32, 1, false > { typedef rgb32_image_t type; };
template<> struct tiff_format< 3,  8, 2, false > { typedef rgb8s_image_t type; };
template<> struct tiff_format< 3, 16, 2, false > { typedef rgb16s_image_t type; };
template<> struct tiff_format< 3, 32, 2, false > { typedef rgb32s_image_t type; };
template<> struct tiff_format< 3, 32, 3, false > { typedef rgb32f_image_t type; };
template<> struct tiff_format< 3, 64, 3, false > { typedef rgb64f_image_t type; };

template<> struct tiff_format< 3,  8, 1, true > { typedef rgb8_planar_image_t type; };
template<> struct tiff_format< 3, 16, 1, true > { typedef rgb16_planar_image_t type; };
template<> struct tiff_format< 3, 32, 1, true > { typedef rgb32_planar_image_t type; };
template<> struct tiff_format< 3,  8, 2, true > { typedef rgb8s_planar_image_t type; };
template<> struct tiff_format< 3, 16, 2, true > { typedef rgb16s_planar_image_t type; };
template<> struct tiff_format< 3, 32, 2, true > { typedef rgb32s_planar_image_t type; };
template<> struct tiff_format< 3, 32, 3, true > { typedef rgb32f_planar_image_t type; };
template<> struct tiff_format< 3, 64, 3, true > { typedef rgb64f_planar_image_t type; };

template<> struct tiff_format< 4,  8, 1, false > { typedef rgba8_image_t type; };
template<> struct tiff_format< 4, 16, 1, false > { typedef rgba16_image_t type; };
template<> struct tiff_format< 4, 32, 1, false > { typedef rgba32_image_t type; };
template<> struct tiff_format< 4,  8, 2, false > { typedef rgba8s_image_t type; };
template<> struct tiff_format< 4, 16, 2, false > { typedef rgba16s_image_t type; };
template<> struct tiff_format< 4, 32, 2, false > { typedef rgba32s_image_t type; };
template<> struct tiff_format< 4, 32, 3, false > { typedef rgba32f_image_t type; };
template<> struct tiff_format< 4, 64, 3, false > { typedef rgba64f_image_t type; };

template<> struct tiff_format< 4,  8, 1, true > { typedef rgba8_planar_image_t type; };
template<> struct tiff_format< 4, 16, 1, true > { typedef rgba16_planar_image_t type; };
template<> struct tiff_format< 4, 32, 1, true > { typedef rgba32_planar_image_t type; };
template<> struct tiff_format< 4,  8, 2, true > { typedef rgba8s_planar_image_t type; };
template<> struct tiff_format< 4, 16, 2, true > { typedef rgba16s_planar_image_t type; };
template<> struct tiff_format< 4, 32, 2, true > { typedef rgba32s_planar_image_t type; };
template<> struct tiff_format< 4, 32, 3, true > { typedef rgba32f_planar_image_t type; };
template<> struct tiff_format< 4, 64, 3, true > { typedef rgba64f_planar_image_t type; };

} // namespace details
} // namespace gil
} // namespace boost

#endif // BOOST_GIL_EXTENSION_IO_TIFF_SUPPORTED_READ_TYPES_HPP_INCLUDED
