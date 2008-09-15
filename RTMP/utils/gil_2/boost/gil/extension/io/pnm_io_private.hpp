/*
  Copyright 2005-2006 Adobe Systems Incorporated
  Distributed under the MIT License (see accompanying file LICENSE_1_0_0.txt
  or a copy at http://opensource.adobe.com/licenses.html)
*/

/*************************************************************************************************/

#ifndef GIL_PNM_IO_PRIVATE_H
#define GIL_PNM_IO_PRIVATE_H

/// \file
/// \brief  Internal support for reading and writing PNM files
//
/// \author Svetlozar Fotev, Motorola Inc.
/// \author Christian Henning
///
/// \date   2005-2007 \n Last updated April 14, 2007

#include <stdio.h>
#include <boost/static_assert.hpp>
#include <boost/scoped_array.hpp>
#include <vector>

namespace boost { namespace gil {

namespace detail {

enum {
	type_mono_asc	= 1,	///< Monochrome ASCII encoding
	type_gray_asc	= 2,	///< Gray level ASCII encoding
	type_color_asc	= 3,	///< sRGB color ASCII encoding
	type_mono_bin	= 4,	///< Monochrome binary encoding
	type_gray_bin	= 5,	///< Gray level binary encoding
	type_color_bin	= 6		///< sRGB color binary encoding
};

/// Determines whether the given channel width and color space are supported for reading and writing
template <typename Chn, typename Spc> struct pnm_read_write_support_private {
	enum {
		supported	= false,
		channels	= 0,
		channel		= 0,
		pixel		= 0
	};
};
template <> struct pnm_read_write_support_private<bits8, gray_t> {
	enum {
		supported	= true,
		channels	= 1,
		channel		= 8,
		pixel		= 8
	};
};
template <> struct pnm_read_write_support_private<bits8, rgb_t> {
	enum {
		supported	= true,
		channels	= 3,
		channel		= 8,
		pixel		= 24
	};
};
template <> struct pnm_read_write_support_private<bits8, rgba_t> {
	enum {
		supported	= true,
		channels	= 4,
		channel		= 8,
		pixel		= 32
	};
};

/// Determines whether the given view type is supported for reading
template <typename V> struct pnm_read_support {
	enum {
		is_supported	= pnm_read_write_support_private<typename V::channel_t, typename V::color_space_t::base>::supported,
		num_channels	= pnm_read_write_support_private<typename V::channel_t, typename V::color_space_t::base>::channels,
		bit_depth		= pnm_read_write_support_private<typename V::channel_t, typename V::color_space_t::base>::channel,
		bit_pixel		= pnm_read_write_support_private<typename V::channel_t, typename V::color_space_t::base>::pixel,
		value			= is_supported
	};
};

/// Determines whether the given view type is supported for writing
template <typename V> struct pnm_write_support {
	enum {
		is_supported	= pnm_read_write_support_private<typename V::channel_t, typename V::color_space_t::base>::supported,
		num_channels	= pnm_read_write_support_private<typename V::channel_t, typename V::color_space_t::base>::channels,
		bit_depth		= pnm_read_write_support_private<typename V::channel_t, typename V::color_space_t::base>::channel,
		bit_pixel		= pnm_read_write_support_private<typename V::channel_t, typename V::color_space_t::base>::pixel,
		value			= is_supported
	};
};


/// Transfers and converts row of pixels
template <typename V, typename C> struct transfer_pnm {
	typedef typename V::x_iterator         iterator_t;
	typedef typename V::value_type         pixel_t;
	typedef typename channel_type<V>::type channel_t;

	/// From PNM to GIL
	static void convert(int bpp, const byte_t *src, iterator_t dest, int cnt, int maxv) throw() {
		byte_t    pak;
		channel_t maxp = std::numeric_limits<channel_t>::max();

		switch (bpp)
		{
		case 1:
			// 1 mono negative
			for (unsigned bit = 0; cnt > 0; --cnt, ++dest) {
				if (bit == 0) {
					bit = 8;
					pak = ~(*src++);
				}
				byte_t y = (pak >> --bit) & 0x01;

				*dest = convertor<V, C>::make(y * maxp / maxv);
			}
			break;

		case 8:
			// 8 mono negative, 8 gray
			if (maxv == 1) {
				for (; cnt > 0; --cnt, ++src, ++dest) {
					*dest = convertor<V, C>::make((1 - *src) * maxp);
				}
			}
			else {
				for (; cnt > 0; --cnt, ++src, ++dest) {
					*dest = convertor<V, C>::make(*src * maxp / maxv);
				}
			}
			break;

		case 24:
			// 8-8-8 RGB
			for (; cnt > 0; --cnt, ++dest) {
				byte_t r = *src++ * maxp / maxv;
				byte_t g = *src++ * maxp / maxv;
				byte_t b = *src++ * maxp / maxv;

				*dest = convertor<V, C>::make(r, g, b);
			}
			break;
		}
	}

	/// From GIL to PNM
	static void convert(int bpp, iterator_t src, byte_t *dest, int cnt) throw() {
		channel_t r, g, b, a;

		switch (bpp)
		{
		case 8:
			// 8 gray
			for (; cnt > 0; --cnt, ++src, ++dest) {
				convertor<V, C>::split(*src, r, g, b, a);
				*dest = g;
			}
			break;

		case 24:
		case 32:
			// 8-8-8 RGB, 8-8-8-8 RGB*
			for (; cnt > 0; --cnt, ++src) {
				convertor<V, C>::split(*src, r, g, b, a);

				*dest++ = r;
				*dest++ = g;
				*dest++ = b;
			}
			break;
		}
	}
};



class pnm_reader : public file_mgr_ext {

public:
    pnm_reader(FILE* file)           : file_mgr_ext(file)           { init(); }
    pnm_reader(const char* filename) : file_mgr_ext(filename, "rb") { init(); }
    pnm_reader(const wchar_t* filename) : file_mgr_ext(filename, L"rb") { init(); }

