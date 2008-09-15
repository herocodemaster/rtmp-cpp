// Copyright 2007 Christian Henning.
// Distributed under the Boost Software License, Version 1.0. (See
// accompanying file LICENSE_1_0.txt or copy at
// http://www.boost.org/LICENSE_1_0.txt)

/*************************************************************************************************/

#ifndef USER_EVENTS_HPP
#define USER_EVENTS_HPP

namespace boost { namespace gil { namespace sdl { namespace detail {

struct default_keyboard_event_handler
{
   // Return true to trigger redraw.
   bool key_up() { return false; }
};

struct default_redraw_event_handler
{
   void redraw( const bgra8_view_t& sdl_view ) {}
};

struct default_timer_event_handler
{
   // Return true to trigger redraw.
   bool time_elapsed() { return false; }
};


struct default_quit_event_handler
{
   void quit() {}
};

} } } } // namespace boost::gil::sdl::detail

#endif // GIL_SDL_CONVERTERS_HPP
