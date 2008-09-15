FILES = capture.c 
OBJECTS = capture.o

LIBDIR = /usr/lib
INCLUDEDIR = /usr/include

CFLAGS = -O3 -Wall `pkg-config --cflags gtk+-2.0` -I.

%.o: %.c
	$(CC) $(CFLAGS) -c -o $@ $<

all:  $(OBJECTS)
	$(CC) $(CFLAGS) $(OBJECTS) -lflv `pkg-config --libs gtk+-2.0` -o capture

clean: 
	rm $(OBJECTS)
	rm capture