   template <typename VIEW>
   void apply( const VIEW& view )
   {
		typedef typename channel_type<VIEW>::type     channel_t;
		typedef typename color_space_type<VIEW>::type color_space_t;

		io_error_if( view.dimensions() != get_dimensions()
				 , "pnm_reader::apply(): input view dimensions do not match the image file");

	  // check if supported
		if (view.width() != width || view.height() != height) {
			io_error("Input view size does not match the image size");
		}
		if (pnm_read_write_support_private<channel_t, color_space_t>::channel != 8) {
			io_error("Input view type is incompatible with the image type");
		}

		// determine the the line pitch
		int pitch = (bpp == 1) ? (width + 7) >> 3 : width * channels;

		// read the raster
		std::vector<byte_t> row(pitch);

		if (type == type_mono_asc || type == type_gray_asc || type == type_color_asc) {
			char buf[16];

			for (int y = 0; y < height; ++y) {
				for (int x = 0; x < pitch; ++x) {
					// read the pixel value
					for (int k = 0; ; ) {
						int ch = read();

						if (isdigit(ch)) {
							buf[k++] = ch;
						}
						else if (k) {
							buf[k] = 0;
							break;
						}
						else if (ch == EOF || !isspace(ch)) {
							return;
						}
					}
					row[x] = atoi(buf);
				}
				transfer_pnm<VIEW, color_space_t>::convert(bpp, &row.front(), view.row_begin(y), width, maxv);
			}
		}
		else {
			for (int y = 0; y < height; ++y) {
				read(&row.front(), pitch);
				transfer_pnm<VIEW, color_space_t>::convert(bpp, &row.front(), view.row_begin(y), width, maxv);
			}
		}

   }

    template <typename IMAGE>
    void read_image(IMAGE& im) {
        im.recreate( get_dimensions() );
        apply(view(im));
    }

