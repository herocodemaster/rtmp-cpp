// Copyright 2007 Christian Henning.
// Distributed under the Boost Software License, Version 1.0. (See
// accompanying file LICENSE_1_0.txt or copy at
// http://www.boost.org/LICENSE_1_0.txt)

/*************************************************************************************************/

#ifndef SDL_TIMER_HPP
#define SDL_TIMER_HPP

#include <boost/shared_ptr.hpp>

#include <SDL.h>

#include "sdl_window_base.hpp"
#include "user_events.hpp"

namespace boost { namespace gil { namespace sdl { namespace detail {

template< typename TIMER_HANDLER  = detail::default_timer_event_handler
        , typename REDRAW_HANDLER = detail::default_timer_event_handler
        >
class sdl_timer_base : virtual public sdl_window_base
                     , public TIMER_HANDLER
{
private:

   typedef boost::shared_ptr<REDRAW_HANDLER> redraw_handler_t;

public:

   sdl_timer_base( int width
                 , int height
                 , redraw_handler_t redraw_handler )
   : sdl_window_base( width, height )
   , _redraw_handler( redraw_handler )
   , _timer_id( 0 )
   , _timer_init( false )
   {}

   ~sdl_timer_base()
   {
      if( _timer_init )
      {
         SDL_RemoveTimer( _timer_id );

         std::cout << "timer is removed." << std::endl;
      }
   }


   // interval in ms
   void set_timer( unsigned int interval )
   {
      if( _timer_init == false )
      {
         _timer_id = SDL_AddTimer( interval
                                 , &sdl_timer_base::timer_callback
                                 , this                              );

         char* error = SDL_GetError();

         _timer_init = true;
      }
      else
      {
         throw std::runtime_error( "Timer is already running." );
      }   
   }

private:

   static Uint32 timer_callback( Uint32 interval, void *param )
   {
      sdl_timer_base* p = (sdl_timer_base*) param;

      if( p && p->time_elapsed() )
      {
         p->lock();

         if( p->_screen->pixels == NULL )
            return 0;
         p->_redraw_handler->redraw( wrap_sdl_image( p->_screen ));
         p->unlock();
      }

      return interval;
   }

private:

   bool _timer_init;

   SDL_TimerID _timer_id;

   redraw_handler_t _redraw_handler;

};

} } } } // namespace boost::gil::sdl::detail

#endif //SDL_TIMER_HPP
