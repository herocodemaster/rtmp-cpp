/*
    Copyright 2007-2008 Andreas Pokorny, Christian Henning
    Use, modification and distribution are subject to the Boost Software License,
    Version 1.0. (See accompanying file LICENSE_1_0.txt or copy at
    http://www.boost.org/LICENSE_1_0.txt).
*/

/*************************************************************************************************/

#ifndef BOOST_GIL_EXTENSION_IO_DETAIL_IO_DEVICE_HPP_INCLUDED
#define BOOST_GIL_EXTENSION_IO_DETAIL_IO_DEVICE_HPP_INCLUDED

////////////////////////////////////////////////////////////////////////////////////////
/// \file               
/// \brief 
/// \author Andreas Pokorny, Christian Henning \n
///         
/// \date   2007-2008 \n
///
////////////////////////////////////////////////////////////////////////////////////////

#include <cassert>

#include <boost/shared_ptr.hpp>

#include <boost/utility/enable_if.hpp>
#include <boost/gil/extension/io_new/detail/base.hpp>

namespace boost { namespace gil { namespace detail {
/*!
 * Implements the IODevice concept c.f. to \ref IODevice required by Image libraries like
 * libjpeg and libpng. 
 *
 * \todo switch to a sane interface as soon as there is 
 * something good in boost. I.E. the IOChains library
 * would fit very well here. 
 *
 * This implementation is based on FILE*.
 */
template< typename FormatTag >
class file_stream_device
{
public:

   typedef FormatTag _tag_t;

public:
    struct read_tag {};
    struct write_tag {};
    file_stream_device( std::string const& file_name, read_tag )
        : file(0),_close(true)
    {
        io_error_if( ( file = fopen( file_name.c_str(), "rb" )) == NULL
                , "file_stream_device: failed to open file" );
    }

    file_stream_device( std::string const& file_name, write_tag )
        : file(0),_close(true)
    {
        io_error_if( ( file = fopen( file_name.c_str(), "wb" )) == NULL
                , "file_stream_device: failed to open file" );
    }

    file_stream_device( FILE * filep)
        : file(filep),_close(false)
    {
    }

    ~file_stream_device()
    {
        if(_close)
            fclose(file);
    }

    std::size_t read( unsigned char* data
                    , std::size_t    count )
    {
        return fread( data, 1, static_cast<int>( count ), file );
    }

    void write( const unsigned char* data
              , std::size_t          count )
    {
        fwrite(data, 1, static_cast<int>( count ), file);
    }

    //!\todo replace with std::ios::seekdir?
    void seek( long count, int whence )
    {
        fseek(file, count, whence );
    }


    void flush()
    {
        fflush(file);
    }
private:
    file_stream_device( file_stream_device const& );
    file_stream_device& operator=( file_stream_device const& );
    FILE* file;
    bool _close;
};


/**
 * Input stream device
 */
class istream_device
{
public:
   istream_device( std::istream& in )
   : _in( in ) {}

    std::size_t read( unsigned char* data
                    , std::size_t    count )
    {
        std::streamsize cr = 0;

        do
        {
            _in.peek();
            std::streamsize c = _in.readsome( reinterpret_cast<char*>( data )
                                            , static_cast<std::streamsize>( count ));

            count -= c;
            data += c;
            cr += c;

        } while( count && _in );

        return static_cast< std::size_t >( cr );
    }

    void seek( long count, int whence )
    {
        _in.seekg( count
                 , whence == SEEK_SET ? std::ios::beg
                                      :( whence == SEEK_CUR ? std::ios::cur
                                                            : std::ios::end )
                 );
    }

    void write( const unsigned char* data
              , std::size_t          count )
    {
        throw std::runtime_error( "Bad io error." );
    }

    void flush() {}

private:

    std::istream& _in;
};

/**
 * Output stream device
 */
class ostream_device
{
public:
    ostream_device( std::ostream & out )
        : out(out)
    {
    }

    size_t read( unsigned char* data
               , std::size_t    count )
    {
        throw std::runtime_error( "Bad io error." );
    }

    void seek( long count, int whence )
    {
        out.seekp(
                count,
                whence == SEEK_SET
                ?std::ios::beg
                :(whence == SEEK_CUR
                    ?std::ios::cur
                    :std::ios::end)
                );
    }

    void write( const unsigned char* data
              , std::size_t          count )
    {
        out.write( reinterpret_cast<char const*>( data )
                 , static_cast<std::streamsize>( count )
                 );
    }

    void flush()
    {
        out << std::flush;
    }

private:

    std::ostream& out;
};


/**
 * Metafunction to detect input devices. 
 * Should be replaced by an external facility in the future.
 */
template< typename IODevice  > struct is_input_device : mpl::false_{};
template< typename FormatTag > struct is_input_device< file_stream_device< FormatTag > > : mpl::true_{};
template<> struct is_input_device< istream_device > : mpl::true_{};

template< typename FormatTag, typename T, typename D = void >
struct is_adaptable_input_device : mpl::false_{};

template< typename FormatTag
        , typename T
        >
struct is_adaptable_input_device< FormatTag
                                , T
                                , typename enable_if< is_base_and_derived< std::istream
                                                                         , T
                                                                         >
                                                    >::type
                                >
    : mpl::true_
{
    typedef istream_device device_type;
};

template< typename FormatTag >
struct is_adaptable_input_device< FormatTag
                                , FILE*
                                , void
                                >
    : mpl::true_
{
    typedef file_stream_device< FormatTag > device_type;
};



/**
 * Metafunction to detect output devices. 
 * Should be replaced by an external facility in the future.
 */
template<typename IODevice> struct is_output_device : mpl::false_{};
template<typename FormatTag> struct is_output_device<file_stream_device< FormatTag > > : mpl::true_{};
template<> struct is_output_device<ostream_device> : mpl::true_{};

template<typename FormatTag, typename IODevice,typename D=void> struct is_adaptable_output_device : mpl::false_{};
template<typename FormatTag, typename T> struct is_adaptable_output_device<FormatTag, T,
    typename enable_if<is_base_and_derived<std::ostream,T> >::type
    >
  : mpl::true_
{
    typedef ostream_device device_type;
};

template<typename FormatTag> struct is_adaptable_output_device<FormatTag,FILE*,void>
  : mpl::true_
{
    typedef file_stream_device< FormatTag > device_type;
};

template<typename Device, typename FormatTag, typename ConversionPolicy> class reader;
template<typename Device, typename FormatTag> class writer;

} // namespace detail
} // namespace gil
} // namespace boost

#endif

