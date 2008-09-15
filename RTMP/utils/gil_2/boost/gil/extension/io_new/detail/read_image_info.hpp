/*
    Copyright 2007-2008 Christian Henning, Andreas Pokorny, Lubomir Bourdev
    Use, modification and distribution are subject to the Boost Software License,
    Version 1.0. (See accompanying file LICENSE_1_0.txt or copy at
    http://www.boost.org/LICENSE_1_0.txt).
*/

/*************************************************************************************************/

#ifndef BOOST_GIL_EXTENSION_IO_IMAGE_READ_INFO_HPP_INCLUDED
#define BOOST_GIL_EXTENSION_IO_IMAGE_READ_INFO_HPP_INCLUDED

////////////////////////////////////////////////////////////////////////////////////////
/// \file               
/// \brief
/// \author Christian Henning, Andreas Pokorny, Lubomir Bourdev \n
///         
/// \date   2007-2008 \n
///
////////////////////////////////////////////////////////////////////////////////////////

#include <boost/type_traits/is_base_and_derived.hpp>
#include <boost/mpl/and.hpp>
#include <boost/utility/enable_if.hpp>

#include "base.hpp"
#include "io_device.hpp"
#include "path_spec.hpp"

namespace boost{ namespace gil {


/// \ingroup IO
/// \brief Returns the image info. Image info is file format specific.
template< typename Device
        , typename FormatTag
        >
inline
image_read_info< FormatTag >
read_image_info( Device&         file
               , const FormatTag tag
               , typename enable_if< typename mpl::and_< typename is_format_tag< FormatTag >::type
                                                       , typename detail::is_input_device<Device>::type
                                                       >::type
                                   >::type* ptr = 0
               )
{
    return detail::reader< Device
                         , FormatTag
                         , detail::read_and_no_convert
                         >( file ).get_info();
}

/// \ingroup IO
/// \brief Returns the image info. Image info is file format specific.
template< typename Device
        , typename FormatTag
        >
inline 
image_read_info<FormatTag>
read_image_info( Device&          file
               , const FormatTag& tag
               , typename enable_if< typename mpl::and_< typename is_format_tag<FormatTag>::type
                                                       , typename detail::is_adaptable_input_device< FormatTag
                                                                                                   , Device
                                                                                                   >::type
                                                                         >::type
                                   >::type* ptr = 0
               )
{
    typedef typename detail::is_adaptable_input_device< FormatTag
                                                      , Device
                                                      >::device_type device_type;

    device_type dev( file );

    return detail::reader< device_type
                         , FormatTag
                         , detail::read_and_no_convert
                         >( dev ).get_info();
}

/// \ingroup IO
/// \brief Returns the image info. Image info is file format specific.
template< typename String
        , typename FormatTag
        >
inline 
image_read_info<FormatTag>  
read_image_info( const String&    file_name
               , const FormatTag& tag
               , typename enable_if< typename mpl::and_< typename is_format_tag< FormatTag >::type
                                                       , typename detail::is_supported_path_spec< String >::type
                                                       >::type
                                   >::type* ptr = 0
               )
{
    detail::file_stream_device< FormatTag > reader( detail::convert_to_string( file_name )
                                                  , detail::file_stream_device< FormatTag >::read_tag()
                                                  );

    return read_image_info( reader
                          , tag    );
}

} // namespace gil
} // namespace boost

#endif // BOOST_GIL_EXTENSION_IO_IMAGE_READ_INFO_HPP_INCLUDED