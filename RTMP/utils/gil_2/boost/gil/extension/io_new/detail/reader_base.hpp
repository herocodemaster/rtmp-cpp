/*
    Copyright 2007-2008 Christian Henning
    Use, modification and distribution are subject to the Boost Software License,
    Version 1.0. (See accompanying file LICENSE_1_0.txt or copy at
    http://www.boost.org/LICENSE_1_0.txt).
*/

/*************************************************************************************************/

#ifndef BOOST_GIL_EXTENSION_IO_DETAIL_READER_BASE_HPP_INCLUDED
#define BOOST_GIL_EXTENSION_IO_DETAIL_READER_BASE_HPP_INCLUDED

////////////////////////////////////////////////////////////////////////////////////////
/// \file               
/// \brief 
/// \author Christian Henning \n
///         
/// \date   2007-2008 \n
///
////////////////////////////////////////////////////////////////////////////////////////

#include "base.hpp"

namespace boost { namespace gil { namespace detail {

template< typename FormatTag
        , typename ConversionPolicy
        >
struct reader_base
{
public:

    template< typename Image >
    void init_image( Image&                              img
                   , const point_t&                      top_left
                   , const point_t&                      dim 
                   , const image_read_info< FormatTag >& info )
    {
        _info = info;

        setup( _top_left, dim );

        img.recreate( _dim.x - _top_left.x
                    , _dim.y - _top_left.y );
    }

    template< typename View >
    void init_view( const View&                         view
                  , const point_t&                      top_left
                  , const image_read_info< FormatTag >& info )
    {
        _info = info;

        setup( _top_left
             , view.dimensions() );
    }

protected:

    reader_base() {}
    reader_base( const typename ConversionPolicy::color_converter_type& cc )
    : _cc_policy( cc )
    {}

private:

    void setup( const point_t& top_left
              , const point_t& dim
              )
    {
        check_coordinates( top_left
                         , dim
                         , _info
                         );

        _top_left = top_left;

        if( dim == point_t( 0, 0 ))
        {
            _dim.x = _info._width;
            _dim.y = _info._height;
        }
        else
        {
            _dim = dim;
        }
    }

    template< typename Info >
    void check_coordinates( const point_t& top_left
                          , const point_t& dim
                          , const Info&    image_info )
    {
       typedef point_t::value_type int_t;

       int_t width  = static_cast<int_t>( image_info._width  );
       int_t height = static_cast<int_t>( image_info._height );

       io_error_if( (  ( width  ) <  top_left.x
                    && ( width  ) <= dim.x
                    && ( height ) <  top_left.y
                    && ( height ) <= dim.y  )
                 , "User provided view has incorrect size."       );

       io_error_if( (  top_left.x > dim.x
                    && top_left.y > dim.y
                    )
                 , "User provided view has incorrect size." );

       io_error_if( (  ( dim.x - top_left.x ) > width
                    || ( dim.y - top_left.y ) > height
                   )
                 , "User provided view has incorrect size." );
    }

protected:

    point_t _top_left;
    point_t _dim;

    image_read_info< FormatTag > _info;

    ConversionPolicy _cc_policy;
};

} // namespace detail
} // namespace gil
} // namespace boost

#endif // BOOST_GIL_EXTENSION_IO_DETAIL_READER_BASE_HPP_INCLUDED
