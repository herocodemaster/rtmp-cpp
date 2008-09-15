/*
  Copyright 2005-2006 Adobe Systems Incorporated
  Distributed under the MIT License (see accompanying file LICENSE_1_0_0.txt
  or a copy at http://opensource.adobe.com/licenses.html)
*/

/*************************************************************************************************/

#ifndef GIL_BMP_IO_PRIVATE_H
#define GIL_BMP_IO_PRIVATE_H

/// \file
/// \brief  Internal support for reading and writing BMP files
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

static const int header_size     = 14;
static const int win32_info_size = 40;
static const int os2_info_size   = 12;
static const int bm_signature    = 0x4D42;

/// BMP compression types
enum {
	ct_rgb		= 0,	///< RGB without compression
	ct_rle8		= 1,	///< 8 bit index with RLE compression
	ct_rle4		= 2,	///< 4 bit index with RLE compression
	ct_bitfield	= 3		///< 16 or 32 bit fields without compression
};

/// BMP file header
struct file_header {
	int			type;	///< File type
	long		size;	///< File size in bytes
	long		offset;	///< Pixels file offset
};

/// BMP information (Windows) or core (OS2) header
struct info_header {
	int			what;	///< Compression type
	int			width;	///< Width in pixels
	int			height;	///< Height in pixels, negative for top-down
	int			planes;	///< Bit planes, always 1
	int			bpp;	///< Bits per pixel
	int			colors;	///< Number of colors in the palette, zero for all colors
	int			entry;	///< Size of pallete entry in bytes
};

/// BMP color palette
struct color_map {
	unsigned	blue;	///< Blue bits mask
	unsigned	green;	///< Green bits mask
	unsigned	red;	///< Red bits mask
	unsigned	unused;	///< Reserved
};

/// Color channel mask
struct bit_field {
	unsigned	mask;	///< Bit mask at corresponding position
	unsigned	width;	///< Bit width of the mask
	unsigned	shift;	///< Bit position from right to left
};

/// BMP color masks
struct color_mask {
	bit_field	red;	 ///< Red bits
	bit_field	green; ///< Green bits
	bit_field	blue;	 ///< Blue bits
};

/// Determines whether the given channel width and color space are supported for reading and writing
template <typename Chn, typename Spc> struct bmp_read_write_support_private {
	enum {
		supported	= false,
		channel		= 0,
		pixel		= 0
	};
};
template <> struct bmp_read_write_support_private<bits8, gray_t> {
	enum {
		supported	= true,
		channel		= 8,
		pixel		= 8
	};
};
template <> struct bmp_read_write_support_private<bits8, rgb_t> {
	enum {
		supported	= true,
		channel		= 8,
		pixel		= 24
	};
};
template <> struct bmp_read_write_support_private<bits8, rgba_t> {
	enum {
		supported	= true,
		channel		= 8,
		pixel		= 32
	};
};

/// Assembles and disassembles pixel of given type
template <typename V, typename C> struct convertor {
};

template <typename V> struct convertor<V, gray_t>
{
	typedef typename channel_type<V>::type channel_t;
	typedef typename V::value_type         pixel_t;

	/// Create gray from luminance
	static pixel_t make(channel_t y) throw() {
		return pixel_t(y);
	}

	/// Create gray from RGBA components
	static pixel_t make(channel_t r, channel_t g, channel_t b, channel_t a = ~0) throw() {
		return pixel_t(rgb_to_luminance<channel_t>(r, g, b));
	}

	/// Split gray to RGBA components
	static void split(const pixel_t& p, channel_t& r, channel_t& g, channel_t& b, channel_t& a) throw() {
		r = get_color(p,gray_color_t());
		g = get_color(p,gray_color_t());
		b = get_color(p,gray_color_t());
		a = ~0;

	}
};

template <typename V> struct convertor<V, rgb_t>
{
	typedef typename channel_type<V>::type channel_t;
	typedef typename V::value_type         pixel_t;

	/// Create RGB from luminance
	static pixel_t make(channel_t y) throw() {
		return pixel_t(y, y, y);
	}

	/// Create RGB from RGBA components
	static pixel_t make(channel_t r, channel_t g, channel_t b, channel_t a = ~0) throw() {
		return pixel_t(r, g, b);
	}

	/// Split RGB to RGBA components
	static void split(const pixel_t& p, channel_t& r, channel_t& g, channel_t& b, channel_t& a) throw() {
		r = get_color(p, red_t());
		g = get_color(p, green_t());
		b = get_color(p, blue_t());
		a = ~0;
	}
};

