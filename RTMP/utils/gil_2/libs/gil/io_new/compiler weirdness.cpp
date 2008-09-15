// remove all read_and_convert_image and there are no errors.

#include "stdafx.h"

#include <boost/mpl/and.hpp>
#include <boost/utility/enable_if.hpp>

#include <boost/type_traits/is_base_and_derived.hpp>
#include <boost/mpl/and.hpp>
#include <boost/utility/enable_if.hpp>
#include <boost/gil/extension/io_new/detail/io_device.hpp>
#include <boost/gil/extension/io_new/detail/base.hpp>
#include <boost/gil/extension/io_new/detail/path_spec.hpp>
#include <boost/gil/extension/io_new/detail/conversion_policies.hpp>

namespace boost{ namespace gil {
/*
namespace detail {
    template<typename Device, typename FormatTag, typename ConversionPolicy>
    struct reader;
    template<typename Device, typename FormatTag>
    struct writer;
}

struct format_tag {};

template<typename PixelType, typename FormatTag> struct is_supported;

template<typename FormatTag> struct is_format_tag : is_base_and_derived<format_tag,FormatTag> {};

template<typename FormatTag> struct image_read_info;
template<typename FormatTag> struct image_write_info;

/// \ingroup IO
template< typename Device
        , typename Image
        , typename ColorConverter
        , typename FormatTag
        >
inline
void read_and_convert_image( Device&        file
                           , Image&         img
                           , const point_t& top_left
                           , const point_t& dim
                           , const ColorConverter& cc
                           , const FormatTag& tag
                           , typename enable_if< typename mpl::and_< typename is_format_tag< FormatTag >::type
                                                                    , typename detail::is_input_device<Device>::type
                                                                    >::type
                                                >::type* ptr = 0
                           )
{
    typedef detail::read_and_convert< ColorConverter > reader_color_convert;

    detail::reader< Device
                  , FormatTag
                  , reader_color_convert
                  > reader( file
                          , cc
                          );

    reader.read_image( img
                     , top_left
                     , dim
                     );
}

template< typename Device
        , typename Image
        , typename ColorConverter
        , typename FormatTag
        >
inline
void read_and_convert_image( Device&               file
                           , Image&                img
                           , const point_t&        top_left
                           , const point_t&        dim
                           , const ColorConverter& cc
                           , const FormatTag&      tag
                           , typename enable_if< typename mpl::and_< typename is_format_tag<FormatTag>::type
                                                                   , typename detail::is_adaptable_input_device< FormatTag
                                                                                                               , Device
                                                                                                               >::type
                                                                         >::type
                                                     >::type* ptr = 0
                           )
{
    typedef typename detail::is_adaptable_input_device< Device >::device_type device_type;
    device_type dev( file );

    read_and_convert_image( dev
                          , img
                          , top_left
                          , cc
                          , tag
                          );
}

template < typename String
         , typename Image
         , typename ColorConverter
         , typename FormatTag
         >
inline
void read_and_convert_image( const String& file_name
                           , Image& img
                           , const point_t& top_left
                           , const point_t& dim
                           , const ColorConverter& cc
                           , const FormatTag& tag
                           , typename enable_if< typename mpl::and_< typename is_format_tag< FormatTag >::type
                                                                 , typename detail::is_supported_path_spec< String >::type
                                                                 >::type
                                            >::type* ptr = 0
                           )
{
    detail::file_stream_device< FormatTag > device( detail::convert_to_string( file_name )
                                                  , detail::file_stream_device< FormatTag >::read_tag()
                                                  );

    read_and_convert_image( device
                          , img
                          , top_left
                          , dim
                          , cc
                          , tag
                          );
}

/// \ingroup IO
template < typename String
         , typename Image
         , typename ColorConverter
         , typename FormatTag
         >
inline
void read_and_convert_image( const String& file_name
                           , Image& img
                           , const ColorConverter& cc
                           , const FormatTag& tag
                           , typename enable_if< typename mpl::and_< typename is_format_tag< FormatTag >::type
                                                                 , typename detail::is_supported_path_spec< String >::type
                                                                 >::type
                                            >::type* ptr = 0
                           )
{
    read_and_convert_image( file_name
                          , img
                          , point_t( 0, 0 )
                          , point_t( 0, 0 )
                          , cc
                          , tag
                          );
}

template < typename Device
         , typename Image
         , typename ColorConverter
         , typename FormatTag
         >
inline
void read_and_convert_image( Device&               device
                           , Image&                img
                           , const ColorConverter& cc
                           , const FormatTag&      tag
                           , typename enable_if< typename mpl::and_< typename mpl::or_< typename detail::is_input_device< Device >::type
                                                                                                      , typename detail::is_adaptable_input_device< FormatTag
                                                                                                                                                  , Device
                                                                                                                                                  >::type
                                                                                                      >::type
                                                                                   , typename is_format_tag< FormatTag >::type
                                                                                   >::type
                                                               >::type* ptr = 0 
                           )
{
    read_and_convert_image( device
                          , img
                          , point_t( 0, 0 )
                          , point_t( 0, 0 )
                          , cc
                          , tag
                          );
}

/// \ingroup IO
template < typename String
         , typename Image
         , typename FormatTag
         >
inline
void read_and_convert_image( const String&    file_name
                           , Image&           img
                           , const point_t&   top_left
                           , const point_t&   dim
                           , const FormatTag& tag
                           , typename enable_if< typename mpl::and_< typename is_format_tag< FormatTag >::type
                                                                 , typename detail::is_supported_path_spec< String >::type
                                                                 >::type
                                            >::type* ptr = 0
                           )
{
   read_and_convert_image( file_name
                         , img
                         , top_left
                         , dim
                         , default_color_converter()
                         , tag
                         );
}

template < typename Device
         , typename Image
         , typename FormatTag
         >
inline
void read_and_convert_image( Device& device
                           , Image& img
                           , const point_t& top_left
                           , const point_t& dim 
                           , const FormatTag& tag
                           , typename enable_if< typename mpl::and_< typename mpl::or_< typename detail::is_input_device< Device >::type
                                                                                      , typename detail::is_adaptable_input_device< FormatTag
                                                                                                                                  , Device
                                                                                                                                  >::type
                                                                                      >::type
                                                                   , typename is_format_tag< FormatTag >::type
                                                                   >::type
                                               >::type* ptr = 0
                           )
{
   read_and_convert_image( device
                         , img
                         , top_left
                         , dim
                         , default_color_converter()
                         , tag
                         );
}

/// \ingroup IO
template < typename String
         , typename Image
         , typename FormatTag
         >
inline
void read_and_convert_image( const String&    file_name
                           , Image&           img
                           , const FormatTag& tag
                           , typename enable_if< typename mpl::and_< typename is_format_tag< FormatTag >::type
                                                                 , typename detail::is_supported_path_spec< String >::type
                                                                 >::type
                                            >::type* ptr = 0
                           )
{
   read_and_convert_image( file_name
                         , img
                         , point_t( 0, 0 )
                         , point_t( 0, 0 )
                         , default_color_converter()
                         , tag
                         );
}

template < typename Device
         , typename Image
         , typename FormatTag
         >
inline
void read_and_convert_image( Device&          device
                           , Image&           img
                           , const FormatTag& tag
                           , typename enable_if< typename mpl::and_< typename mpl::or_< typename detail::is_input_device< Device >::type
                                                                                      , typename detail::is_adaptable_input_device< FormatTag
                                                                                                                                  , Device
                                                                                                                                  >::type
                                                                                      >::type
                                                                   , typename is_format_tag< FormatTag >::type
                                                                   >::type
                                               >::type* ptr = 0
                           )
{
   read_and_convert_image( device
                         , img
                         , point_t( 0, 0 )
                         , point_t( 0, 0 )
                         , default_color_converter()
                         , tag
                         );
}

// ----- View --- Reader --------- With ColorConverter -----------------

/// \ingroup IO
template< typename Device
        , typename View
        , typename ColorConverter
        , typename FormatTag
        >
inline
void read_and_convert_view( Device&               file
                          , const View&           view
                          , const point_t&        top_left
                          , const point_t&        dim
                          , const ColorConverter& cc
                          , const FormatTag&      tag
                          , typename enable_if< typename mpl::and_< typename is_format_tag< FormatTag >::type
                                                                  , typename detail::is_input_device< Device >::type
                                                                  >::type
                                              >::type* ptr = 0
                          )
{
    typedef detail::read_and_convert< ColorConverter > reader_color_convert;

    detail::reader< Device
                  , FormatTag
                  , reader_color_convert
                  > reader( file
                          , cc
                          );

    reader.read_view( view
                     , top_left
                     , dim
                     );
}


template< typename Device
        , typename View
        , typename ColorConverter
        , typename FormatTag
        >
inline
void read_and_convert_view( Device&               file
                          , const View&           view
                          , const point_t&        top_left
                          , const point_t&        dim  
                          , const ColorConverter& cc
                          , const FormatTag&      tag
                          , typename enable_if< typename mpl::and_< typename is_format_tag< FormatTag >::type
                                                                  , typename detail::is_adaptable_input_device< FormatTag
                                                                                                              , Device
                                                                                                              >::type
                                                                  >::type
                                              >::type* ptr = 0
                          )
{
    typedef typename detail::is_adaptable_input_device< Device >::device_type device_type;
    device_type dev( file );

    read_and_convert_view( dev
                         , view
                         , top_left
                         , cc
                         , tag
                         );
}

template < typename String
         , typename View
         , typename ColorConverter
         , typename FormatTag
         >
inline
void read_and_convert_view( const String&         file_name
                          , const View&           view
                          , const point_t&        top_left
                          , const point_t&        dim
                          , const ColorConverter& cc
                          , const FormatTag&      tag
                          , typename enable_if< typename mpl::and_< typename is_format_tag< FormatTag >::type
                                                                 , typename detail::is_supported_path_spec< String >::type
                                                                 >::type
                                            >::type* ptr = 0
                          )
{
    detail::file_stream_device< FormatTag > device( detail::convert_to_string( file_name )
                                                  , detail::file_stream_device< FormatTag >::read_tag()
                                                  );

    read_and_convert_view( device
                         , view
                         , top_left
                         , dim
                         , cc
                         , tag
                         );
}

/// \ingroup IO
template < typename String
         , typename View
         , typename ColorConverter
         , typename FormatTag
         >
inline
void read_and_convert_view( const String&         file_name
                          , const View&           view
                          , const ColorConverter& cc
                          , const FormatTag&      tag
                          , typename enable_if< typename mpl::and_< typename is_format_tag< FormatTag >::type
                                                                 , typename detail::is_supported_path_spec< String >::type
                                                                 >::type
                                            >::type* ptr = 0
                          )
{
    read_and_convert_view( file_name
                         , view
                         , point_t( 0, 0 )
                         , point_t( 0, 0 )
                         , cc
                         , tag
                         );
}

template < typename Device
         , typename View
         , typename ColorConverter
         , typename FormatTag
         >
inline
void read_and_convert_view( Device&               device
                          , const View&           view
                          , const ColorConverter& cc
                          , const FormatTag&      tag
                          , typename enable_if< typename mpl::and_< typename mpl::or_< typename detail::is_input_device< Device >::type
                                                                                      , typename detail::is_adaptable_input_device< FormatTag
                                                                                                                                  , Device
                                                                                                                                  >::type
                                                                                      >::type
                                                                   , typename is_format_tag< FormatTag >::type
                                                                   >::type
                                               >::type* ptr = 0
                          )
{
    read_and_convert_view( device
                         , view
                         , point_t( 0, 0 )
                         , point_t( 0, 0 )
                         , cc
                         , tag
                         );
}

/// \ingroup IO
template < typename String
         , typename View
         , typename FormatTag
         >
inline
void read_and_convert_view( const String&    file_name
                          , const View&      view
                          , const point_t&   top_left
                          , const point_t&   dim
                          , const FormatTag& tag
                          , typename enable_if< typename mpl::and_< typename is_format_tag< FormatTag >::type
                                                                 , typename detail::is_supported_path_spec< String >::type
                                                                 >::type
                                            >::type* ptr = 0
                          )
{
   read_and_convert_view( file_name
                        , view
                        , top_left
                        , dim
                        , default_color_converter()
                        , tag
                        );
}

template < typename Device
         , typename View
         , typename FormatTag
         >
inline
void read_and_convert_view( Device&          device
                          , const View&      view
                          , const point_t&   top_left
                          , const point_t&   dim 
                          , const FormatTag& tag
                          , typename enable_if< typename mpl::and_< typename mpl::or_< typename detail::is_input_device< Device >::type
                                                                                      , typename detail::is_adaptable_input_device< FormatTag
                                                                                                                                  , Device
                                                                                                                                  >::type
                                                                                      >::type
                                                                   , typename is_format_tag< FormatTag >::type
                                                                   >::type
                                               >::type* ptr = 0
                          )
{
   read_and_convert_view( device
                        , view
                        , top_left
                        , dim
                        , default_color_converter()
                        , tag
                        );
}

/// \ingroup IO
template < typename String
         , typename View
         , typename FormatTag
         >
inline
void read_and_convert_view( const String&    file_name
                          , const View&      view
                          , const FormatTag& tag
                          , typename enable_if< typename mpl::and_< typename is_format_tag< FormatTag >::type
                                                                 , typename detail::is_supported_path_spec< String >::type
                                                                 >::type
                                            >::type* ptr = 0
                          )
{
   read_and_convert_view( file_name
                        , img
                        , point_t( 0, 0 )
                        , point_t( 0, 0 )
                        , default_color_converter()
                        , tag
                        );
}

template < typename Device
         , typename View
         , typename FormatTag
         >
inline
void read_and_convert_view( Device&          device
                          , const View&      view
                          , const FormatTag& tag
                          , typename enable_if< typename mpl::and_< typename mpl::or_< typename detail::is_input_device< Device >::type
                                                                                      , typename detail::is_adaptable_input_device< FormatTag
                                                                                                                                  , Device
                                                                                                                                  >::type
                                                                                      >::type
                                                                   , typename is_format_tag< FormatTag >::type
                                                                   >::type
                                               >::type* ptr = 0
                          )
{
   read_and_convert_view( device
                        , view
                        , point_t( 0, 0 )
                        , point_t( 0, 0 )
                        , default_color_converter()
                        , tag
                        );
}
*/
} // namespace gil
} // namespace boost


int main(int argc, char *argv[])
{
    return 0;
}
