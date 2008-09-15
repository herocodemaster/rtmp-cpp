// image_test.cpp : Defines the entry point for the console application.
//

#include <boost/mpl/vector_c.hpp>

#include <boost/gil/gil_all.hpp>

#include <boost/gil/extension/io/io_base.hpp>
#include <boost/gil/extension/io/bmp_dynamic_io.hpp>
#include <boost/gil/extension/io/pnm_dynamic_io.hpp>

using namespace boost;
using namespace gil;
using namespace std;

void main() 
{
    const string in_dir="";  // directory of source images
    const std::string out_dir=in_dir+"image_io-out/";

// *********************************** 
// ************************ BMP Test
// *********************************** 

///////////////////
// 1-bit images
///////////////////
   {
      // black and white palette ( #000000, #FFFFFF )

      gray8_image_t image;
      bmp_read_image( in_dir+"g01bw.bmp", image);

      bmp_write_view( out_dir+"g01bw.bmp", view( image ));

      gray8_image_t image_2( image.dimensions() );
      bmp_read_view( out_dir+"g01bw.bmp", view( image_2 ));

      assert( image == image_2 );
   }

   {
      // white and black palette (#FFFFFF,#000000). Should look the same as g01bw, not inverted.

      gray8_image_t image;
      bmp_read_image( in_dir+"g01wb.bmp", image);

      bmp_write_view( out_dir+"g01wb.bmp", view( image ));
   }

   {
      // blue and green palette (#4040FF,#40FF40)

      rgb8_image_t image;
      bmp_read_image( in_dir+"g01bg.bmp", image);

      bmp_write_view( out_dir+"g01bg.bmp", view( image ));
   }

   {
      // blue and green palette (#4040FF,#40FF40)
      rgb8_image_t image;
      bmp_read_image( in_dir+"g01p1.bmp", image);

      bmp_write_view( out_dir+"g01p1.bmp", view( image ));
   }

///////////////////
// 4-bit images
///////////////////

   {
      // basic 4bpp (16 color) image

      rgb8_image_t image;
      bmp_read_image( in_dir+"g04.bmp", image);

      bmp_write_view( out_dir+"g04.bmp", view( image ));
   }

   {
      /* not supported, yet
      // RLE compressed

      rgb8_image_t image;
      bmp_read_image( in_dir+"g04rle.bmp", image);

      bmp_write_view( out_dir+"g04rle.bmp", view( image ));
      */
   }

   {
      // 4-color grayscale palette

      gray8_image_t image;
      bmp_read_image( in_dir+"g04p4.bmp", image);

      bmp_write_view( out_dir+"g04p4.bmp", view( image ));
   }

///////////////////
// 8-bit images
///////////////////
   {
      // basic 8bpp (256 color) image

      rgb8_image_t image;
      bmp_read_image( in_dir+"g08.bmp", image);

      bmp_write_view( out_dir+"g08.bmp", view( image ));
   }

   {
      // biClrUsed=256, biClrImportant=0 [=256]

      rgb8_image_t image;
      bmp_read_image( in_dir+"g08p256.bmp", image);

      bmp_write_view( out_dir+"g08p256.bmp", view( image ));
   }

   {
      // biClrUsed=256, biClrImportant=256

      rgb8_image_t image;
      bmp_read_image( in_dir+"g08pi256.bmp", image);

      bmp_write_view( out_dir+"g08pi256.bmp", view( image ));
   }

   {
      // biClrUsed=256, biClrImportant=64. It's barely possible that some 
      // sophisticated viewers may display this image in grayscale,
      // if there are a limited number of colors available.

      rgb8_image_t image;
      bmp_read_image( in_dir+"g08pi64.bmp", image);

      bmp_write_view( out_dir+"g08pi64.bmp", view( image ));
   }

   {
      /* not supported, yet
      // RLE compressed.

      rgb8_image_t image;
      bmp_read_image( in_dir+"g08rle.bmp", image);

      bmp_write_view( out_dir+"g08rle.bmp", view( image ));
      */
   }

   {
      // OS/2-style bitmap. This is an obsolete variety of BMP that is still 
      // encountered sometimes. It has 3-byte palette entries (instead of 4), 
      // and 16-bit width/height fields (instead of 32).

      rgb8_image_t image;
      bmp_read_image( in_dir+"g08os2.bmp", image);

      bmp_write_view( out_dir+"g08os2.bmp", view( image ));
   }

   {
      // resolution 7874x7874 pixels/meter (200x200 dpi)

      rgb8_image_t image;
      bmp_read_image( in_dir+"g08res22.bmp", image);

      bmp_write_view( out_dir+"g08res22.bmp", view( image ));
   }

   {
      // resolution 3937x3937 pixels/meter (100x100 dpi)

      rgb8_image_t image;
      bmp_read_image( in_dir+"g08res11.bmp", image);

      bmp_write_view( out_dir+"g08res11.bmp", view( image ));
   }

   {
      // resolution 7874x3937 pixels/meter (200x100 dpi). 
      // Some programs (e.g. Imaging for Windows) may display this image 
      // stretched vertically, which is the optimal thing to do if the 
      // program is primarily a viewer, rather than an editor.

      rgb8_image_t image;
      bmp_read_image( in_dir+"g08res21.bmp", image);

      bmp_write_view( out_dir+"g08res21.bmp", view( image ));
   }

   {
      // bits size not given (set to 0). This is legal for uncompressed bitmaps.

      rgb8_image_t image;
      bmp_read_image( in_dir+"g08s0.bmp", image);

      bmp_write_view( out_dir+"g08s0.bmp", view( image ));
   }

   {
      // bfOffBits in header not set to the usual value. There are 100 extra unused 
      // bytes between palette and bits.

      rgb8_image_t image;
      bmp_read_image( in_dir+"g08offs.bmp", image);

      bmp_write_view( out_dir+"g08offs.bmp", view( image ));
   }

   {
      // size 126x63 (right and bottom slightly clipped) 

      rgb8_image_t image;
      bmp_read_image( in_dir+"g08w126.bmp", image);

      bmp_write_view( out_dir+"g08w126.bmp", view( image ));
   }

   {
      // size 125x62

      rgb8_image_t image;
      bmp_read_image( in_dir+"g08w125.bmp", image);

      bmp_write_view( out_dir+"g08w125.bmp", view( image ));
   }

   {
      // size 124x61

      rgb8_image_t image;
      bmp_read_image( in_dir+"g08w124.bmp", image);

      bmp_write_view( out_dir+"g08w124.bmp", view( image ));
   }

   {
      // 64-color grayscale palette
      gray8_image_t image;
      bmp_read_image( in_dir+"g08p64.bmp", image);

      bmp_write_view( out_dir+"g08p64.bmp", view( image ));
   }

///////////////////
// 16-bit images
///////////////////
   {
      // 15-bit color (1 bit wasted), biCompression=BI_RGB (no bitfields, defaults to 5-5-5)

      rgb8_image_t image;
      bmp_read_image( in_dir+"g16def555.bmp", image);

      bmp_write_view( out_dir+"g16def555.bmp", view( image ));
   }

   {
      // 15-bit color, biCompression=BI_BITFIELDS (bitfields indicate 5-5-5)

      rgb8_image_t image;
      bmp_read_image( in_dir+"g16bf555.bmp", image);

      bmp_write_view( out_dir+"g16bf555.bmp", view( image ));
   }

   {
      // 16-bit color, biCompression=BI_BITFIELDS (bitfields indicate 5-6-5)

      rgb8_image_t image;
      bmp_read_image( in_dir+"g16bf565.bmp", image);

      bmp_write_view( out_dir+"g16bf565.bmp", view( image ));
   }

///////////////////
// 24-bit images
///////////////////
   {
      // 24-bit color (BGR)

      rgb8_image_t image;
      bmp_read_image( in_dir+"g24.bmp", image);

      bmp_write_view( out_dir+"g24.bmp", view( image ));
   }

///////////////////
// 32-bit images
///////////////////
   {
      // 24-bit color (8 bits wasted), biCompression=BI_RGB (no bitfields, defaults to BGRx)

      rgba8_image_t image;
      bmp_read_image( in_dir+"g32def.bmp", image );

      bmp_write_view( out_dir+"g32def.bmp", view( image ));
   }

   {
      // 24-bit color (8 bits wasted), biCompression=BI_BITFIELDS (bitfields indicate BGRx)

      rgba8_image_t image;
      bmp_read_image( in_dir+"g32bf.bmp", image );

      bmp_write_view( out_dir+"g32bf.bmp", view( image ));
   }


// *********************************** 
// ************************ PNM Test
// *********************************** 
   {
      // a PBMA file
      gray8_image_t image;
      pnm_read_image( in_dir+"p1.pnm", image );

      pnm_write_view( out_dir+"p1.pnm", view( image ));

      pnm_read_image( out_dir+"p1.pnm", image );
      bmp_write_view( out_dir+"p1.pnm.bmp", view( image ));
   }

   {
      // a PGMA file
      rgb8_image_t image;
      pnm_read_image( in_dir+"p2.pnm", image );

      pnm_write_view( out_dir+"p2.pnm", view( image ));

      pnm_read_image( out_dir+"p2.pnm", image );
      bmp_write_view( out_dir+"p2.pnm.bmp", view( image ));
   }

   {
      // a PPMB file
      rgb8_image_t image;
      pnm_read_image( in_dir+"p3.pnm", image );

      pnm_write_view( out_dir+"p3.pnm", view( image ));

      pnm_read_image( out_dir+"p3.pnm", image );
      bmp_write_view( out_dir+"p3.pnm.bmp", view( image ));
   }

   {
      // a PBMB file
      rgb8_image_t image;
      pnm_read_image( in_dir+"p4.pnm", image );

      pnm_write_view( out_dir+"p4.pnm", view( image ));

      pnm_read_image( out_dir+"p4.pnm", image );
      bmp_write_view( out_dir+"p4.pnm.bmp", view( image ));
   }


   {
      // a PGMB file
      rgb8_image_t image;
      pnm_read_image( in_dir+"p5.pnm", image );

      pnm_write_view( out_dir+"p5.pnm", view( image ));

      pnm_read_image( out_dir+"p5.pnm", image );
      bmp_write_view( out_dir+"p5.pnm.bmp", view( image ));
   }

   {
      // a PBMA file
      rgb8_image_t image;
      pnm_read_image( in_dir+"p6.pnm", image );

      pnm_write_view( out_dir+"p6.pnm", view( image ));

      pnm_read_image( out_dir+"p6.pnm", image );
      bmp_write_view( out_dir+"p6.pnm.bmp", view( image ));
   }
}