template <typename V> struct convertor<V, rgba_t>
{
	typedef typename channel_type<V>::type channel_t;
	typedef typename V::value_type         pixel_t;

	/// Create RGBA from luminance
	static pixel_t make(channel_t y) throw() {
		return pixel_t(y, y, y, ~0);
	}

	/// Create RGBA from RGBA components
	static pixel_t make(channel_t r, channel_t g, channel_t b, channel_t a = ~0) throw() {
		return pixel_t(r, g, b, a);
	}

	/// Split RGBA to RGBA components
	static void split(const pixel_t& p, channel_t& r, channel_t& g, channel_t& b, channel_t& a) throw() {
		r = get_color(p, red_t());
		g = get_color(p, green_t());
		b = get_color(p, blue_t());
		a = get_color(p, alpha_t());
	}
};

/// Transfers and converts row of pixels
template <typename V, typename C> struct transfer
{
	typedef typename V::x_iterator         iterator_t;
	typedef typename V::value_type         pixel_t;
	typedef typename channel_type<V>::type channel_t;

	/// From BMP to GIL
	static void convert(int bpp, const byte_t *src, iterator_t dest, int cnt, const color_map pal[], const color_mask& msk) throw() {
		unsigned bit;
		byte_t   pak, idx;

		switch (bpp)
		{
		case 1:
			// 1 indexed
			for (bit = 0; cnt > 0; --cnt, ++dest) {
				if (bit == 0) {
					bit = 8;
					pak = *src++;
				}
				idx = (pak >> --bit) & 0x01;

				*dest = convertor<V, C>::make(pal[idx].red, pal[idx].green, pal[idx].blue);
			}
			break;

		case 4:
			// 4 indexed
			for (bit = 0; cnt > 0; --cnt, ++dest) {
				if (bit == 0) {
					bit = 8;
					pak = *src++;
				}
				bit -= 4;
				idx = (pak >> bit) & 0x0F;

				*dest = convertor<V, C>::make(pal[idx].red, pal[idx].green, pal[idx].blue);
			}
			break;

		case 8:
			// 8 indexed
			for (; cnt > 0; --cnt, ++src, ++dest) {
				idx = *src;
				*dest = convertor<V, C>::make(pal[idx].red, pal[idx].green, pal[idx].blue);
			}
			break;

		case 15:
		case 16:
			// 5-5-5, 5-6-5 BGR
			for (; cnt > 0; --cnt, ++dest, src += 2) {
				int p = (src[1] << 8) | src[0];

				int r = ((p & msk.red.mask)   >> msk.red.shift)   << (8 - msk.red.width);
				int g = ((p & msk.green.mask) >> msk.green.shift) << (8 - msk.green.width);
				int b = ((p & msk.blue.mask)  >> msk.blue.shift)  << (8 - msk.blue.width);

				*dest = convertor<V, C>::make(r, g, b);
			}
			break;

		case 24:
			// 8-8-8 BGR
			for (; cnt > 0; --cnt, ++dest) {
				byte_t b = *src++;
				byte_t g = *src++;
				byte_t r = *src++;

				*dest = convertor<V, C>::make(r, g, b);
			}
			break;

		case 32:
			// 8-8-8-8 BGR*
			for (; cnt > 0; --cnt, ++dest) {
				byte_t b = *src++;
				byte_t g = *src++;
				byte_t r = *src++;
				byte_t a = *src++;

				*dest = convertor<V, C>::make(r, g, b);
			}
			break;
		}
	}

	/// From GIL to BMP
	static void convert(int bpp, iterator_t src, byte_t *dest, int cnt) throw() {
		channel_t r, g, b, a;

		switch (bpp)
		{
		case 8:
			// 8
			for (; cnt > 0; --cnt, ++src, ++dest) {
				convertor<V, C>::split(*src, r, g, b, a);
				*dest = g;
			}
			break;

		case 24:
			// 8-8-8
			for (; cnt > 0; --cnt, ++src) {
				convertor<V, C>::split(*src, r, g, b, a);

				*dest++ = b;
				*dest++ = g;
				*dest++ = r;
			}
			break;

		case 32:
			// 8-8-8-8
			for (; cnt > 0; --cnt, ++src) {
				convertor<V, C>::split(*src, r, g, b, a);

				*dest++ = b;
				*dest++ = g;
				*dest++ = r;
				*dest++ = 0;
			}
			break;
		}
	}
};