    point2<int> get_dimensions() const {
        return point2<int>( width, height );
    }

protected:
	/// Read PNM character
	char read_char(FILE *file) {
		int ch = getc(file);

		if (ch == EOF) {
			io_error("Unexpected EOF");
		}
		if (ch == '#') {
			// skip comment to EOL
			do {
				ch = getc(file);

				if (ch == EOF) {
					io_error("Unexpected EOF reading comment");
				}
			} while (ch != '\n' && ch != '\r');
		}
		return (char) ch;
	}

	/// Read PNM integer
	unsigned int read_int(FILE *file) {
		char ch;

		do {
			ch = read_char(file);
		} while (ch == ' ' || ch == '\t' || ch == '\n' || ch == '\r');

		if (ch < '0' || '9' < ch) {
			io_error("Unexpected characters reading decimal digits");
		}
		unsigned val = 0;

		do {
			unsigned dig = ch - '0';

			if (val > INT_MAX / 10 - dig) {
				io_error("Integer too large");
			}
			val = val * 10 + dig;

			ch = read_char(file);
		} while ('0' <= ch && ch <= '9');

		return val;
	}

	/// Read PNM information
	void init() {
		FILE *fp = get();

		// read PNM type information
		if (read_char(fp) != 'P') {
			io_error("Invalid PNM signature");
		}
		type = read_char(fp) - '0';

		if (type < type_mono_asc || type > type_color_bin) {
			io_error("Invalid PNM file (supports P1 to P6)");
		}

		// get dimensions
		width  = read_int(fp);
		height = read_int(fp);

		// get pixel range
		if (type == type_mono_asc || type == type_mono_bin) {
			maxv = 1;
		}
		else {
			maxv = read_int(fp);

			if (maxv > 255) {
				io_error("Unsupported PNM format (supports maximum value 255)");
			}
		}

		// determine the channels and BPP
		switch (type) {
			case type_mono_asc:  channels = 1; bpp =  8; break;
			case type_gray_asc:  channels = 1; bpp =  8; break;
			case type_color_asc: channels = 3; bpp = 24; break;

			case type_mono_bin:  channels = 1; bpp =  1; break;
			case type_gray_bin:  channels = 1; bpp =  8; break;
			case type_color_bin: channels = 3; bpp = 24; break;
		}
	}

protected:
	/// Image type and maximum pixel value
	int type, maxv;

	/// Image bits and channels
	int bpp, channels;

	/// Image dimensions
	int width, height;
};

class pnm_writer : public file_mgr_ext {
public:
    pnm_writer(FILE* file)           : file_mgr_ext(file)           {}
    pnm_writer(const char* filename) : file_mgr_ext(filename, "wb") {}
    pnm_writer(const wchar_t* filename) : file_mgr_ext(filename, L"wb") {}

    template <typename VIEW>
    void apply(const VIEW& view) {

	   typedef typename channel_type<VIEW>::type     channel_t;
      typedef typename color_space_type<VIEW>::type color_space_t;

      // check if supported
      if( pnm_read_write_support_private<channel_t, color_space_t>::channel != 8) {
	      io_error("Input view type is incompatible with the image type");
      }

		int width  = view.width();
		int height = view.height();

      int num    = num_channels<VIEW>::value;
		int chn    = std::min( 3, num );
		int bpp    = chn * 8;
		int pitch  = chn * width;
		int type   = (num_channels<VIEW>::value == 1) ? type_gray_bin : type_color_bin;

      // Add a white space at each string so read_int() can decide when a numbers ends.
      print_line("P%i ", type);
      print_line("%i ", width);
      print_line("%i ", height);
      print_line("255 ");

		// writes the raster
		std::vector<byte_t> row(pitch);

		for (int y = 0; y < height; ++y) {
			transfer_pnm<VIEW, color_space_t>::convert(bpp, view.row_begin(y), &row.front(), width);
			write(&row.front(), pitch);
		}
	}
};

} // namespace detail
} // namespace gil
} // namespace boost

#endif
