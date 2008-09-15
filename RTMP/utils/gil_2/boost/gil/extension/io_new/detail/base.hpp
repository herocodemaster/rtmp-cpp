/*
    Copyright 2007-2008 Christian Henning, Andreas Pokorny, Lubomir Bourdev
    Use, modification and distribution are subject to the Boost Software License,
    Version 1.0. (See accompanying file LICENSE_1_0.txt or copy at
    http://www.boost.org/LICENSE_1_0.txt).
*/

/*************************************************************************************************/

#ifndef BOOST_GIL_EXTENSION_IO_DETAIL_BASE_HPP_INCLUDED
#define BOOST_GIL_EXTENSION_IO_DETAIL_BASE_HPP_INCLUDED

////////////////////////////////////////////////////////////////////////////////////////
/// \file               
/// \brief 
/// \author Christian Henning, Andreas Pokorny, Lubomir Bourdev \n
///         
/// \date   2007-2008 \n
///
////////////////////////////////////////////////////////////////////////////////////////

#include <ostream>
#include <istream>
#include <vector>

#include <boost/array.hpp>
#include <boost/bind.hpp>
#include <boost/type_traits/is_base_of.hpp>

#include <boost/gil/utilities.hpp>
#include <boost/gil/color_convert.hpp>
#include <boost/gil/bit_aligned_pixel_reference.hpp>
#include <boost/gil/bit_aligned_pixel_iterator.hpp>

#include "typedefs.hpp"

