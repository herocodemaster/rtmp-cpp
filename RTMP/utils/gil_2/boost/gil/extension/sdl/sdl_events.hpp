// Copyright 2007 Christian Henning.
// Distributed under the Boost Software License, Version 1.0. (See
// accompanying file LICENSE_1_0.txt or copy at
// http://www.boost.org/LICENSE_1_0.txt)

/*************************************************************************************************/

#ifndef SDL_EVENTS_HPP
#define SDL_EVENTS_HPP

namespace boost { namespace gil { namespace sdl { namespace detail {

struct sdl_event_base
{
   unsigned int type() { return _type; }

protected:

   sdl_event_base() : _type( 0 ) {}
   sdl_event_base( int type ) : _type( type ) {}

private:

   int _type;
};

struct redraw_event : public sdl_event_base
{
   redraw_event()
   : sdl_event_base( type )
   {}

   static const int type = 1;
};

struct key_up_event : public sdl_event_base
{
   key_up_event()
   : sdl_event_base( type )
   {}

   static const int type = 2;
};

struct quit_event : public sdl_event_base
{
   quit_event()
   : sdl_event_base( type )
   {}

   static const int type = 3;
};

} } } } // namespace boost::gil::sdl::detail

#endif // SDL_EVENTS_HPP
