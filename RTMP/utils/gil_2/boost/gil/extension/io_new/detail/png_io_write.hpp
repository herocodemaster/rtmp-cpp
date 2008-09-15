/*
    Copyright 2007-2008 Christian Henning, Andreas Pokorny, Lubomir Bourdev
    Use, modification and distribution are subject to the Boost Software License,
    Version 1.0. (See accompanying file LICENSE_1_0.txt or copy at
    http://www.boost.org/LICENSE_1_0.txt).
*/

/*************************************************************************************************/

#ifndef BOOST_GIL_EXTENSION_IO_DETAIL_PNG_IO_WRITE_HPP_INCLUDED
#define BOOST_GIL_EXTENSION_IO_DETAIL_PNG_IO_WRITE_HPP_INCLUDED

////////////////////////////////////////////////////////////////////////////////////////
/// \file               
/// \brief 
/// \author Christian Henning, Andreas Pokorny, Lubomir Bourdev \n
///         
/// \date   2007-2008 \n
///
////////////////////////////////////////////////////////////////////////////////////////

extern "C" {
#include <png.h>
}

#include "typedefs.hpp"
#include "base.hpp"
#include "png_io_base.hpp"

namespace boost { namespace gil { namespace detail {

template<typename Device>
class writer<Device,png_tag> : png_io_base<Device>
{
public:

    writer( Device & io_dev )
        : png_io_base<Device>(io_dev), _png_ptr(0), _info_ptr(0)
    {
        _png_ptr = png_create_write_struct(PNG_LIBPNG_VER_STRING,NULL,NULL,NULL);
        io_error_if(_png_ptr==NULL,"png_writer: fail to call png_create_write_struct()");
        // allocate/initialize the image information data
        _info_ptr = png_create_info_struct(_png_ptr);

        if (_info_ptr == NULL) {
            png_destroy_write_struct(&_png_ptr,png_infopp_NULL);
            io_error("png_writer: fail to call png_create_info_struct()");
        }
        if (setjmp(png_jmpbuf(_png_ptr))) {
            //free all of the memory associated with the png_ptr and info_ptr
            png_destroy_write_struct(&_png_ptr, &_info_ptr);
            io_error("png_writer: fail to call setjmp()");
        }

        this->init_io( _png_ptr );
    }
    ~writer()
    {
        png_destroy_write_struct(&_png_ptr,&_info_ptr);
    }


    template <typename View>
    void apply( const View&                      view
              , const image_write_info<png_tag>& info )
    {
        typedef png_rw_support< typename channel_type<View>::type
                              , typename color_space_type<View>::type
                              > png_rw_info;

        png_set_IHDR( _png_ptr
                    , _info_ptr
                    , view.width()
                    , view.height()
                    , png_rw_info::bit_depth
                    , png_rw_info::color_type
                    , info._interlace_method
                    , info._compression_method
                    , info._filter_method 
                    );

#ifdef PNG_FLOATING_POINT_SUPPORTED 
        png_set_gAMA( _png_ptr
                    , _info_ptr
                    , info._gamma
                    );
#else
        png_set_gAMA_fixed( _png_ptr
                          , _info_ptr
                          , info._gamma
                          );
#endif

        png_set_pHYs( _png_ptr
                    , _info_ptr
                    , &info._x_res
                    , &info._y_res
                    , PNG_RESOLUTION_METER
                    );

        png_set_sBIT( _png_ptr
                    , _info_ptr
                    , const_cast< png_color_8* >( &info._sbits )
                    );

        png_write_info( _png_ptr
                      ,_info_ptr
                      );

        write_view( view
                  , is_bit_aligned< View >::type );
    }

    template< typename View >
    void apply( const View& view )
    {
        typedef png_rw_support< typename kth_semantic_element_type< typename View::value_type
                                                                  , 0
                                                                  >::type
                              , typename color_space_type<View>::type
                              > png_rw_info;

        png_set_IHDR( _png_ptr
                    , _info_ptr
                    , static_cast<int>( view.width()  )
                    , static_cast<int>( view.height() )
                    , png_rw_info::bit_depth
                    , png_rw_info::color_type
                    , PNG_INTERLACE_NONE
                    , PNG_COMPRESSION_TYPE_DEFAULT
                    , PNG_FILTER_TYPE_DEFAULT
                    );

        png_write_info(_png_ptr,_info_ptr);
        write_view( view
                  , is_bit_aligned< View >::type() );
    }

private:
    template<typename View>
    void write_view( const View& view
                   ,  boost::mpl::false_ // is bit aligned
                   )
    {
        typedef png_rw_support<
                        typename channel_type<View>::type,
                        typename color_space_type<View>::type
                        > png_rw_info;

        if (little_endian() )
        {   
            if( png_rw_info::bit_depth == 16 )
                png_set_swap(_png_ptr);
            if( png_rw_info::bit_depth < 8 )
                png_set_packswap(_png_ptr);
        }

        row_buffer_helper_view<View> row_buffer( view.width() );

        for( int y = 0; y != view.height(); ++ y) 
        {
            std::copy( view.row_begin( y )
                     , view.row_end  ( y )
                     , row_buffer.begin()
                     );

            png_write_row( _png_ptr
                         , reinterpret_cast< png_bytep >( row_buffer.data() )
                         );
        }
        
        png_write_end( _png_ptr
                     , _info_ptr
                     );
    }

    template<typename View>
    void write_view( const View& view
                   , boost::mpl::true_ // is bit aligned
                   ) 
    {
        typedef png_rw_support< typename kth_semantic_element_type< typename View::value_type
                                                                 , 0>::type,
                        typename color_space_type<View>::type
                        > png_rw_info;

        if (little_endian() )
        {   
            if( png_rw_info::bit_depth == 16 )
                png_set_swap(_png_ptr);
            if( png_rw_info::bit_depth < 8 )
                png_set_packswap(_png_ptr);
        }

        row_buffer_helper_view< View > row_buffer( view.width() );

        for( int y = 0; y != view.height(); ++ y) 
        {
            std::copy( view.row_begin( y )
                     , view.row_end  ( y )
                     , row_buffer.begin()
                     );

            png_write_row( _png_ptr
                         , reinterpret_cast< png_bytep >( row_buffer.data() )
                         );
        }
        
        png_write_end(_png_ptr,_info_ptr);
    }

    void init_io( png_structp png_ptr )
    {
        png_set_write_fn(png_ptr, 
                static_cast<void*>(&this->_io_dev), 
                static_cast<png_rw_ptr>(&png_io_base<Device>::write_data),
                static_cast<png_flush_ptr>(&png_io_base<Device>::flush)
                );
    }

    png_structp _png_ptr;
    png_infop _info_ptr;
};


} // namespace detail
} // namespace gil
} // namespace boost



#endif // BOOST_GIL_EXTENSION_IO_DETAIL_PNG_IO_WRITE_HPP_INCLUDED
