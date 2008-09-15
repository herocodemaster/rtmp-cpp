/*
    Copyright 2007-2008 Christian Henning, Andreas Pokorny, Lubomir Bourdev
    Use, modification and distribution are subject to the Boost Software License,
    Version 1.0. (See accompanying file LICENSE_1_0.txt or copy at
    http://www.boost.org/LICENSE_1_0.txt).
*/

/*************************************************************************************************/

#ifndef BOOST_GIL_EXTENSION_IO_READ_IMAGE_HPP_INCLUDED
#define BOOST_GIL_EXTENSION_IO_READ_IMAGE_HPP_INCLUDED

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

// ------ Image --- Reader -------- no conversion -----------


/// \ingroup IO
template < typename Device
         , typename Image
         , typename FormatTag
         > 
inline 
void read_image( Device&          file
               , Image&           img
               , const point_t&   top_left
               , const point_t&   dim
               , const FormatTag& tag
               , typename enable_if< typename mpl::and_< typename detail::is_input_device< Device    >::type
                                                       , typename is_format_tag          < FormatTag >::type
                                                       , typename is_supported           < typename Image::value_type
                                                                                         , FormatTag 
                                                                                         >::type 
                                                                       >::type 
                                   >::type* ptr = 0
               )
{
    detail::reader< Device
                  , FormatTag
                  , detail::read_and_no_convert
                  > reader( file );

    reader.init_image( img
                     , top_left
                     , dim
                     , reader.get_info()
                     );

    reader.apply( view( img ));
}

template < typename Device
         , typename Image
         , typename FormatTag
         > 
inline void read_image( Device&          file
                      , Image&           img
                      , const point_t&   top_left
                      , const point_t&   dim
                      , const FormatTag& tag
                      , typename enable_if< typename mpl::and_< typename detail::is_adaptable_input_device< FormatTag
                                                                                                                         , Device
                                                                                                                         >::type
                                                                             , typename is_format_tag<FormatTag>::type
                                                                             , typename is_supported< typename Image::value_type
                                                                                                    , FormatTag
                                                                                                    >::type 
                                                                             >::type
                                                         >::type* ptr = 0
                      )
{
    typedef typename detail::is_adaptable_input_device<Device>::device_type device_type;
    device_type dev(file);
    detail::reader<device_type,FormatTag,detail::read_and_no_convert> reader(dev);

    reader.init_image( img
                     , top_left
                     , dim
                     , reader.get_info
                     );

    reader.apply( view( img ));
}

template < typename String
         , typename Image
         , typename FormatTag
         > 
inline 
void read_image( const String& file_name
               , Image& img
               , const point_t& top_left
               , const point_t& dim
               , const FormatTag& tag
               , typename enable_if< typename mpl::and_< typename detail::is_supported_path_spec< String >::type
                                                                     , typename is_format_tag< FormatTag >::type
                                                                     , typename is_supported< typename Image::value_type
                                                                                            , FormatTag
                                                                                            >::type
                                                                     >::type
                                                 >::type* ptrdiff_t = 0
               )
{
    detail::file_stream_device<FormatTag> device( detail::convert_to_string( file_name )
                                                , detail::file_stream_device<FormatTag>::read_tag()
                                                );

    read_image( device
              , img
              , top_left
              , dim
              , tag
              );
}

/// \ingroup IO
template < typename Device
         , typename Image
         , typename FormatTag
         > 
inline
void read_image( Device& file
               , Image& img
               , const FormatTag& tag
               , typename enable_if< typename mpl::and_< typename detail::is_input_device< Device    >::type
                                                       , typename is_format_tag          < FormatTag >::type
                                                       , typename is_supported           < typename Image::value_type
                                                                                         , FormatTag 
                                                                                         >::type 
                                                       >::type 
                                   >::type* ptr = 0 
               )
{
    read_image( file
              , img
              , point_t( 0, 0 )
              , point_t( 0, 0 )
              , tag
              );
}

/// \ingroup IO
template < typename Device
         , typename Image
         , typename FormatTag
         > 
inline
void read_image( Device&          file
               , Image&           img
               , const FormatTag& tag
               , typename enable_if< typename mpl::and_< typename detail::is_adaptable_input_device< FormatTag
                                                                                                                         , Device
                                                                                                                         >::type
                                                                             , typename is_format_tag<FormatTag>::type
                                                                             , typename is_supported< typename Image::value_type
                                                                                                    , FormatTag
                                                                                                    >::type 
                                                                             >::type
                                                         >::type* ptr = 0 
               )
{
    typedef typename detail::is_adaptable_input_device< FormatTag, Device >::device_type device_type;
    device_type dev( file );

    read_image( dev
              , img
              , point_t( 0, 0 )
              , point_t( 0, 0 )
              , tag
              );
}

/// \ingroup IO
template < typename String
         , typename Image
         , typename FormatTag
         > 
inline
void read_image( const String&    file_name
               , Image&           img
               , const FormatTag& tag
               , typename enable_if< typename mpl::and_< typename detail::is_supported_path_spec< String >::type
                                                       , typename is_format_tag< FormatTag >::type
                                                       , typename is_supported< typename Image::value_type
                                                                              , FormatTag
                                                                              >::type
                                                       >::type
                                   >::type* ptrdiff_t = 0
               )
{
    detail::file_stream_device<FormatTag> device( detail::convert_to_string( file_name )
                                                , detail::file_stream_device<FormatTag>::read_tag()
                                                );

    read_image( device
              , img
              , point_t( 0, 0 )
              , point_t( 0, 0 )
              , tag
              );
}

} // namespace gil
} // namespace boost

#endif // BOOST_GIL_EXTENSION_IO_READ_IMAGE_HPP_INCLUDED