/*
    Copyright 2007-2008 Andreas Pokorny, Christian Henning
    Use, modification and distribution are subject to the Boost Software License,
    Version 1.0. (See accompanying file LICENSE_1_0.txt or copy at
    http://www.boost.org/LICENSE_1_0.txt).
*/

/*************************************************************************************************/

#ifndef BOOST_GIL_EXTENSION_IO_DETAIL_TIFF_IO_DEVICE_HPP_INCLUDED
#define BOOST_GIL_EXTENSION_IO_DETAIL_TIFF_IO_DEVICE_HPP_INCLUDED

////////////////////////////////////////////////////////////////////////////////////////
/// \file               
/// \brief 
/// \author Andreas Pokorny, Christian Henning \n
///         
/// \date   2007-2008 \n
///
////////////////////////////////////////////////////////////////////////////////////////

extern "C" {
#include "tiff.h"
#include "tiffio.h"
}

#include <boost/shared_ptr.hpp>
#include <boost/utility/enable_if.hpp>

#include "detail/base.hpp"
#include "detail/io_device.hpp"

namespace boost { namespace gil { namespace detail {

/*!
 *
 * file_stream_device specialization for tiff images, which are based on TIFF*.
 */
template<>
class file_stream_device< tiff_tag >
{
public:

    struct read_tag {};
    struct write_tag {};

    file_stream_device( std::string const& file_name, read_tag )
    {
        TIFF* tiff;

        io_error_if( ( tiff = TIFFOpen( file_name.c_str(), "r" )) == NULL
                   , "file_stream_device: failed to open file" );

        _tiff_file = tiff_file_t( tiff, TIFFClose );
    }

    file_stream_device( std::string const& file_name, write_tag )
    {
        TIFF* tiff;

        io_error_if( ( tiff = TIFFOpen( file_name.c_str(), "w" )) == NULL
                   , "file_stream_device: failed to open file" );

        _tiff_file = tiff_file_t( tiff, TIFFClose );
    }

    file_stream_device( TIFF* tiff_file )
    : _tiff_file( tiff_file
                , TIFFClose ) {}


    unsigned int get_default_strip_size()
    {
        return TIFFDefaultStripSize( _tiff_file.get()
                                   , 0 );
    }

    template <typename Property>
    bool get_property( typename Property::type& value  )
    {
        if( TIFFGetFieldDefaulted( _tiff_file.get()
                                 , Property::tag
                                 , &value ) == 1 )
        {
            return true;
        }

        return false;
    }
   

    template <typename Property>
    inline
    bool set_property( const typename Property::type& value )
    {
       if( TIFFSetField( _tiff_file.get()
                       , Property::tag
                       , value  ) == 1 )
       {
          return true;
       }

       return false;
    }

    template< typename Buffer >
    void read_scaline( Buffer&        buffer
                     , std::ptrdiff_t row
                     , tsample_t      plane   )
    {
        io_error_if( TIFFReadScanline( _tiff_file.get()
                                     , &buffer.front()
                                     , (uint32) row
                                     , plane           ) == -1
                   , "Read error."
                   );
    }

   bool are_bytes_swapped()
   {
      TIFFIsByteSwapped( _tiff_file.get() );
   }

   std::size_t get_scanline_size()
   {
      return TIFFScanlineSize( _tiff_file.get() );
   }


   int get_field_defaulted( boost::uint16_t*& red
                          , boost::uint16_t*& green
                          , boost::uint16_t*& blue  )
   {
      return TIFFGetFieldDefaulted( _tiff_file.get()
                                  , TIFFTAG_COLORMAP
                                  , &red
                                  , &green
                                  , &blue              );
   }

    template< typename Buffer >
    inline 
    void write_scaline( Buffer&     buffer
                      , uint32      row
                      , tsample_t   plane
                      )
    {
       io_error_if( TIFFWriteScanline( _tiff_file.get()
                                     , &buffer.front()
                                     , row
                                     , plane 
                                     ) == -1
                   , "Write error"
                   );
    }


private:

   typedef boost::shared_ptr<TIFF> tiff_file_t;
   tiff_file_t _tiff_file;
};

/*
template< typename T, typename D >
struct is_adaptable_input_device< tiff_tag, T, D > : mpl::false_{};
*/

template< typename FormatTag >
struct is_adaptable_input_device< FormatTag
                                , TIFF*
                                , void
                                >
    : mpl::true_
{
    typedef file_stream_device< FormatTag > device_type;
};

template< typename FormatTag >
struct is_adaptable_output_device< FormatTag
                                 , TIFF*
                                 , void
                                 >
    : mpl::true_
{
    typedef file_stream_device< FormatTag > device_type;
};

} // namespace detail
} // namespace gil
} // namespace boost

#endif // BOOST_GIL_EXTENSION_IO_DETAIL_TIFF_IO_DEVICE_HPP_INCLUDED
