#include "stdafx.h"

#include <iostream>

#include <boost/gil/extension/io_new/png_read.hpp>
#include <boost/gil/gil_all.hpp>

using namespace std;
using namespace boost;
using namespace gil;

int main(int argc, char *argv[])
{
    typedef bit_aligned_image3_type< 24, 24, 24, rgb_layout_t >::type image_t;
    image_t img;

   return 0;
}
