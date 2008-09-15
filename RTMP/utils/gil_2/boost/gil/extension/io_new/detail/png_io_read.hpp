/*
    Copyright 2007-2008 Christian Henning, Andreas Pokorny, Lubomir Bourdev
    Use, modification and distribution are subject to the Boost Software License,
    Version 1.0. (See accompanying file LICENSE_1_0.txt or copy at
    http://www.boost.org/LICENSE_1_0.txt).
*/

/*************************************************************************************************/

#ifndef BOOST_GIL_EXTENSION_IO_DETAIL_PNG_IO_READ_HPP_INCLUDED
#define BOOST_GIL_EXTENSION_IO_DETAIL_PNG_IO_READ_HPP_INCLUDED

////////////////////////////////////////////////////////////////////////////////////////
/// \file               
/// \brief 
/// \author Christian Henning, Andreas Pokorny, Lubomir Bourdev \n
///         
/// \date   2007-2008 \n
///
////////////////////////////////////////////////////////////////////////////////////////

#include <boost/gil/extension/io_new/png_tags.hpp>

#include <boost/gil/gil_all.hpp>

#include "base.hpp"
#include "reader_base.hpp"
#include "io_device.hpp"
#include "typedefs.hpp"
#include "png_io_base.hpp"
#include "png_supported_types.hpp"

