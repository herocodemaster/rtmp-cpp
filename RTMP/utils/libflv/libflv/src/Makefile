FILES = flv.c screen.c 
OBJECTS = flv.o screen.o 

LIBDIR = /usr/lib
INCLUDEDIR = /usr/include

CFLAGS = -O3 -Wall -I.

%.o: %.c
	$(CC) $(CFLAGS) -c -o $@ $<

all:  $(OBJECTS)
	$(CC) $(CFLAGS) $(OBJECTS) -lz -shared -o libflv.so

install: all
	install -d $(LIBDIR)
	install -d $(INCLUDEDIR) 
	install -m 0644 libflv.h $(INCLUDEDIR)
	install -m 0644 libflv.so $(LIBDIR)/
	(cd $(LIBDIR) && ln -fs libflv.so libflv.so.0)

clean: 
	rm $(OBJECTS)
	rm libflv.so
