// Copyright 2007 Christian Henning.
// Distributed under the Boost Software License, Version 1.0. (See
// accompanying file LICENSE_1_0.txt or copy at
// http://www.boost.org/LICENSE_1_0.txt)

/*************************************************************************************************/

#ifndef SDL_WINDOW_HPP
#define SDL_WINDOW_HPP

#include <boost/scoped_ptr.hpp>

#include <boost/thread/thread.hpp>
#include <boost/thread/mutex.hpp>

#include <SDL.h>

#include "sdl_events.hpp"
#include "sdl_timer.hpp"
#include "sdl_window_base.hpp"
#include "user_events.hpp"
#include "message_queue.h"

namespace boost { namespace gil { namespace sdl { 

typedef detail::sdl_event_base sdl_event_t;

// active object for displaying images

template < typename KEYBOARD_EVENTS = detail::default_keyboard_event_handler
         , typename REDRAW_EVENT    = detail::default_redraw_event_handler
         , typename TIMER_EVENT     = detail::default_timer_event_handler
         , typename QUIT_EVENT      = detail::default_quit_event_handler
         >
class sdl_window : virtual public sdl_window_base
                 , public detail::sdl_timer_base< TIMER_EVENT
                                                , REDRAW_EVENT >
                 , public KEYBOARD_EVENTS
                 , public QUIT_EVENT
{
   typedef shared_ptr<REDRAW_EVENT> redraw_handler_t;

public:

   sdl_window( int width
             , int height
             , redraw_handler_t rh = redraw_handler_t( new REDRAW_EVENT() ))
   : sdl_window_base( width
                    , height )
   , detail::sdl_timer_base< TIMER_EVENT
                           , REDRAW_EVENT>( width
                                          , height
                                          , rh     )

   , _redraw_handler( rh )
   , _cancel( false )
   {
      _thread.reset( new boost::thread( boost::bind( &boost::gil::sdl::sdl_window< KEYBOARD_EVENTS
                                                                                 , REDRAW_EVENT
                                                                                 , TIMER_EVENT 
                                                                                 , QUIT_EVENT >::_run
                                                   , this )));
   }

   ~sdl_window()
   {
      cancel();
      _thread->join();
   }

   void cancel()
   {
      boost::mutex::scoped_lock oLock( _sentinel );

      _cancel = true;
      std::cout << "cancel is true." << std::endl;
   }

private:

   void _run()
   {
      boost::shared_ptr< sdl_event_t > e;

      while( _cancel == false )
      {
         if( get_queue() )
         {
            std::cout << "before get event." << std::endl;
            get_queue()->dequeue( e );
            std::cout << "after get event." << std::endl;

            switch( e->type() )
            {
               case detail::redraw_event::type:
               {
                  lock();

                  _redraw_handler->redraw( wrap_sdl_image( _screen ));

                  unlock();

                  break;
               }

               case detail::key_up_event::type:
               {
                  lock();

                  if( key_up() == true )
                  {
                     _redraw_handler->redraw( wrap_sdl_image( _screen ));
                  }

                  unlock();

                  break;
               }

               case detail::quit_event::type:
               {
                  std::cout << "received quit event." << std::endl;

                  quit();
               }
            }
         }
      }


      std::cout << "thread main is done." << std::endl;
   }

private:

   typedef boost::scoped_ptr<boost::thread> thread_t;
   thread_t _thread;

   mutable boost::mutex _sentinel;

   redraw_handler_t _redraw_handler;

   bool _cancel;
};


} } } // namespace boost::gil::sdl

#endif // SDL_WINDOW_HPP
