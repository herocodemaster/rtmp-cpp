FILES = dump.c 
OBJECTS = dump.o

LIBDIR = /usr/lib
INCLUDEDIR = /usr/include

CFLAGS = -O3 -Wall -I.

%.o: %.c
	$(CC) $(CFLAGS) -c -o $@ $<

all:  $(OBJECTS)
	$(CC) $(CFLAGS) $(OBJECTS) -lflv -o dump

clean: 
	rm $(OBJECTS)
	rm dump
