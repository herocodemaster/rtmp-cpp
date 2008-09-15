/*
  Copyright 2005-2006 Adobe Systems Incorporated
  Distributed under the MIT License (see accompanying file LICENSE_1_0_0.txt
  or a copy at http://opensource.adobe.com/licenses.html)
*/

/*************************************************************************************************/

#ifndef GIL_IO_BASE_H
#define GIL_IO_BASE_H

/// \file
/// \brief  Handle input-output errors
/// \author Lubomir Bourdev and Hailin Jin \n
///         Adobe Systems Incorporated
/// \date   2005-2007 \n Last updated April 14, 2007

#if defined _WIN32
	#include <malloc.h>
#elif defined __GNUC__
	#include <alloca.h>
#endif

#include <ios>
#include <stdarg.h>

#include <boost/gil/extension/io/io_error.hpp>

#include <boost/cstdint.hpp>
#include <boost/shared_ptr.hpp>

namespace boost { namespace gil {

namespace detail {

   typedef unsigned char byte_t;

		/// Buffer element size; treat void elements as bytes
		template <typename T> struct buff_item {
			enum { size = sizeof(T) };
		};
		template <> struct buff_item<void> {
			enum { size = 1 };
		};

    class file_mgr_ext : public file_mgr
    {
    protected:

        file_mgr_ext(FILE* file) 
        : file_mgr( file )
        {}

        file_mgr_ext( const char* filename, const char* flags )
        : file_mgr( filename, flags )
        {}

        file_mgr_ext(const wchar_t* filename, const wchar_t* flags)
        : file_mgr( "", "" )
        {
            FILE* fp;
            #ifdef _WIN32
               io_error_if((fp=_wfopen(filename,flags))==NULL, "file_mgr: failed to open file");
            #else
               // convert filename string
			      int   len = wcslen(filename);
			      char* filename_buf = reinterpret_cast<char*>( alloca(len));
			      wcstombs(filename_buf, filename, len);

               // convert flags string
               len = wcslen(flags);
               char* flags_buf = reinterpret_cast<char*>( alloca(len));
			      wcstombs(flags_buf, flags, len);

               // open file
               io_error_if((fp=fopen(filename_buf,flags_buf))==NULL, "file_mgr: failed to open file");
            #endif

            _fp=boost::shared_ptr<FILE>(fp,fclose);
        }

			/// Reads raw byte
			int read() throw() {
				return fgetc(_fp.get());
			}

			/// Reads number of elements in a buffer
			template <typename T> size_t read(T *buf, size_t cnt) throw() {
				return fread(buf, buff_item<T>::size, cnt, _fp.get());
			}

			/// Reads array
			template <typename T, size_t N> size_t read(T (&buf)[N]) throw() {
				return read(buf, N);
			}

			/// Writes number of elements from a buffer
			template <typename T> size_t write(const T *buf, size_t cnt) throw() {
				return fwrite(buf, buff_item<T>::size, cnt, _fp.get());
			}

			/// Writes array
			template <typename T, size_t N> size_t write(const T (&buf)[N]) throw() {
				return write(buf, N);
			}

			/// Positions the file pointer absolutely
			long seek(long at) throw() {
				return fseek(_fp.get(), at, SEEK_SET);
			}

			/// Reads byte
			boost::uint8_t read_int8() throw() {
				byte_t m[1];

				read(m);
				return m[0];
			}

			/// Reads 16 bit little endian integer
			boost::uint16_t read_int16() throw() {
				byte_t m[2];

				read(m);
				return (m[1] << 8) | m[0];
			}

			/// Reads 32 bit little endian integer
			boost::uint32_t read_int32() throw() {
				byte_t m[4];

				read(m);
				return (m[3] << 24) | (m[2] << 16) | (m[1] << 8) | m[0];
			}

			/// Writes byte
			void write_int8(boost::uint8_t x) throw() {
				byte_t m[1] = { x };
				write(m);
			}

			/// Writes 16 bit little endian integer
			void write_int16(boost::uint16_t x) throw() {
				byte_t m[2];

				m[0] = byte_t(x >> 0);
				m[1] = byte_t(x >> 8);

				write(m);
			}

			/// Writes 32 bit little endian integer
			void write_int32(boost::uint32_t x) throw() {
				byte_t m[4];

				m[0] = byte_t(x >>  0);
				m[1] = byte_t(x >>  8);
				m[2] = byte_t(x >> 16);
				m[3] = byte_t(x >> 24);

				write(m);
			}

			/// Reads ASCII text line
			char *read_line(char *text, int cnt) throw() {
				return fgets(text, cnt, _fp.get());
			}

			/// Reads ASCII text line
			template <size_t N> char *read_line(char (&text)[N]) throw() {
				return read_line(text, N - 1);
			}

			/// Prints formatted ASCII text
			void print_line(const char *fmt, ...) throw() {
				va_list arg;

				va_start(arg, fmt);
				vfprintf(_fp.get(), fmt, arg);
				va_end(arg);
			}


		/// Compute the consecutive zero bits on the right
		template <typename T> inline unsigned trailing_zeros(T x) throw() {
			unsigned n;

			x = ~x & (x - 1);
			n = 0;

			while (x) {
				n = n + 1;
				x = x >> 1;
			}
			return n;
		}

		/// Counts a bit-set
		template <typename T> inline unsigned count_ones(T x) throw() {
			unsigned n;

			for (n = 0; x; ++n) {
				// clear the least significant bit set
				x &= x - 1;
			}
			return n;
		}
    };
}

} // namespace gil
} // namespace boost

#endif // GIL_IO_BASE_H