class bmp_reader : public file_mgr_ext {
protected:
   info_header _info_header;
   file_header _file_header;

    void init() {

      // Read file header.
      _file_header.type = read_int16();
      _file_header.size = read_int32();
      read_int16(); // read reserved bytes
      read_int16(); // read reserved bytes
      _file_header.offset = read_int32();

      if (_file_header.type != bm_signature) {
	      io_error("file_mgr: not a BMP file");
      }

      if (_file_header.offset >= _file_header.size) {
	      io_error("file_mgr: invalid BMP file header");
      }

      // Read the info header size.
      int info_header_size = read_int32();

      if( info_header_size == win32_info_size )
      {
	      // Windows header
	      _info_header.width  = read_int32();
	      _info_header.height = read_int32();
	      _info_header.planes = read_int16();
	      _info_header.bpp    = read_int16();
	      _info_header.what   = read_int32();
					         read_int32();
					         read_int32();
					         read_int32();
	      _info_header.colors = read_int32();
					         read_int32();
	      _info_header.entry  = 4;
      }
      else if ( info_header_size == os2_info_size )
      {
	      // OS2 header
	      _info_header.width  = read_int16();
	      _info_header.height = read_int16();
	      _info_header.planes = read_int16();
	      _info_header.bpp    = read_int16();
	      _info_header.what   = ct_rgb;
	      _info_header.colors = 0;
	      _info_header.entry  = 3;
      }
      else {
	      io_error("file_mgr: invalid BMP info header");
      }

      /// check supported bits per pixel
      if (_info_header.bpp < 1 || _info_header.bpp > 32) {
	      io_error("file_mgr: unsupported BMP format");
      }
    }


public:
    bmp_reader(FILE* file)              : file_mgr_ext( file )            { init(); }
    bmp_reader(const char* filename)    : file_mgr_ext( filename, "rb" )  { init(); }
    bmp_reader(const wchar_t* filename) : file_mgr_ext( filename, L"rb" ) { init(); }

