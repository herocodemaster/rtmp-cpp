/*
    Copyright 2007-2008 Andreas Pokorny, Christian Henning
    Use, modification and distribution are subject to the Boost Software License,
    Version 1.0. (See accompanying file LICENSE_1_0.txt or copy at
    http://www.boost.org/LICENSE_1_0.txt).
*/

/*************************************************************************************************/

#ifndef BOOST_GIL_EXTENSION_IO_DETAIL_CONVERSION_POLICIES_HPP_INCLUDED
#define BOOST_GIL_EXTENSION_IO_DETAIL_CONVERSION_POLICIES_HPP_INCLUDED

////////////////////////////////////////////////////////////////////////////////////////
/// \file               
/// \brief 
/// \author Andreas Pokorny, Christian Henning \n
///         
/// \date   2007-2008 \n
///
////////////////////////////////////////////////////////////////////////////////////////

#include <algorithm>
#include <iterator>
#include <boost/gil/image_view_factory.hpp>

namespace boost{namespace gil{ namespace detail {


// The following two functions look the same but are different since one is using
// a pixel_t as template parameter whereas the other is using reference_t.
template< typename View >
bool compare_channel_sizes( const std::vector< unsigned int >& channel_sizes // in bits
                          , mpl::false_                                      // is_bit_aligned
                          , mpl::true_                                       // is_homogeneous
                          ) 
{
    typedef typename View::value_type pixel_t;

    typedef channel_traits< element_type< pixel_t >::type >::value_type channel_t;
    unsigned int s = boost::gil::detail::unsigned_integral_num_bits< channel_t >::value;

    return ( s == channel_sizes[0] );
}


template< typename View >
bool compare_channel_sizes( const std::vector< unsigned int >& channel_sizes // in bits
                          , mpl::true_                                       // is_bit_aligned
                          , mpl::true_                                       // is_homogeneous
                          ) 
{
    typedef typename View::reference ref_t;

    typedef channel_traits< element_type< ref_t >::type >::value_type channel_t;
    channel_t c;

    unsigned int s = boost::gil::detail::unsigned_integral_num_bits< channel_t >::value;

    return ( s == channel_sizes[0] );
}

struct compare_channel_sizes_fn
{
    compare_channel_sizes_fn( unsigned int* a )
    : _a( a )
    , _b( true )
    {}

    template< typename ChannelSize >
    void operator()( ChannelSize x)
    {
        if( x != *_a++ )
        {
            _b = false;
        }
    }

    unsigned int* _a;
    bool _b;
};

template< typename View >
bool compare_channel_sizes( std::vector< unsigned int >& channel_sizes // in bits
                          , mpl::true_                                 // is_bit_aligned
                          , mpl::false_                                // is_homogeneous
                          )
{
    // loop through all channels and compare

    typedef typename View::reference ref_t;
    typedef channel_sizes_type< ref_t >::type cs_t;

    compare_channel_sizes_fn fn( &channel_sizes.front() );
    mpl::for_each< cs_t >( fn );

    return fn._b;
}

struct read_and_no_convert 
{
public:
    typedef void* color_converter_type;

    template< typename View >
    bool is_allowed( unsigned int                       src_n // num channels
                   , const std::vector< unsigned int >& src_s // array of channel sizes
                   , unsigned int                       src_f // channel format
                   )
    {
        typedef typename View::value_type pixel_t;
        typedef typename View::reference  ref_t;
        typedef channel_traits< element_type< pixel_t >::type >::value_type channel_t;

        int dst_n = num_channels< pixel_t >::value;
        int dst_f = format_value< channel_t >( is_bit_aligned< pixel_t >::type() );

        bool s = compare_channel_sizes< View >( src_s
                                              , is_bit_aligned< ref_t >::type()
                                              , is_homogeneous< ref_t >::type() );

        return (  dst_n == src_n
               && s
               && dst_f == src_f
               );
    }

    template< typename InIterator
            , typename OutIterator
            >
    void read( const InIterator& begin
             , const InIterator& end
             , OutIterator       out
             , typename disable_if< typename pixels_are_compatible< typename std::iterator_traits<InIterator>::value_type
                                                                  , typename std::iterator_traits<OutIterator>::value_type
                                                                  >::type 
                                  >::type* ptr = 0
             )
    {}

    template< typename InIterator
            , typename OutIterator
            >
    void read( const InIterator& begin
             , const InIterator& end
             , OutIterator       out
             , typename enable_if< typename pixels_are_compatible< typename std::iterator_traits<InIterator>::value_type
                                                                 , typename std::iterator_traits<OutIterator>::value_type
                                                                 >::type 
                                 >::type* ptr = 0
             )
    {
        std::copy( begin, end, out );
    }

};

template<typename CC>
struct read_and_convert
{
public:
    typedef CC color_converter_type;
    CC _cc;

    template< typename View >
    bool is_allowed( unsigned int                       src_n // num channels
                   , const std::vector< unsigned int >& src_s // array of channel sizes
                   , unsigned int                       src_f // channel format
                   )
    {
        return true;
    }

    read_and_convert( color_converter_type const& cc )
        : _cc(cc) {}

    template< typename InIterator
            , typename OutIterator
            >
    void read( const InIterator& begin
             , const InIterator& end
             , OutIterator       out
             )
    {
        typedef color_convert_deref_fn< typename std::iterator_traits<InIterator>::reference
                                      , typename std::iterator_traits<OutIterator>::value_type //reference?
                                      , CC
                                      > deref_t;

        std::transform( begin
                      , end
                      , out
                      , deref_t( _cc )
                      );

    }
};

} // namespace detail
} // namespace gil
} // namespace boost

#endif // BOOST_GIL_EXTENSION_IO_DETAIL_CONVERSION_POLICIES_HPP_INCLUDED
