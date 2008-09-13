#include <gtk/gtk.h>
#include <gdk/gdkx.h>
#include <stdio.h>
#include <sys/types.h>
#include <sys/stat.h>
#include <fcntl.h>
#include <libflv.h>

static int is_recording;
static GdkWindow *window;
static GdkPixbuf *pixbuf;
static GdkScreen *screen = NULL;
static int timeStamp;
static FLVStream *flv;
static ScreenVideo *video;

#define FRAMERATE 3

static void destroy( GtkWidget *widget,
                     gpointer   data )
{
    gtk_main_quit ();
}


#define MASK (ButtonPressMask | ButtonReleaseMask)

static GdkNativeWindow select_window(GdkScreen *screen) {
	Display    *x_dpy;
	Cursor      x_cursor;
	XEvent      x_event;
	Window      x_win = None;
	Window      x_root;
	gint        x_scr;
	gint        status;
	gint        buttons = 0;

	x_dpy = GDK_SCREEN_XDISPLAY (screen);
	x_scr = GDK_SCREEN_XNUMBER (screen);
	
	x_root   = RootWindow (x_dpy, x_scr);
  	x_cursor = XCreateFontCursor (x_dpy, GDK_CROSSHAIR);

	status = XGrabPointer (x_dpy, x_root, False,
			MASK, GrabModeSync, GrabModeAsync,
			x_root, x_cursor, CurrentTime);

	if (status != GrabSuccess) {
		printf("!GrabSucces\n");
		return 0;
	}
	
	while ( (x_win == None) || (buttons != 0))
    	{
      		XAllowEvents (x_dpy, SyncPointer, CurrentTime);
      		XWindowEvent (x_dpy, x_root, MASK, &x_event);

      		switch (x_event.type)
       		{
        		case ButtonPress:
          			if (x_win == None)
            			{
              				x_win = x_event.xbutton.subwindow;
	              			if (x_win == None)
        	        			x_win = x_root;
            			}
          			buttons++;
          			break;

	        	case ButtonRelease:
        	  		if (buttons > 0)
            				buttons--;
          			break;

        	}
	}
	
	XUngrabPointer (x_dpy, CurrentTime);
	XFreeCursor (x_dpy, x_cursor);

  	return x_win;
}


gint capture() {
	gint x,y;
	GdkRectangle rect, screen_rect;
	int n_channels, rowstride;
	struct PixelData pixelData;
	int frame;

	if (!window)
    	{
      		printf("Specified window not found\n");
      		return FALSE;
    	}
	
	screen_rect.x      = 0;
 	screen_rect.y      = 0;
	screen_rect.width  = gdk_screen_get_width (screen);
	screen_rect.height = gdk_screen_get_height (screen);
	gdk_window_get_origin(window, &x, &y);

	rect.x = x;
	rect.y = y;
	gdk_drawable_get_size (GDK_DRAWABLE (window), &rect.width, &rect.height);

	
	if (! gdk_rectangle_intersect (&rect, &screen_rect, &rect))
	    return FALSE;

	pixbuf = gdk_pixbuf_get_from_drawable (NULL, window,
                                             NULL,
                                             rect.x - x,  rect.y - y , 0, 0,
                                             rect.width, rect.height);

	n_channels = gdk_pixbuf_get_n_channels (pixbuf);
	rowstride =  gdk_pixbuf_get_rowstride(pixbuf);

  	g_assert (gdk_pixbuf_get_colorspace (pixbuf) == GDK_COLORSPACE_RGB);
  	g_assert (gdk_pixbuf_get_bits_per_sample (pixbuf) == 8);
	g_assert (n_channels >= 3);

	/* preparing pixeldata */
	pixelData.width = gdk_pixbuf_get_width (pixbuf);
	pixelData.height = gdk_pixbuf_get_height (pixbuf);
	pixelData.data = gdk_pixbuf_get_pixels (pixbuf);
	pixelData.rowOrder = TOPDOWN;
	pixelData.n_channels = n_channels;
	pixelData.rowPadding = rowstride - pixelData.width * n_channels;

	frame = ScreenVideo_addFrame(video, flv, &pixelData, timeStamp);
	
	g_object_unref(pixbuf);	
	timeStamp += 1000 / FRAMERATE;

	return TRUE;
}


gint next_capture(gpointer data) {
	if (is_recording) 
		return capture();
	
	else
		return FALSE;
}


void stop_recording(GtkWidget *widget, gpointer data) {
	is_recording = 0;
	FLVStream_write(flv, "/tmp/test.flv");
}

void start_recording(GtkWidget *widget, gpointer data) {
	GdkNativeWindow native;
	GdkDisplay *display;
	GdkRectangle rect;

	if(is_recording)
		return;

	screen = gtk_widget_get_screen(widget);
	if (screen == NULL) {
		screen = gdk_screen_get_default ();
	}
	native = select_window(screen);
	display = gdk_screen_get_display (screen);
	window = gdk_window_foreign_new_for_display (display, native);
	
	gdk_drawable_get_size (GDK_DRAWABLE (window), &rect.width, &rect.height);
	
	/* creating flv und screenVideo instances */
	flv = newFLVStream(FLVVERSION_1, FLVFLAG_VIDEO);
	video = newScreenVideo(rect.width, rect.height, 64);
 	printf("video width: %i, height: %i\n", rect.width, rect.height);

	/* capture timer */
	g_timeout_add ( 1000 / FRAMERATE,  next_capture, NULL);
	is_recording = 1;
}	

int main( int   argc,
          char *argv[] )
{

    GtkWidget *window;
    GtkWidget *b_start, *b_stop, *box;
    
        
    gtk_init (&argc, &argv);
    is_recording = 0;
    box = gtk_hbox_new(FALSE, 0);
    
    window = gtk_window_new (GTK_WINDOW_TOPLEVEL);
        
    g_signal_connect (G_OBJECT (window), "destroy",
		      G_CALLBACK (destroy), NULL);
    
    gtk_container_set_border_width (GTK_CONTAINER (window), 10);
   
	
    
    b_start = gtk_button_new_with_label ("capture");
    b_stop = gtk_button_new_with_label ("stop");
   
    gtk_box_pack_start (GTK_BOX (box), b_start, TRUE, TRUE, 0);
    gtk_box_pack_start (GTK_BOX (box), b_stop, TRUE, TRUE, 0);
    
    
    g_signal_connect (G_OBJECT (b_start), "clicked",
		      G_CALLBACK (start_recording), NULL);
    
    g_signal_connect (G_OBJECT (b_stop), "clicked",
		    G_CALLBACK (stop_recording), NULL);
    
    /*
    g_signal_connect_swapped (G_OBJECT (button), "clicked",
			      G_CALLBACK (gtk_widget_destroy),
                              G_OBJECT (window));
    
    */
    gtk_container_add (GTK_CONTAINER (window), box);
    
    gtk_widget_show (b_start);
    gtk_widget_show (b_stop);
    gtk_widget_show (box);
    gtk_widget_show (window);
    
    gtk_main ();
    
    return 0;
}