   template <typename VIEW>
   void apply( const VIEW& view )
   {
      io_error_if( view.dimensions() != get_dimensions()
                 , "bmp_reader::apply(): input view dimensions do not match the image file");

      color_mask mask;

      // read the color masks
      if( _info_header.what == ct_bitfield )
      {
         mask.red.mask    = read_int32();
         mask.green.mask  = read_int32();
         mask.blue.mask   = read_int32();

         mask.red.width   = count_ones( mask.red.mask   );
         mask.green.width = count_ones( mask.green.mask );
         mask.blue.width  = count_ones( mask.blue.mask  );

         mask.red.shift   = trailing_zeros( mask.red.mask   );
         mask.green.shift = trailing_zeros( mask.green.mask );
         mask.blue.shift  = trailing_zeros( mask.blue.mask  );
      }
      else if( _info_header.what == ct_rgb )
      {
         switch( _info_header.bpp )
         {
            case 15: case 16:
            {
		         mask.red.mask   = 0x007C00; mask.red.width   = 5; mask.red.shift   = 10;
		         mask.green.mask = 0x0003E0; mask.green.width = 5; mask.green.shift =  5;
		         mask.blue.mask  = 0x00001F; mask.blue.width  = 5; mask.blue.shift  =  0;
		         break;
            }

	         case 24: case 32:
	         {
		         mask.red.mask   = 0xFF0000; mask.red.width   = 8; mask.red.shift   = 16;
		         mask.green.mask = 0x00FF00; mask.green.width = 8; mask.green.shift =  8;
		         mask.blue.mask  = 0x0000FF; mask.blue.width  = 8; mask.blue.shift  =  0;
		         break;
            }
	      }
      }
      else
      {
	      io_error( "bmp_reader::apply(): unsupported BMP compression" );
      }

      // Read the color map.

		std::vector<color_map> palette;

      if( _info_header.bpp <= 8 )
      {
	      int entries = _info_header.colors;

	      if( entries == 0 )
	      {
		      entries = 1 << _info_header.bpp;
	      }

         palette.resize( entries );

	      for( int i = 0; i < entries; ++i )
	      {
		      palette[i].blue  = read_int8();
		      palette[i].green = read_int8();
		      palette[i].red   = read_int8();

		      if( _info_header.entry > 3 )
		      {
			      read_int8();
		      }
	      }
      }

      seek(_file_header.offset);

      // the row pitch must be multiple 4 bytes
      int pitch;

      if (_info_header.bpp < 8) {
	      pitch = ((_info_header.width * _info_header.bpp) + 7) >> 3;
      }
      else {
	      pitch = _info_header.width * ((_info_header.bpp + 7) >> 3);
      }
      pitch = (pitch + 3) & ~3;

      // read the raster
      std::vector<byte_t> row(pitch);

      int ybeg = 0;
      int yend = _info_header.height;
      int yinc = 1;

      if( _info_header.height > 0 )
      {
	      ybeg = _info_header.height - 1;
	      yend = -1;
	      yinc = -1;
      }

		const color_map *pal = 0;

		if( palette.size() > 0 )
		{
			pal = &palette.front();
		}

      for( int y = ybeg; y != yend; y += yinc )
      {
         typedef typename color_space_type<VIEW>::type Spc;

	      read(&row.front(), pitch);
	      transfer<VIEW, Spc>::convert( _info_header.bpp
	                                  , &row.front()
	                                  , view.row_begin( y )
	                                  , _info_header.width
	                                  , pal
	                                  , mask                 );
      }
    }
    
    template <typename IMAGE>
    void read_image(IMAGE& im) {
        im.recreate( get_dimensions() );
        apply(view(im));
    }

    point2<int> get_dimensions() const {
        return point2<int>( _info_header.width,_info_header.height );
    }
};

class bmp_writer : public file_mgr_ext {
public:
    bmp_writer(FILE* file)           : file_mgr_ext(file)           {}
    bmp_writer(const char* filename) : file_mgr_ext(filename, "wb") {}
    bmp_writer(const wchar_t* filename) : file_mgr_ext(filename, L"wb") {}
    
    template <typename VIEW>
    void apply(const VIEW& view) {

      typedef typename channel_type<VIEW>::type     channel_t;
      typedef typename color_space_type<VIEW>::type color_space_t;

      // check if supported
      if (bmp_read_write_support_private<channel_t, color_space_t>::channel != 8) {
	      io_error("Input view type is incompatible with the image type");
      }

      // compute the file size
      int bpp = num_channels<VIEW>::value * 8;
      int ent = 0;

      if (bpp <= 8) {
	      ent = 1 << bpp;
      }
      int spn = (view.width() * num_channels<VIEW>::value + 3) & ~3;
      int ofs = header_size + win32_info_size + ent * 4;
      int siz = ofs + spn * view.height();

      // write the BMP file header
      write_int16(bm_signature);
      write_int32(siz);
      write_int16(0);
      write_int16(0);
      write_int32(ofs);

      // writes Windows information header
      write_int32( win32_info_size );
      write_int32(view.width());
      write_int32(view.height());
      write_int16(1);
      write_int16(bpp);
      write_int32(ct_rgb);
      write_int32(0);
      write_int32(0);
      write_int32(0);
      write_int32(ent);
      write_int32(0);

      // writes artificial gray palette
      for (int i = 0; i < ent; ++i) {
	      write_int8(i);
	      write_int8(i);
	      write_int8(i);
	      write_int8(0);
      }

      // writes the raster
      std::vector<byte_t> row(spn);

      for (int y = view.height() - 1; y >= 0; --y) {
	      transfer<VIEW, color_space_t>::convert(bpp, view.row_begin(y), &row.front(), view.width());
	      write(&row.front(), spn);
      }
    }
};

} // namespace detail
} // namespace gil
} // namespace boost

#endif
