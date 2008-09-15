/*
    Copyright 2007-2008 Christian Henning, Andreas Pokorny, Lubomir Bourdev
    Use, modification and distribution are subject to the Boost Software License,
    Version 1.0. (See accompanying file LICENSE_1_0.txt or copy at
    http://www.boost.org/LICENSE_1_0.txt).
*/

/*************************************************************************************************/

#ifndef BOOST_GIL_EXTENSION_IO_JPEG_IO_WRITE_HPP_INCLUDED
#define BOOST_GIL_EXTENSION_IO_JPEG_IO_WRITE_HPP_INCLUDED

////////////////////////////////////////////////////////////////////////////////////////
/// \file               
/// \brief 
/// \author Christian Henning, Andreas Pokorny, Lubomir Bourdev \n
///         
/// \date   2007-2008 \n
///
////////////////////////////////////////////////////////////////////////////////////////

#include <vector>
#include <boost/gil/extension/io_new/jpeg_tags.hpp>
#include <boost/gil/extension/io_new/detail/jpeg_supported_types.hpp>

namespace boost { namespace gil { namespace detail {

template< typename Device >
class writer< Device
            , jpeg_tag
            > 
{
public:

    writer( Device & file )
        : out(file)
    {
        _dest._jdest.free_in_buffer = sizeof(buffer);
        _dest._jdest.next_output_byte = buffer;
        _dest._jdest.init_destination = reinterpret_cast<void(*)(j_compress_ptr)>(&writer<Device,jpeg_tag>::init_device);
        _dest._jdest.empty_output_buffer = reinterpret_cast<boolean(*)(j_compress_ptr)>(&writer<Device,jpeg_tag>::empty_buffer);
        _dest._jdest.term_destination = reinterpret_cast<void(*)(j_compress_ptr)>(&writer<Device,jpeg_tag>::close_device);
        _dest._this = this;

        _cinfo.err = jpeg_std_error( &_jerr );

        jpeg_create_compress( &_cinfo  );
        _cinfo.dest = &_dest._jdest;

    }

    ~writer()
    {
        jpeg_finish_compress ( &_cinfo );
        jpeg_destroy_compress( &_cinfo );
    }

    jpeg_compress_struct& get()
    {
        return _cinfo;
    }


    template<typename View>
    void apply( const View& view )
    {
        _cinfo.image_width  = JDIMENSION(view.width());
        _cinfo.image_height = JDIMENSION(view.height());
        _cinfo.input_components = num_channels<View>::value;
        _cinfo.in_color_space = detail::jpeg_rw_support<typename channel_type<View>::type,
            typename color_space_type<View>::type>::color_type;
        jpeg_set_defaults(&_cinfo);
        jpeg_set_quality(&_cinfo, 100, TRUE);

        write_rows( view );
    }

    template<typename View>
    void apply( const View&                       view
              , const image_write_info<jpeg_tag>& info )
    {
        _cinfo.image_width  = JDIMENSION(view.width());
        _cinfo.image_height = JDIMENSION(view.height());
        _cinfo.input_components = num_channels<View>::value;
        _cinfo.in_color_space = detail::jpeg_rw_support< typename channel_type<View>::type
                                                       , typename color_space_type<View>::type
                                                       >::color_type;

        jpeg_set_defaults( &_cinfo);
        jpeg_set_quality ( &_cinfo
                         , info._quality
                         , TRUE
                         );

        write_rows( view );
    }

private:

    template<typename View>
    void write_rows( const View& view )
    {
        jpeg_start_compress(&_cinfo, TRUE);

        std::vector<typename View::value_type> row_buffer( view.width() );
        JSAMPLE* row_addr = reinterpret_cast<JSAMPLE*>( &row_buffer[0] );

        for( int y =0; y != view.height(); ++ y )
        {
            std::copy( view.row_begin( y )
                     , view.row_end  ( y )
                     , row_buffer.begin()
                     );

            jpeg_write_scanlines( &this->_cinfo
                                , &row_addr
                                , 1
                                );
        }
    }

    struct gil_jpeg_destination_mgr
    {
        jpeg_destination_mgr _jdest;
        writer<Device,jpeg_tag> * _this;
    };

    static void init_device( jpeg_compress_struct* cinfo )
    {
        gil_jpeg_destination_mgr* dest = reinterpret_cast< gil_jpeg_destination_mgr* >( cinfo->dest );

        dest->_jdest.free_in_buffer   = sizeof( dest->_this->buffer );
        dest->_jdest.next_output_byte = dest->_this->buffer;
    }

    static boolean empty_buffer( jpeg_compress_struct* cinfo )
    {
        gil_jpeg_destination_mgr* dest = reinterpret_cast< gil_jpeg_destination_mgr* >( cinfo->dest );

        dest->_this->out.write( dest->_this->buffer
                              , buffer_size 
                              );

        writer<Device,jpeg_tag>::init_device( cinfo );
        return 1;
    }

    static void close_device( jpeg_compress_struct* cinfo )
    {
        writer<Device,jpeg_tag>::empty_buffer( cinfo );

        gil_jpeg_destination_mgr* dest = reinterpret_cast< gil_jpeg_destination_mgr* >( cinfo->dest );

        dest->_this->out.flush();
    }

    Device &out;

    jpeg_compress_struct _cinfo;
    jpeg_error_mgr       _jerr;

    gil_jpeg_destination_mgr _dest;

    static const unsigned int buffer_size = 1024;
    JOCTET buffer[buffer_size];
};

} // detail
} // gil
} // boost

#endif // BOOST_GIL_EXTENSION_IO_JPEG_IO_WRITE_HPP_INCLUDED