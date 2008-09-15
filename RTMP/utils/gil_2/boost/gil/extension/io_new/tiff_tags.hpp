/*
    Copyright 2007-2008 Christian Henning, Andreas Pokorny, Lubomir Bourdev
    Use, modification and distribution are subject to the Boost Software License,
    Version 1.0. (See accompanying file LICENSE_1_0.txt or copy at
    http://www.boost.org/LICENSE_1_0.txt).
*/

/*************************************************************************************************/

#ifndef BOOST_GIL_EXTENSION_IO_TIFF_TAGS_HPP
#define BOOST_GIL_EXTENSION_IO_TIFF_TAGS_HPP

////////////////////////////////////////////////////////////////////////////////////////
/// \file               
/// \brief All supported tiff tags by the gil io extension.
/// \author Christian Henning, Andreas Pokorny, Lubomir Bourdev \n
///         
/// \date   2007-2008 \n
///
////////////////////////////////////////////////////////////////////////////////////////

/// For a description see: http://www.awaresystems.be/imaging/tiff/tifftags/baseline.html
/// see http://www.remotesensing.org/libtiff/
/// or http://www.libtiff.org/man/


extern "C" {
#include "tiff.h"
}

#include "detail/base.hpp"

namespace boost { namespace gil {

struct tiff_tag : format_tag {};

struct tiff_new_subfile_type
{
   typedef boost::uint32_t type;
   static const unsigned int tag = TIFFTAG_SUBFILETYPE;
};

struct tiff_subfile_type
{
   typedef boost::uint16_t type;
   static const unsigned int tag = TIFFTAG_OSUBFILETYPE;
};

struct tiff_image_width
{
   typedef boost::uint32_t type;
   static const unsigned int tag = TIFFTAG_IMAGEWIDTH;
};

struct tiff_image_height
{
   typedef boost::uint32_t type;
   static const unsigned int tag = TIFFTAG_IMAGELENGTH;
};

struct tiff_bits_per_sample
{
   typedef boost::uint16_t type;
   static const unsigned int tag = TIFFTAG_BITSPERSAMPLE;
};

struct tiff_compression
{
   typedef boost::uint16_t type;
   static const unsigned int tag = TIFFTAG_COMPRESSION;
};

struct tiff_photometric_interpretation
{
   typedef boost::uint16_t type;
   static const unsigned int tag = TIFFTAG_PHOTOMETRIC;
};

struct tiff_thresholding
{
   typedef boost::uint16_t type;
   static const unsigned int tag = TIFFTAG_THRESHHOLDING;
};

struct tiff_cell_width
{
   typedef boost::uint16_t type;
   static const unsigned int tag = TIFFTAG_CELLWIDTH;
};

struct tiff_cell_length
{
   typedef boost::uint16_t type;
   static const unsigned int tag = TIFFTAG_CELLLENGTH;
};

struct tiff_fill_order
{
   typedef std::string type;
   static const unsigned int tag = TIFFTAG_FILLORDER;
};

struct tiff_image_description
{
   typedef std::string type;
   static const unsigned int tag = TIFFTAG_IMAGEDESCRIPTION;
};

struct tiff_make
{
   typedef std::string type;
   static const unsigned int tag = TIFFTAG_MAKE;
};

struct tiff_model
{
   typedef std::string type;
   static const unsigned int tag = TIFFTAG_MODEL;
};

struct tiff_orientation
{
   typedef boost::uint16_t type;
   static const unsigned int tag = TIFFTAG_ORIENTATION;
};

struct tiff_samples_per_pixel
{
   typedef boost::uint16_t type;
   static const unsigned int tag = TIFFTAG_SAMPLESPERPIXEL;
};

struct tiff_rows_per_strip
{
   typedef boost::uint32_t type;
   static const unsigned int tag = TIFFTAG_ROWSPERSTRIP;
};

struct tiff_min_sample_value
{
   typedef boost::uint16_t type;
   static const unsigned int tag = TIFFTAG_MINSAMPLEVALUE;
};

struct tiff_max_sample_value
{
   typedef boost::uint16_t type;
   static const unsigned int tag = TIFFTAG_MAXSAMPLEVALUE;
};

struct tiff_x_resolution
{
   typedef float type;
   static const unsigned int tag = TIFFTAG_XRESOLUTION;
};

struct tiff_y_resolution
{
   typedef float type;
   static const unsigned int tag = TIFFTAG_YRESOLUTION;
};

struct tiff_planar_configuration
{
   typedef boost::uint16_t type;
   static const unsigned int tag = TIFFTAG_PLANARCONFIG;
};

struct tiff_gray_response_unit
{
   typedef boost::uint16_t type;
   static const unsigned int tag = TIFFTAG_GRAYRESPONSEUNIT;
};

struct tiff_gray_response_curve
{
   typedef boost::uint16_t* type;
   static const unsigned int tag = TIFFTAG_GRAYRESPONSECURVE;
};

struct tiff_resolution_unit
{
   typedef boost::uint16_t type;
   static const unsigned int tag = TIFFTAG_RESOLUTIONUNIT;
};

struct tiff_software
{
   typedef std::string type;
   static const unsigned int tag = TIFFTAG_SOFTWARE;
};

struct tiff_date_time
{
   typedef std::string type;
   static const unsigned int tag = TIFFTAG_DATETIME;
};

struct tiff_artist
{
   typedef std::string type;
   static const unsigned int tag = TIFFTAG_ARTIST;
};

struct tiff_host_computer
{
   typedef std::string type;
   static const unsigned int tag = TIFFTAG_HOSTCOMPUTER;
};

struct tiff_color_map
{
   typedef boost::uint16_t* red_t;
   typedef boost::uint16_t* green_t;
   typedef boost::uint16_t* blue_t;

   static const unsigned int tag = TIFFTAG_COLORMAP;
};

struct tiff_extra_samples
{
   typedef boost::uint16_t* type;
   static const unsigned int tag = TIFFTAG_EXTRASAMPLES;
};

struct tiff_copyright
{
   typedef std::string type;
   static const unsigned int tag = TIFFTAG_COPYRIGHT;
};

// non-baseline tags

struct tiff_sample_format
{
   typedef boost::uint16_t type;
   static const unsigned int tag = TIFFTAG_SAMPLEFORMAT;
};

struct tiff_indexed
{
   typedef boost::uint16_t type;
   static const unsigned int tag = TIFFTAG_INDEXED;
};

template<>
struct image_read_info<tiff_tag>
{
   tiff_image_width::type  _width;
   tiff_image_height::type _height;

   tiff_compression::type _compression;

   tiff_bits_per_sample::type   _bits_per_sample;
   tiff_samples_per_pixel::type _samples_per_pixel;
   tiff_sample_format::type     _sample_format;

   tiff_planar_configuration::type _planar_configuration;

   tiff_photometric_interpretation::type _photometric_interpretation;
};

template<>
struct image_write_info<tiff_tag>
{
   tiff_photometric_interpretation::type _photometric_interpretation;
   tiff_compression::type                _compression;
   tiff_orientation::type                _orientation;
   tiff_planar_configuration::type       _planar_configuration;
};

} // namespace gil
} // namespace boost

#endif // BOOST_GIL_EXTENSION_IO_TIFF_TAGS_HPP
