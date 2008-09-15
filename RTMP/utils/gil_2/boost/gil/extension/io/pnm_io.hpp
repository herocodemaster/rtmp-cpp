/*
  Copyright 2005-2006 Adobe Systems Incorporated
  Distributed under the MIT License (see accompanying file LICENSE_1_0_0.txt
  or a copy at http://opensource.adobe.com/licenses.html)
*/

/*************************************************************************************************/

#ifndef GIL_PNM_IO_H
#define GIL_PNM_IO_H

/// \file
/// \brief  Support for reading and writing PNM files
//
/// \author Svetlozar Fotev, Motorola Inc.
/// \author Christian Henning
///         
/// \date   2005-2007 \n Last updated April 14, 2007

#include <cstdio>
#include <algorithm>
#include <string>
#include <boost/static_assert.hpp>
#include <boost/shared_ptr.hpp>
#include <boost/gil/extension/io/io_base.hpp>
#include <boost/gil/extension/io/pnm_io_private.hpp>

namespace boost { namespace gil {

/// Determines whether the given view type is supported for reading

/// \brief Determines whether the given view type is supported for reading
/// \ingroup PNM_IO
template <typename VIEW> struct pnm_read_write_support {

    BOOST_STATIC_CONSTANT( bool
                         , is_supported =
                           (detail::pnm_read_write_support_private< typename channel_type<VIEW>::type
                                                                  , typename color_space_type<VIEW>::type
                                                                  >::supported ));

    BOOST_STATIC_CONSTANT( unsigned int
                         , bit_depth =
                           ( detail::pnm_read_write_support_private< typename channel_type<VIEW>::type
                                                                   , typename color_space_type<VIEW>::type
                                                                   >::channel ));

    BOOST_STATIC_CONSTANT( unsigned int
                         , bit_pixel =
                           ( detail::pnm_read_write_support_private< typename channel_type<VIEW>::type
                                                                   , typename color_space_type<VIEW>::type
                                                                   >::pixel ));

    BOOST_STATIC_CONSTANT(bool, value=is_supported);
};

/// \brief Returns the width and height of the PNM file at the specified location.
/// Throws std::ios_base::failure if the location does not correspond to a valid PNM file
/// \ingroup PNM_IO
inline point2<int> pnm_read_dimensions(const char* filename) {
    detail::pnm_reader m(filename);
    return m.get_dimensions();
}

/// \brief Returns the width and height of the PNM file at the specified location.
/// Throws std::ios_base::failure if the location does not correspond to a valid PNM file
/// \ingroup PNM_IO
inline point2<int> pnm_read_dimensions(const std::string& filename) {
    return pnm_read_dimensions(filename.c_str());
}

/// \brief Loads the image specified by the given pnm image file name into the given view.
/// \ingroup PNM_IO
/// Triggers a compile assert if the view color space and channel depth are not supported by the PNM library or by the I/O extension.
/// Throws std::ios_base::failure if the file is not a valid PNM file, or if its color space or channel depth are not 
/// compatible with the ones specified by VIEW, or if its dimensions don't match the ones of the view.
template <typename VIEW>
inline void pnm_read_view(const wchar_t* filename,const VIEW& view) {
    BOOST_STATIC_ASSERT(pnm_read_write_support<VIEW>::is_supported);

    detail::pnm_reader m(filename);
    m.apply(view);
}

/// \brief Loads the image specified by the given pnm image file name into the given view.
/// \ingroup PNM_IO
/// Triggers a compile assert if the view color space and channel depth are not supported by the PNM library or by the I/O extension.
/// Throws std::ios_base::failure if the file is not a valid PNM file, or if its color space or channel depth are not 
/// compatible with the ones specified by VIEW, or if its dimensions don't match the ones of the view.
template <typename VIEW>
inline void pnm_read_view(const char* filename,const VIEW& view) {
    BOOST_STATIC_ASSERT(pnm_read_write_support<VIEW>::is_supported);

    detail::pnm_reader m(filename);
    m.apply(view);
}

/// \brief Loads the image specified by the given pnm image file name into the given view.
/// \ingroup PNM_IO
template <typename VIEW>
inline void pnm_read_view(const std::string& filename,const VIEW& view) {
    pnm_read_view(filename.c_str(),view);
}

/// \brief Allocates a new image whose dimensions are determined by the given pnm image file, and loads the pixels into it.
/// \ingroup PNM_IO
/// Triggers a compile assert if the image color space or channel depth are not supported by the PNM library or by the I/O extension.
/// Throws std::ios_base::failure if the file is not a valid PNM file, or if its color space or channel depth are not 
/// compatible with the ones specified by IMAGE
template <typename IMAGE>
inline void pnm_read_image(const wchar_t* filename,IMAGE& im) {
   BOOST_STATIC_ASSERT(pnm_read_write_support<IMAGE::view_t>::is_supported);

    detail::pnm_reader m(filename);
    m.read_image(im);
}

/// \brief Allocates a new image whose dimensions are determined by the given pnm image file, and loads the pixels into it.
/// \ingroup PNM_IO
/// Triggers a compile assert if the image color space or channel depth are not supported by the PNM library or by the I/O extension.
/// Throws std::ios_base::failure if the file is not a valid PNM file, or if its color space or channel depth are not 
/// compatible with the ones specified by IMAGE
template <typename IMAGE>
inline void pnm_read_image(const char* filename,IMAGE& im) {
   BOOST_STATIC_ASSERT(pnm_read_write_support<IMAGE::view_t>::is_supported);

    detail::pnm_reader m(filename);
    m.read_image(im);
}

/// \brief Allocates a new image whose dimensions are determined by the given pnm image file, and loads the pixels into it.
/// \ingroup PNM_IO
template <typename IMAGE>
inline void pnm_read_image(const std::string& filename,IMAGE& im) {
    pnm_read_image(filename.c_str(),im);
}

/// \brief Saves the view to a pnm file specified by the given pnm image file name.
/// \ingroup PNM_IO
/// Triggers a compile assert if the view color space and channel depth are not supported by the pnm library or by the I/O extension.
/// Throws std::ios_base::failure if it fails to create the file.
template <typename VIEW>
inline void pnm_write_view(const wchar_t* filename,const VIEW& view ) {
    BOOST_STATIC_ASSERT(pnm_read_write_support<VIEW>::is_supported);

    detail::pnm_writer m(filename);
    m.apply(view);
}

/// \brief Saves the view to a pnm file specified by the given pnm image file name.
/// \ingroup PNM_IO
/// Triggers a compile assert if the view color space and channel depth are not supported by the pnm library or by the I/O extension.
/// Throws std::ios_base::failure if it fails to create the file.
template <typename VIEW>
inline void pnm_write_view(const char* filename,const VIEW& view ) {
    BOOST_STATIC_ASSERT(pnm_read_write_support<VIEW>::is_supported);

    detail::pnm_writer m(filename);
    m.apply(view);
}

/// \brief Saves the view to a pnm file specified by the given pnm image file name.
/// \ingroup PNM_IO
template <typename VIEW>
inline void pnm_write_view(const std::string& filename,const VIEW& view ) {
    pnm_write_view(filename.c_str(),view);
}

} // namespace gil
} // namespace boost

#endif