namespace boost { namespace gil { namespace detail {

template< typename Device
        , typename ConversionPolicy
        >
class reader< Device
            , png_tag
            , ConversionPolicy
            >
    : public png_io_base< Device >
    , public reader_base< png_tag
                        , ConversionPolicy >
{
public:

    reader( Device& io_dev )
    : png_io_base< Device >( io_dev )
    , _png_ptr ( NULL )
    , _info_ptr( NULL )
    {
        this->check();
        init_reader();
    }

    reader( Device& io_dev
          , const typename ConversionPolicy::color_converter_type& cc )
    : png_io_base< Device >( io_dev )
    , reader_base< png_tag
                 , ConversionPolicy >( cc )
    , _png_ptr ( 0 )
    , _info_ptr( 0 )
    {
        this->check();
        init_reader();
    }

    ~reader()
    {
        png_destroy_read_struct( &_png_ptr
                               , &_info_ptr
                               , png_infopp_NULL
                               );
    }

    image_read_info<png_tag> get_info() const
    {
        image_read_info<png_tag> ret = {0};
        png_get_IHDR(_png_ptr, _info_ptr, 
                &ret._width, &ret._height,
                &ret._bit_depth,&ret._color_type,
                &ret._interlace_method, &ret._compression_method,
                &ret._filter_method );

        int res_unit = PNG_RESOLUTION_METER;
        png_get_pHYs(_png_ptr, _info_ptr, &ret._x_res, &ret._y_res, &res_unit);
        png_get_sBIT(_png_ptr, _info_ptr, &ret._sbits );

#ifdef PNG_FLOATING_POINT_SUPPORTED 
        png_get_gAMA(_png_ptr, _info_ptr, &ret._gamma );
#else
        png_get_gAMA_fixed(_png_ptr, _info_ptr, &ret._gamma );
#endif
        return ret;
    }

    template<typename View>
    void apply( const View& view )
    {
        apply_impl( view );
    }

private:
    template<typename View>
    void apply_impl( const View& view )
    {
        if( little_endian() )
        {   
            if( this->_info._bit_depth == 16 )
                png_set_swap(_png_ptr);
            if( this->_info._bit_depth < 8 )
                png_set_packswap(_png_ptr);
        }

        png_bitdepth::type bit_depth = this->_info._bit_depth;
        png_color_type::type color_type = this->_info._color_type;
        
        if(color_type == PNG_COLOR_TYPE_PALETTE)
        {
            color_type = PNG_COLOR_TYPE_RGB;
            bit_depth = 8;
            png_set_palette_to_rgb(_png_ptr);
        }

        if(png_get_valid(_png_ptr, _info_ptr,PNG_INFO_tRNS)) 
        {
            if( color_type == PNG_COLOR_TYPE_RGB )
                color_type = PNG_COLOR_TYPE_RGBA;
            else if( color_type== PNG_COLOR_TYPE_GRAY )
                color_type = PNG_COLOR_TYPE_GA;

            png_set_tRNS_to_alpha(_png_ptr);
        }

        switch( color_type )
        {
            case PNG_COLOR_TYPE_GRAY:
            {
                switch( bit_depth )
                {
                    case 1: read_rows< gray1_image_t::view_t::reference >( view ); break;
                    case 2: read_rows< gray2_image_t::view_t::reference >( view ); break;
                    case 4: read_rows< gray4_image_t::view_t::reference >( view ); break;
                    case 8: read_rows< gray8_pixel_t > ( view ); break;
                    case 16:read_rows< gray16_pixel_t >( view ); break;
                    default: io_error("png_reader::read_data(): unknown combination of color type and bit depth");
                }

                break;
            }
            case PNG_COLOR_TYPE_GA:
            {
                #ifdef ENABLE_GRAY_ALPHA
                switch( bit_depth )
                {
                    case 8: read_rows< gray_alpha8_pixel_t > ( view ); break;
                    case 16:read_rows< gray_alpha16_pixel_t >( view ); break;
                    default: io_error("png_reader::read_data(): unknown combination of color type and bit depth");
                }
                #else
                    io_error("gray_alpha isn't enabled. Use ENABLE_GRAY_ALPHA when building application.");
                #endif // ENABLE_GRAY_ALPHA
                

                break;
            }
            case PNG_COLOR_TYPE_RGB:
            {
                switch (bit_depth)
                {
                    case 8:  read_rows< rgb8_pixel_t > ( view ); break;
                    case 16: read_rows< rgb16_pixel_t >( view ); break;
                    default: io_error("png_reader::read_data(): unknown combination of color type and bit depth");
                }

                break;
            }
            case PNG_COLOR_TYPE_RGBA:
            {
                switch( bit_depth )
                {
                    case 8 : read_rows< rgba8_pixel_t > ( view ); break;
                    case 16: read_rows< rgba16_pixel_t >( view ); break;
                    default: io_error("png_reader_color_convert::read_data(): unknown combination of color type and bit depth");
                }

                break;
            }
            default: io_error("png_reader_color_convert::read_data(): unknown color type");
        }

        png_read_end( _png_ptr
                    , NULL
                    );
    }

    template< typename ImagePixel
            , typename View
            >
    void read_rows( const View& view )
    {
        io_error_if( ! ConversionPolicy::template is_allowed< ImagePixel
                                                            , typename View::value_type
                                                            >::type::value
                    , "User provided view has incorrect color space or channel type."
                    );

        row_buffer_helper<ImagePixel> buffer( static_cast<int>( this->_info._width ));

        // skip rows
        for( int y = 0; y < this->_top_left.y; ++y )
        {
            png_read_row( _png_ptr
                        , reinterpret_cast< png_bytep >( buffer.data() )
                        , 0
                        );
        }

        for(int y = 0; y < view.height(); ++y )
        {
            png_read_row( _png_ptr
                        , reinterpret_cast< png_bytep >( buffer.data() )
                        , 0
                        );

            this->_cc_policy.read( buffer.begin() + this->_top_left.x
                                 , buffer.begin() + this->_dim.x
                                 , view.row_begin( y )
                                 );
        }
    }

    void init_reader()
    {
        _png_ptr = png_create_read_struct(PNG_LIBPNG_VER_STRING,NULL,NULL,NULL);
        io_error_if(_png_ptr==NULL,"png_reader: fail to call png_create_write_struct()");
        // allocate/initialize the image information data
        _info_ptr = png_create_info_struct(_png_ptr);

        if (_info_ptr == NULL) {
            png_destroy_read_struct(&_png_ptr,png_infopp_NULL,png_infopp_NULL);
            io_error("png_reader: fail to call png_create_info_struct()");
        }
        if (setjmp(png_jmpbuf(_png_ptr))) {
            //free all of the memory associated with the png_ptr and info_ptr
            png_destroy_read_struct(&_png_ptr, &_info_ptr, png_infopp_NULL);
            io_error("png_reader: fail to call setjmp()");
        }

        init_io( _png_ptr );

        png_set_sig_bytes(_png_ptr,PNG_BYTES_TO_CHECK);
        png_read_info(_png_ptr, _info_ptr);
    }

    void init_io( png_structp png_ptr )
    {
        png_set_read_fn(png_ptr, 
                static_cast<void*>(&this->_io_dev), 
                static_cast<void(*)(png_structp, png_bytep, png_size_t)>(&png_io_base<Device>::read_data));
    }

    png_structp _png_ptr;
    png_infop _info_ptr;
};


} // namespace detail
} // namespace gil
} // namespace boost



#endif // BOOST_GIL_EXTENSION_IO_DETAIL_PNG_IO_READ_HPP_INCLUDED
