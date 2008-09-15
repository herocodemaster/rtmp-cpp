// Copyright 2007 Christian Henning.
// Distributed under the Boost Software License, Version 1.0. (See
// accompanying file LICENSE_1_0.txt or copy at
// http://www.boost.org/LICENSE_1_0.txt)

/*************************************************************************************************/

#ifndef SDL_WINDOW_BASE_HPP
#define SDL_WINDOW_BASE_HPP

#include <boost/scoped_ptr.hpp>

#include <boost/thread/thread.hpp>
#include <boost/thread/mutex.hpp>

#include <SDL.h>

#include "sdl_events.hpp"
#include "message_queue.h"

namespace boost { namespace gil { namespace sdl { 

typedef detail::sdl_event_base sdl_event_t;
typedef ogx::message_queue< sdl_event_t > queue_t;

class sdl_window_base
{
protected:

   sdl_window_base( int width
                  , int height )
   : _queue( NULL )
   , _surface_init( false )
   , _width ( width  )
   , _height( height )
   {
      if( !_surface_init )
      {
         _screen = SDL_SetVideoMode( _width
                                 , _height
                                 , 32
                                 , SDL_SWSURFACE );

         if( _screen == NULL )
         {
            throw std::runtime_error( "Couldn't create SDL window" );
         }

         _surface_init = true;
      }

   }

   void lock()
   {
      if( _screen && SDL_MUSTLOCK( _screen ))
      {
         if( SDL_LockSurface( _screen ) < 0 ) 
         {
            return;
         }
      }
   }

   void unlock()
   {
      if( _screen && SDL_MUSTLOCK( _screen )) 
      {
         SDL_UnlockSurface( _screen );
      }

      // Tell SDL to update the whole screen
      SDL_UpdateRect( _screen
                    , 0
                    , 0
                    , _width
                    , _height );
   }

   queue_t* get_queue() { return _queue; }

private:

   void set_queue( queue_t& queue )
   {
      _queue = &queue;
   }

   friend class sdl_service;

protected:

   bool _surface_init;

   int _width;
   int _height;

   SDL_Surface* _screen;

private:

   queue_t* _queue;
};

} } } // namespace boost::gil::sdl

#endif // SDL_WINDOW_BASE_HPP