namespace boost { namespace gil {

typedef boost::gil::point2< std::ptrdiff_t > point_t;

struct format_tag {};

/**
 * Boolean meta function, mpl::true_ if the pixel type \a PixelType is supported 
 * by the image format identified with \a FormatTag.
 * \todo the name is_supported is to generic, pick something more IO realted.
 */
template<typename PixelType, typename FormatTag> struct is_supported;

template<typename FormatTag> struct is_format_tag : is_base_and_derived<format_tag,FormatTag> {};
template<typename FormatTag> struct image_read_info;
template<typename FormatTag> struct image_write_info;


namespace detail {

// - performance specialization double
// - to eliminate compiler warning 4244
template <typename GrayChannelValue>
struct rgb_to_luminance_fn< double, double, double, GrayChannelValue > {
    GrayChannelValue operator()( const double& red
                               , const double& green
                               , const double& blue    ) const
   {
      return channel_convert<GrayChannelValue>( red * 0.30 + green * 0.59 + blue * 0.11 );
   }
};

/*
template < typename Channel >
struct bits_per_sample : public mpl::int_< sizeof( Channel )* 8 / byte_to_memunit< Channel >::value > {};
*/

inline 
void io_error( const std::string& descr )
{
   throw std::ios_base::failure( descr );
}

inline 
void io_error_if( bool expr, const std::string& descr )
{
   if( expr ) 
      io_error( descr );
}

template< typename PixelRefT>
struct is_bit_aligned : mpl::false_{};

template <typename B, typename C, typename L, bool M>  
struct is_bit_aligned<bit_aligned_pixel_reference<B,C,L,M> > : mpl::true_{};

template <typename B, typename C, typename L, bool M>  
struct is_bit_aligned<const bit_aligned_pixel_reference<B,C,L,M> > : mpl::true_{};

template <typename B, typename C, typename L>  
struct is_bit_aligned<packed_pixel<B,C,L> > : mpl::true_{};

template <typename B, typename C, typename L>  
struct is_bit_aligned<const packed_pixel<B,C,L> > : mpl::true_{};


inline
unsigned char swap_bits( unsigned char c )
{
   unsigned char result = 0;
   for( int i = 0; i < 8; ++i )
   {
      result = result << 1;
      result |= ( c & 1 );
      c = c >> 1;
   }

   return result;
}


template <typename B, typename C, typename L, bool M>  
struct gen_chan_ref
{
	typedef packed_dynamic_channel_reference<B,mpl::at_c<C,0>::type::value,M> type;
};

template<typename C,typename CMP, int Next, int Last>
struct is_homogeneous_impl;


template<typename C,typename CMP, int Last>
struct is_homogeneous_impl<C,CMP,Last,Last> : mpl::true_{};


template<typename C,typename CMP, int Next, int Last>
struct is_homogeneous_impl
	: mpl::and_<
		is_homogeneous_impl<C,CMP,Next+1,Last>,
		is_same<CMP,typename mpl::at_c<C,Next>::type> 
		> {};

} // namespace boost::gil::detail

template <typename P>
struct is_homogeneous;

template <typename B, typename C, typename L, bool M>  
struct is_homogeneous<bit_aligned_pixel_reference<B,C,L,M> > 
	: detail::is_homogeneous_impl<C,typename mpl::at_c<C,0>::type,1,mpl::size<C>::type::value>
{};

template <typename B, typename C, typename L, bool M>  
struct is_homogeneous<const bit_aligned_pixel_reference<B,C,L,M> > 
	: detail::is_homogeneous_impl<C,typename mpl::at_c<C,0>::type,1,mpl::size<C>::type::value>
{};


namespace detail
{

template<typename A, typename B>
struct is_similar
    : mpl::false_
{};

template<typename A>
struct is_similar<A,A>
    : mpl::true_
{};

template<typename B,int I, int S, bool M, int I2>
struct is_similar<packed_channel_reference<B,I,S,M>, packed_channel_reference<B,I2,S,M> >
    : mpl::true_
{};

template<typename C,typename CMP, int I,int Last>
struct is_homogeneous_impl_p;

template<typename C,typename CMP, int Last>
struct is_homogeneous_impl_p<C,CMP,Last,Last> : mpl::true_{};

template<typename C,typename CMP, int Next, int Last>
struct is_homogeneous_impl_p
	: mpl::and_<
		is_homogeneous_impl_p<C,CMP,Next+1,Last>,
		is_similar<CMP,typename mpl::at_c<C,Next>::type> 
		> {};

template <typename T>
struct get_num_bits;
template<typename B,int I, int S, bool M>
struct get_num_bits<packed_channel_reference<B,I,S,M> >
{
    BOOST_STATIC_CONSTANT(int,value=S);
};
template<typename B,int I, int S, bool M>
struct get_num_bits<const packed_channel_reference<B,I,S,M> >
{
    BOOST_STATIC_CONSTANT(int,value=S);
};

template <typename B, typename C, typename L>  
struct gen_chan_ref_p
{
	typedef packed_dynamic_channel_reference<
        B,
        get_num_bits<typename mpl::at_c<C,0>::type>::value,
        true> type;
};

}

// for packed_pixel
template <typename B, typename C, typename L>  
struct is_homogeneous<packed_pixel<B,C,L> > 
	: detail::is_homogeneous_impl_p<C,typename mpl::at_c<C,0>::type,1,mpl::size<C>::type::value>
{};

template <typename B, typename C, typename L>  
struct is_homogeneous<const packed_pixel<B,C,L> > 
	: detail::is_homogeneous_impl_p<C,typename mpl::at_c<C,0>::type,1,mpl::size<C>::type::value>
{};

// pixel
template < typename C, typename L > struct is_homogeneous< pixel<C,L> > : mpl::true_ {};
template < typename C, typename L > struct is_homogeneous<const pixel<C,L> > : mpl::true_ {};
template < typename C, typename L > struct is_homogeneous< pixel<C,L>& > : mpl::true_ {};
template < typename C, typename L > struct is_homogeneous<const pixel<C,L>& > : mpl::true_ {};

// planar pixel reference
template <typename Channel, typename ColorSpace>
struct is_homogeneous< planar_pixel_reference< Channel, ColorSpace > > : mpl::true_ {};
template <typename Channel, typename ColorSpace>
struct is_homogeneous< const planar_pixel_reference< Channel, ColorSpace > > : mpl::true_ {};



//! This implementation works for bit_algined_pixel_reference 
//! with a homegeneous channel layout. 
//! The result type will be a packed_dynamic_channel_reference, since the 
//! offset info will be missing. 
template <typename B, typename C, typename L, bool M>  
struct channel_type<bit_aligned_pixel_reference<B,C,L,M> > 
	: boost::lazy_enable_if< 
		is_homogeneous<bit_aligned_pixel_reference<B,C,L,M> >,
		detail::gen_chan_ref<B,C,L,M>
		>
{};

template <typename B, typename C, typename L, bool M>  
struct channel_type<const bit_aligned_pixel_reference<B,C,L,M> > 
	: boost::lazy_enable_if< 
		is_homogeneous<bit_aligned_pixel_reference<B,C,L,M> >,
		detail::gen_chan_ref<B,C,L,M>
		>
{};

template <typename B, typename C, typename L>  
struct channel_type<packed_pixel<B,C,L> > 
	: boost::lazy_enable_if< 
		is_homogeneous<packed_pixel<B,C,L> >,
		detail::gen_chan_ref_p<B,C,L>
		>
{};

template <typename B, typename C, typename L>  
struct channel_type<const packed_pixel<B,C,L> > 
	: boost::lazy_enable_if< 
		is_homogeneous<packed_pixel<B,C,L> >,
		detail::gen_chan_ref_p<B,C,L>
		>
{};

namespace detail{

template<typename PixelT,typename DummyT = void >
struct row_buffer_helper
{
    typedef PixelT element_t;
    typedef std::vector< element_t > buffer_t;
    typedef typename buffer_t::iterator iterator_t;

