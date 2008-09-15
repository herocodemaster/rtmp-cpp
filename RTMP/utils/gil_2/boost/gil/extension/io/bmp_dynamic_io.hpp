/*
  Copyright 2005-2006 Adobe Systems Incorporated
  Distributed under the MIT License (see accompanying file LICENSE_1_0_0.txt
  or a copy at http://opensource.adobe.com/licenses.html)
*/


#ifndef GIL_EXTENSION_IO_BMP_DYNAMIC_IO_HPP
#define GIL_EXTENSION_IO_BMP_DYNAMIC_IO_HPP

/// \file
/// \brief  BMP image files dynamic I/O operations.
///
/// \author Svetlozar Fotev, Motorola Inc.
/// \author Christian Henning
///
/// \date   2005-2007 \n Last updated April 14, 2007


#include <boost/gil/extension/io/bmp_io.hpp>
#include <boost/gil/extension/dynamic_image/any_image.hpp>
#include <boost/gil/extension/dynamic_image/apply_operation.hpp>
#include <boost/gil/extension/io/dynamic_io.hpp>
#include <boost/mpl/bool.hpp>
#include <boost/mpl/vector.hpp>

namespace boost { namespace gil {

namespace detail {
	/// Determines whether the given view type is supported for reading
	struct bmp_read_check {
		template <typename V> struct apply: boost::mpl::bool_<bmp_read_write_support<V>::is_supported> {
		};
	};

	/// Determines whether the given view type is supported for writing
	struct bmp_write_check {
		template <typename V> struct apply: boost::mpl::bool_<bmp_read_write_support<V>::is_supported> {
		};
	};

	/// Checks the BMP format
	struct bmp_format_check {
		bmp_format_check(int bpp) throw(): bit_depth(bpp) {
		}

		template <typename IMG> bool apply() {
			return bmp_read_write_support<typename IMG::view_t>::bit_pixel == bit_depth;
		}

	private:
		int bit_depth;
	};

	/// BMP dynamic file reader
	struct bmp_reader_dynamic: bmp_reader {
		/// Creates reader from file
		template <typename T> bmp_reader_dynamic(const T *file): bmp_reader(file) {
		}

		/// Reads a run-time instantiated image from file
		template <typename IMG> void read_image(any_image<IMG>& img) {
			if (!construct_matched(img, bmp_format_check(_info_header.bpp))) {
				io_error("No matching image type");
			}
			else {
            img.recreate( point2<int>(_info_header.width, abs(_info_header.height )));
				detail::dynamic_io_fnobj<bmp_read_check, bmp_reader> op(this);
				apply_operation(view(img), op);
			}
		}
	};

	/// BMP dynamic file writer
	struct bmp_writer_dynamic: bmp_writer {
		/// Creates writer into file
		template <typename T> bmp_writer_dynamic(const T *name): bmp_writer(name) {
		}

		/// Writes an image view to the file
		template <typename V> void write_view(const any_image_view<V>& rtv) {
			detail::dynamic_io_fnobj<bmp_write_check, bmp_writer> op(this);
			apply_operation(rtv, op);
		}
	};
	
} // namespace detail


/// \brief reads a BMP image into a run-time instantiated image
/// \ingroup BMP_IO
/// Opens the given BMP file name, selects the first type in Images whose color space and channel are compatible to those of the image file
/// and creates a new image of that type with the dimensions specified by the image file.
/// Throws std::ios_base::failure if none of the types in Images are compatible with the type on disk.
template <typename IMG> 
inline void bmp_read_image(const wchar_t *file, any_image<IMG>& img) {
	detail::bmp_reader_dynamic m(file);
	m.read_image(img);
}

/// \brief reads a BMP image into a run-time instantiated image
/// \ingroup BMP_IO
/// Opens the given BMP file name, selects the first type in Images whose color space and channel are compatible to those of the image file
/// and creates a new image of that type with the dimensions specified by the image file.
/// Throws std::ios_base::failure if none of the types in Images are compatible with the type on disk.
template <typename IMG> 
inline void bmp_read_image(const char *file, any_image<IMG>& img) {
	detail::bmp_reader_dynamic m(file);
	m.read_image(img);
}

/// \brief reads a BMP image into a run-time instantiated image
/// \ingroup BMP_IO
template <typename IMG> 
inline void bmp_read_image(const std::string& file, any_image<IMG>& img) {
	bmp_read_image(file.c_str(), img);
}

/// \brief Saves the currently instantiated view to a bmp file specified by the given bmp image file name.
/// \ingroup BMP_IO
/// Throws std::ios_base::failure if the currently instantiated view type is not supported for writing by the I/O extension 
/// or if it fails to create the file.
template <typename V> 
inline void bmp_write_view(const wchar_t *file, const any_image_view<V>& view) {
	detail::bmp_writer_dynamic m(file);
	m.write_view(view);
}

/// \brief Saves the currently instantiated view to a bmp file specified by the given bmp image file name.
/// \ingroup BMP_IO
/// Throws std::ios_base::failure if the currently instantiated view type is not supported for writing by the I/O extension 
/// or if it fails to create the file.
template <typename V> 
inline void bmp_write_view(const char *file, const any_image_view<V>& view) {
	detail::bmp_writer_dynamic m(file);
	m.write_view(view);
}

/// \brief Saves the currently instantiated view to a bmp file specified by the given bmp image file name.
/// \ingroup BMP_IO
template <typename V> inline void bmp_write_view(const std::string& file, const any_image_view<V>& view) {
	bmp_write_view(file.c_str(), view);
}

/// Supported BMP images types list
typedef boost::mpl::vector<gray8_image_t, rgb8_image_t, rgba8_image_t> bmp_types_t;

} // namespace gil
} // namespace boost

#endif
