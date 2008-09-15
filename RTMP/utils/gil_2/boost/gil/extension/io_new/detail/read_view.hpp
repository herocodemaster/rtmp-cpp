/*
    Copyright 2007-2008 Christian Henning, Andreas Pokorny, Lubomir Bourdev
    Use, modification and distribution are subject to the Boost Software License,
    Version 1.0. (See accompanying file LICENSE_1_0.txt or copy at
    http://www.boost.org/LICENSE_1_0.txt).
*/

/*************************************************************************************************/

#ifndef BOOST_GIL_EXTENSION_IO_READ_VIEW_HPP_INCLUDED
#define BOOST_GIL_EXTENSION_IO_READ_VIEW_HPP_INCLUDED

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

// ------ View --- Reader -------- no conversion -----------

/// \ingroup IO
template < typename Device
         , typename View
         , typename FormatTag
         >
inline 
void read_view( Device&          file
              , const View&      view
              , const point_t&   top_left
              , const FormatTag& tag
              , typename enable_if< typename mpl::and_< typename detail::is_input_device< Device    >::type
                                                       , typename is_format_tag          < FormatTag >::type
                                                       , typename is_supported           < typename View::value_type
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

    reader.init_view( view
                    , top_left
                    , reader.get_info()
                    );

    reader.apply( view );
}

template < typename Device
         , typename View
         , typename FormatTag
         > 
inline 
void read_view( Device&          file
              , const View&      view
              , const point_t&   top_left
              , const FormatTag& tag
              , typename enable_if< typename mpl::and_< typename detail::is_adaptable_input_device< FormatTag
                                                                                                                         , Device
                                                                                                                         >::type
                                                                             , typename is_format_tag<FormatTag>::type
                                                                             , typename is_supported< typename View::value_type
                                                                                                    , FormatTag
                                                                                                    >::type 
                                                                             >::type
                                                         >::type* ptr = 0 
              )
{
    typedef typename detail::is_adaptable_input_device< FormatTag
                                                      , Device
                                                      >::device_type device_type;

    device_type dev( file );

    detail::reader< device_type
                  , FormatTag
                  , detail::read_and_no_convert
                  > reader(dev);

    reader.init_view( view
                    , top_left
                    , reader.get_info()
                    );

    reader.apply( view );
}

template < typename String
         , typename View
         , typename FormatTag
         > 
inline 
void read_view( const String& file_name
              , const View& view
              , const point_t& top_left
              , const FormatTag& tag
              , typename enable_if< typename mpl::and_< typename detail::is_supported_path_spec< String >::type
                                                                     , typename is_format_tag< FormatTag >::type
                                                                     , typename is_supported< typename View::value_type
                                                                                            , FormatTag
                                                                                            >::type
                                                                     >::type
                                                 >::type* ptrdiff_t = 0
              )
{
    detail::file_stream_device<FormatTag> device( detail::convert_to_string( file_name )
                                                , detail::file_stream_device< FormatTag >::read_tag()
                                                );

    read_view( device
             , view
             , top_left
             , tag
             );
}

/// \ingroup IO
/// \brief Loads the image specified by the \a file_name into the given \a view.
template < typename String
         , typename View
         , typename FormatTag
         > 
inline 
void read_view( const String&    file_name
              , const View&      view
              , FormatTag const& tag
              , typename enable_if< typename mpl::and_< typename detail::is_supported_path_spec< String >::type
                                                                     , typename is_format_tag< FormatTag >::type
                                                                     , typename is_supported< typename View::value_type
                                                                                            , FormatTag
                                                                                            >::type
                                                                     >::type
                                                 >::type* ptrdiff_t = 0
              )
{
    read_view( file_name
             , view
             , point_t( 0, 0 )
             , tag
             );
}

template< typename Device
        , typename View
        , typename FormatTag
        > 
inline 
void read_view( Device&          device
              , const View&      view
              , const FormatTag& tag
              , typename enable_if< typename mpl::and_< typename is_format_tag<FormatTag>::type
                                                                                       , typename mpl::or_< typename detail::is_input_device< Device >::type
                                                                                                          , typename detail::is_adaptable_input_device< FormatTag
                                                                                                                                                      , Device
                                                                                                                                                      >::type
                                                                                                          >::type
                                                                                       , typename is_supported< typename View::value_type, FormatTag>::type
                                                                                       >::type
                                                                   >::type* ptr = 0
              )
{
    read_view( device
             , view
             , point_t(0,0)
             , tag
             );
}

} // namespace gil
} // namespace boost

#endif // BOOST_GIL_EXTENSION_IO_READ_VIEW_HPP_INCLUDED