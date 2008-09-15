/*
    Copyright 2007-2008 Christian Henning, Andreas Pokorny, Lubomir Bourdev
    Use, modification and distribution are subject to the Boost Software License,
    Version 1.0. (See accompanying file LICENSE_1_0.txt or copy at
    http://www.boost.org/LICENSE_1_0.txt).
*/

/*************************************************************************************************/

#ifndef BOOST_GIL_EXTENSION_IO_PNG_SUPPORTED_TYPES_HPP
#define BOOST_GIL_EXTENSION_IO_PNG_SUPPORTED_TYPES_HPP

////////////////////////////////////////////////////////////////////////////////////////
/// \file               
/// \brief 
/// \author Christian Henning, Andreas Pokorny, Lubomir Bourdev \n
///         
/// \date   2007-2008 \n
///
////////////////////////////////////////////////////////////////////////////////////////

#ifdef ENABLE_GRAY_ALPHA
#include "boost/gil/extension/toolbox/gray_alpha.hpp"
#endif // ENABLE_GRAY_ALPHA

namespace boost { namespace gil { namespace detail {

static const size_t PNG_BYTES_TO_CHECK = 4;

template <typename Channel,typename ColorSpace>
struct png_rw_support {
    BOOST_STATIC_CONSTANT(bool,is_supported=false);
    BOOST_STATIC_CONSTANT(int,bit_depth=0);
    BOOST_STATIC_CONSTANT(int,color_type=0);
};
template <typename BitField,bool Mutable>
struct png_rw_support<packed_dynamic_channel_reference<BitField,1,Mutable>,gray_t> {
    BOOST_STATIC_CONSTANT(bool,is_supported=true);
    BOOST_STATIC_CONSTANT(int,bit_depth=1);
    BOOST_STATIC_CONSTANT(int,color_type=PNG_COLOR_TYPE_GRAY);
};
template <typename BitField,bool Mutable>
struct png_rw_support<packed_dynamic_channel_reference<BitField,2,Mutable>,gray_t> {
    BOOST_STATIC_CONSTANT(bool,is_supported=true);
    BOOST_STATIC_CONSTANT(int,bit_depth=2);
    BOOST_STATIC_CONSTANT(int,color_type=PNG_COLOR_TYPE_GRAY);
};
template <typename BitField, bool Mutable>
struct png_rw_support<packed_dynamic_channel_reference<BitField,4,Mutable>,gray_t> {
    BOOST_STATIC_CONSTANT(bool,is_supported=true);
    BOOST_STATIC_CONSTANT(int,bit_depth=4);
    BOOST_STATIC_CONSTANT(int,color_type=PNG_COLOR_TYPE_GRAY);
};
template <>
struct png_rw_support<bits8,gray_t> {
    BOOST_STATIC_CONSTANT(bool,is_supported=true);
    BOOST_STATIC_CONSTANT(int,bit_depth=8);
    BOOST_STATIC_CONSTANT(int,color_type=PNG_COLOR_TYPE_GRAY);
};

#ifdef ENABLE_GRAY_ALPHA
template <>
struct png_rw_support<bits8,gray_alpha_t> {
    BOOST_STATIC_CONSTANT(bool,is_supported=true);
    BOOST_STATIC_CONSTANT(int,bit_depth=8);
    BOOST_STATIC_CONSTANT(int,color_type=PNG_COLOR_TYPE_GA);
};
#endif // ENABLE_GRAY_ALPHA

template <>
struct png_rw_support<bits8,rgb_t> {
    BOOST_STATIC_CONSTANT(bool,is_supported=true);
    BOOST_STATIC_CONSTANT(int,bit_depth=8);
    BOOST_STATIC_CONSTANT(int,color_type=PNG_COLOR_TYPE_RGB);
};

template <>
struct png_rw_support<bits8,rgba_t> {
    BOOST_STATIC_CONSTANT(bool,is_supported=true);
    BOOST_STATIC_CONSTANT(int,bit_depth=8);
    BOOST_STATIC_CONSTANT(int,color_type=PNG_COLOR_TYPE_RGBA);
};
template <>
struct png_rw_support<bits16,gray_t> {
    BOOST_STATIC_CONSTANT(bool,is_supported=true);
    BOOST_STATIC_CONSTANT(int,bit_depth=16);
    BOOST_STATIC_CONSTANT(int,color_type=PNG_COLOR_TYPE_GRAY);
};
template <>
struct png_rw_support<bits16,rgb_t> {
    BOOST_STATIC_CONSTANT(bool,is_supported=true);
    BOOST_STATIC_CONSTANT(int,bit_depth=16);
    BOOST_STATIC_CONSTANT(int,color_type=PNG_COLOR_TYPE_RGB);
};
template <>
struct png_rw_support<bits16,rgba_t> {
    BOOST_STATIC_CONSTANT(bool,is_supported=true);
    BOOST_STATIC_CONSTANT(int,bit_depth=16);
    BOOST_STATIC_CONSTANT(int,color_type=PNG_COLOR_TYPE_RGBA);
};

#ifdef ENABLE_GRAY_ALPHA
template <>
struct png_rw_support<bits16,gray_alpha_t> {
    BOOST_STATIC_CONSTANT(bool,is_supported=true);
    BOOST_STATIC_CONSTANT(int,bit_depth=16);
    BOOST_STATIC_CONSTANT(int,color_type=PNG_COLOR_TYPE_GA);
};
#endif // ENABLE_GRAY_ALPHA

} // namespace detail

template<typename PixelType>
struct is_supported<PixelType,png_tag>
    //!\todo this should be lazy: -- channel_type is undefined for non-homogenous pixel types
    : mpl::bool_<detail::png_rw_support<
                    typename channel_type<PixelType>::type,
                    typename color_space_type<PixelType>::type
                    >::is_supported>
{};

} // namespace gil
} // namespace boost


#endif


