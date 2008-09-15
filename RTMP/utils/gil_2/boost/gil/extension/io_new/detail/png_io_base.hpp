/*
    Copyright 2007-2008 Christian Henning, Andreas Pokorny, Lubomir Bourdev
    Use, modification and distribution are subject to the Boost Software License,
    Version 1.0. (See accompanying file LICENSE_1_0.txt or copy at
    http://www.boost.org/LICENSE_1_0.txt).
*/

/*************************************************************************************************/

#ifndef BOOST_GIL_EXTENSION_IO_DETAIL_PNG_IO_BASE_HPP_INCLUDED
#define BOOST_GIL_EXTENSION_IO_DETAIL_PNG_IO_BASE_HPP_INCLUDED

////////////////////////////////////////////////////////////////////////////////////////
/// \file               
/// \brief 
/// \author Christian Henning, Andreas Pokorny, Lubomir Bourdev \n
///         
/// \date   2007-2008 \n
///
////////////////////////////////////////////////////////////////////////////////////////

#include <boost/gil/extension/io_new/png_tags.hpp>

#include "base.hpp"
#include "reader_base.hpp"
#include "io_device.hpp"
#include "typedefs.hpp"
#include "png_supported_types.hpp"

namespace boost { namespace gil { namespace detail {

template<typename Device>
class png_io_base
{
public:
    png_io_base( Device & io_dev )
        : _io_dev(io_dev)
    {}

protected:
    Device & _io_dev;

    void check() const
    {
        unsigned char buf[PNG_BYTES_TO_CHECK];

        io_error_if(_io_dev.read(buf, PNG_BYTES_TO_CHECK) != PNG_BYTES_TO_CHECK,
                "png_check_validity: failed to read image");

        io_error_if(png_sig_cmp(png_bytep(buf), png_size_t(0), PNG_BYTES_TO_CHECK)!=0,
                "png_check_validity: invalid png image");
    }

    static void read_data( png_structp png_ptr, png_bytep data, png_size_t length)
    {
        static_cast<Device*>(png_get_io_ptr(png_ptr) )->read( data
                                                            , length );
    }

    static void write_data( png_structp png_ptr
                          , png_bytep   data
                          , png_size_t  length
                          )
    {
        static_cast<Device*>( png_get_io_ptr( png_ptr ))->write( data
                                                               , length );
    }

    static void flush(png_structp png_ptr)
    {
        static_cast<Device*>(png_get_io_ptr(png_ptr) )->flush();
    }

};

} // namespace detail
} // namespace gil
} // namespace boost

#endif // BOOST_GIL_EXTENSION_IO_DETAIL_PNG_IO_BASE_HPP_INCLUDED
