// Copyright 2007 Christian Henning.
// Distributed under the Boost Software License, Version 1.0. (See
// accompanying file LICENSE_1_0.txt or copy at
// http://www.boost.org/LICENSE_1_0.txt)

/*************************************************************************************************/

#ifndef GIL_SDL_CONVERTERS_HPP
#define GIL_SDL_CONVERTERS_HPP

#include <SDL.h>

namespace boost { namespace gil { namespace sdl { 

inline
bgra8_view_t wrap_sdl_image( SDL_Surface* screen )
{
   return interleaved_view( screen->w
                          , screen->h
                          , (bgra8_pixel_t*) screen->pixels
                          , screen->pitch   );
}

} } } // namespace boost::gil::sdl

#endif // GIL_SDL_CONVERTERS_HPP
