/*
    Copyright 2007-2008 Christian Henning, Andreas Pokorny, Lubomir Bourdev
    Use, modification and distribution are subject to the Boost Software License,
    Version 1.0. (See accompanying file LICENSE_1_0.txt or copy at
    http://www.boost.org/LICENSE_1_0.txt).
*/

/*************************************************************************************************/

#ifndef BOOST_GIL_EXTENSION_IO_PNG_TAGS_HPP
#define BOOST_GIL_EXTENSION_IO_PNG_TAGS_HPP 

////////////////////////////////////////////////////////////////////////////////////////
/// \file               
/// \brief
/// \author Christian Henning, Andreas Pokorny, Lubomir Bourdev \n
///         
/// \date   2007-2008 \n
///
////////////////////////////////////////////////////////////////////////////////////////

extern "C" {
#include <png.h>
}
#include "detail/base.hpp"

namespace boost { namespace gil {

struct png_tag : format_tag {};

struct png_image_width
{
    typedef png_uint_32 type;
};

struct png_image_height
{
    typedef png_uint_32 type;
};

struct png_interlace_method
{
    typedef int type;
};

struct png_compression_method
{
    typedef int type;
};

struct png_filter_method
{
    typedef int type;
};


struct png_bitdepth
{
    typedef int type;
};

struct png_num_channels
{
    typedef png_byte type;
};

struct png_color_type
{
    typedef int type;
};

struct png_significant_bits
{
    typedef png_color_8p type;
};


struct png_x_resolution
{
    typedef png_uint_32 type;
};

struct png_y_resolution
{
    typedef png_uint_32 type;
};

/// @todo srgb intent + gamma ?
struct png_gamma
{
#ifdef PNG_FLOATING_POINT_SUPPORTED
    typedef double type;
#else 
    typedef png_fixed_point type;
#endif
};

/// @todo Support for text?


template<>
struct image_read_info<png_tag>
{
    png_image_width::type  _width;
    png_image_height::type _height;

    png_num_channels::type _num_channels;
    png_bitdepth::type _bit_depth;
    png_color_type::type  _color_type;

    png_interlace_method::type _interlace_method;
    png_compression_method::type _compression_method;
    png_filter_method::type _filter_method;


    png_gamma::type _gamma;
    png_x_resolution::type _x_res;
    png_y_resolution::type _y_res;
    png_significant_bits::type _sbits;
};

template<>
struct image_write_info<png_tag>
{
    png_interlace_method::type _interlace_method;
    png_compression_method::type _compression_method;
    png_filter_method::type _filter_method;

    png_gamma::type _gamma;
    png_x_resolution::type _x_res;
    png_y_resolution::type _y_res;
    png_significant_bits::type _sbits;
};

} // namespace gil
} // namespace boost

#endif // BOOST_GIL_EXTENSION_IO_PNG_TAGS_HPP
