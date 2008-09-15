/*
    Copyright 2007-2008 Christian Henning, Andreas Pokorny, Lubomir Bourdev
    Use, modification and distribution are subject to the Boost Software License,
    Version 1.0. (See accompanying file LICENSE_1_0.txt or copy at
    http://www.boost.org/LICENSE_1_0.txt).
*/

/*************************************************************************************************/

#ifndef BOOST_GIL_EXTENSION_IO_TIFF_IO_HPP_INCLUDED
#define BOOST_GIL_EXTENSION_IO_TIFF_IO_HPP_INCLUDED

////////////////////////////////////////////////////////////////////////////////////////
/// \file               
/// \brief
/// \author Christian Henning, Andreas Pokorny, Lubomir Bourdev \n
///         
/// \date   2007-2008 \n
///
////////////////////////////////////////////////////////////////////////////////////////

#include <boost/mpl/not.hpp>
#include <boost/type_traits/is_same.hpp>

#include <boost/gil/channel.hpp>
#include <boost/gil/color_base.hpp>

namespace boost{ namespace gil {

namespace detail {

// TIFF virtually supports everything 
struct tiff_rw_support {
    BOOST_STATIC_CONSTANT(bool,is_supported=true);
};

} // namespace detail

template<typename PixelType>
struct is_supported<PixelType,tiff_tag>
    : mpl::bool_< detail::tiff_rw_support::is_supported >
{};

} // namespace gil
} // namespace boost

#endif // BOOST_GIL_EXTENSION_IO_TIFF_IO_HPP_INCLUDED
