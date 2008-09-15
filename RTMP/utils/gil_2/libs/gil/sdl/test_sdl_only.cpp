// test.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

#include <SDL.h>


//#include "boost/gil/extension/sdl/sdl_wrapper.hpp"

using namespace std;
using namespace boost;
using namespace gil;

SDL_Surface* _screen;

const int _width  = 640;
const int _height = 480;


void _render()
{
   // Lock surface if needed
   if( SDL_MUSTLOCK( _screen ))
   {
      if( SDL_LockSurface( _screen ) < 0 ) 
      {
         return;
      }
   }

   // Ask SDL for the time in milliseconds
   int tick = SDL_GetTicks();

   // Declare a couple of variables
   int i, j, yofs, ofs;

   // Draw to screen
   yofs = 0;
   for (i = 0; i < _height; i++)
   {
      for (j = 0, ofs = yofs; j < _width; j++, ofs++)
      {
         ((unsigned int*) _screen->pixels)[ofs] = i * j + i * j + tick;
      }

      yofs += _screen->pitch / 4;
   }

   // Unlock if needed
   if( SDL_MUSTLOCK( _screen )) 
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

int main( int argc, char* argv[] )
{
   // Initialize SDL's subsystems - in this case, only video.
   if ( SDL_Init(SDL_INIT_VIDEO) < 0 ) 
   {
      std::string error( "Unable to init SDL: " );
      error += SDL_GetError();
      cout << error << endl;;

      return 1;
   }

   _screen = SDL_SetVideoMode( _width
                             , _height
                             , 0
                             , SDL_ANYFORMAT );

   if( _screen == NULL )
   {
      cout << "Couldn't create SDL window" << endl;
   }

   // Poll for events, and handle the ones we care about.
   SDL_Event event;

   while( true )
   {
      // Render stuff
      _render();

      while( SDL_PollEvent( &event ))
      {
         cout << "event" << endl;

         switch (event.type) 
         {
            case SDL_KEYDOWN:
            {
               break;
            }

            case SDL_KEYUP:
            {
               // If escape is pressed, return (and thus, quit)
               if( event.key.keysym.sym == SDLK_ESCAPE )
                  return 0;

               break;
            }

            case SDL_QUIT:
            {
               return 0;
            }

         } //switch
      } // while
   } // while

	return 0;
}