    buffer_t row_buffer;
    row_buffer_helper( int width )
        : row_buffer(width) 
    {
    }

    element_t* data()
    {
        return &row_buffer[0];
    }

    iterator_t begin()
    {
        return row_buffer.begin();
    }

    iterator_t end()
    {
        return row_buffer.end();
    }
};

template<typename PixelT>
struct row_buffer_helper<PixelT, typename enable_if<
    typename mpl::and_<
        typename is_bit_aligned<PixelT>::type,
        typename is_homogeneous<PixelT>::type
        >::type
    >::type>
{
    typedef unsigned char element_t;
    typedef std::vector< element_t > buffer_t;
    typedef PixelT pixel_type;
    typedef bit_aligned_pixel_iterator<pixel_type> iterator_t;


    int c, r;
    buffer_t row_buffer;
    row_buffer_helper( int width )
        : c(
                (width * 
                num_channels<pixel_type>::type::value *
                channel_type<pixel_type>::type::num_bits) >>
                3 
                ),
        r( 
                width * 
                num_channels<pixel_type>::type::value *
                channel_type<pixel_type>::type::num_bits  -
                ( c << 3 )
         ), 
        row_buffer(c + (r!=0))
    {
    }

    iterator_t begin()
    {
        return iterator_t(&row_buffer.front(),0);
    }

    iterator_t end()
    {
        return r==0 
            ? iterator_t(&row_buffer.back() + 1,0)
            : iterator_t(&row_buffer.back(), r);
    }

    element_t* data()
    {
        return &row_buffer[0];
    }
};

template<typename View,typename D = void>
struct row_buffer_helper_view
    : row_buffer_helper<typename View::value_type>
{
    row_buffer_helper_view( int width ) 
      :  row_buffer_helper<typename View::value_type>(width)
    {}
};


template<typename View>
struct row_buffer_helper_view<View,
    typename enable_if<typename is_bit_aligned<typename View::value_type>::type>::type
    >
    : row_buffer_helper<typename View::reference>
{
    row_buffer_helper_view( int width ) 
        : row_buffer_helper<typename View::reference>(width)
    {}
};


template< typename IsBitAligned
        , typename Buffer
        >
struct swap_bits_fn
{
   template< typename Device >
   swap_bits_fn( const Device& dev ) {}

   void operator() ( Buffer& ) {}
};

template<>
struct swap_bits_fn< boost::mpl::true_
                   , std::vector< unsigned char > >
{
   template< typename Device >
   swap_bits_fn( const Device& dev )
   {
      for( int i = 0; i < 256; ++i )
      {
         _lookup[i] = swap_bits( i );
      }

      _swap_bits = ( dev.are_bytes_swapped() > 0 ) ? true : false;
   }

   void operator() ( std::vector< unsigned char >& buffer )
   {
      typedef swap_bits_fn< boost::mpl::true_, std::vector< unsigned char > > tt;

      if( _swap_bits )
      {
         std::transform( buffer.begin()
                       , buffer.end()
                       , buffer.begin()
                       , boost::bind( &tt::_swap, *this, _1 ));
      }
   }
 
 private:
 
   unsigned char _swap( unsigned char byte ) const
   {
      return _lookup[ byte ];
   }
 
 private:
 
   boost::array< unsigned char, 256 > _lookup;
   bool _swap_bits;
};


template< typename Channel >
int format_value( boost::mpl::true_ ) // is_bit_aligned
{
    return SAMPLEFORMAT_UINT;
}

template< typename Channel >
int format_value( boost::mpl::false_ ) // is_bit_aligned
{
    if( is_unsigned< Channel >::value )
    {
        return SAMPLEFORMAT_UINT;
    }
    if( is_signed< Channel >::value )
    {
        return SAMPLEFORMAT_INT;
    }
    else if( is_floating_point< Channel >::value )
    {
        return SAMPLEFORMAT_IEEEFP;
    }

    throw std::runtime_error( "Unkown channel format." );

    return 0;
}


} // namespace detail
} // namespace gil
} // namespace boost

#endif // BOOST_GIL_EXTENSION_IO_DETAIL_BASE_HPP_INCLUDED
