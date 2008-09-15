/*
    Copyright 2005-2007 Adobe Systems Incorporated
   
    Use, modification and distribution are subject to the Boost Software License,
    Version 1.0. (See accompanying file LICENSE_1_0.txt or copy at
    http://www.boost.org/LICENSE_1_0.txt).

    See http://opensource.adobe.com/gil for most recent version including documentation.
*/

/*************************************************************************************************/

/// \file
/// \brief Example file to demonstrate a way to compute histogram
/// \author Lubomir Bourdev and Hailin Jin
/// \date February 27, 2007

#include <algorithm>
#include <fstream>
#include <boost/gil/image.hpp>
#include <boost/gil/typedefs.hpp>
#include <boost/gil/color_convert.hpp>
#include <boost/gil/extension/io/jpeg_io.hpp>

using namespace boost::gil;

template <typename GrayView, typename R>
void gray_image_hist(const GrayView& img_view, R& hist) 
{
//    for_each_pixel(img_view,++lambda::var(hist)[lambda::_1]);
    for (typename GrayView::iterator it=img_view.begin(); it!=img_view.end(); ++it)
	{
        ++hist[*it];
	}
}

template <typename V, typename R>
void get_hist(const V& img_view, R& hist) 
{
    gray_image_hist(color_converted_view<gray8_pixel_t>(img_view), hist);
}

int main() 
{
    rgb8_image_t img;
    jpeg_read_image("test.jpg",img);

    int histogram[256];
    std::fill(histogram,histogram+256,0);
    get_hist(const_view(img),histogram);

    std::fstream histo_file("out-histogram.txt",std::ios::out);
    for(std::size_t ii=0;ii<256;++ii)
	{
        histo_file << histogram[ii] << std::endl;
	}
    histo_file.close();

    return 0;
}




//#include <iostream>
//#include "Integer.hpp"
//#include "boost/spirit/home/support/detail/integer/endian.hpp"
//
//int main()
//{
//
//	boost::integer::endian<boost::integer::big, int, 8, boost::integer::unaligned> pepe;
//	
//
//
//	
//
//	//Integer<38> integer38; //No hay constructor ni ninguna otra funcion porque 38 no es una posibilidad
//
//	Integer<8> integer8(65);
//	Integer<16> integer16(65);
//	
//	int i8 = integer8.toInteger();
//	int i16 = integer16.toInteger();
//
//	int i16_b = integer16.native_to_big(i16);
//
//	unsigned char* uc = 0;
//	integer8.getByteArray(uc);
//	std::cout << uc << std::endl;
//	integer16.getByteArray(uc);
//	std::cout << uc << std::endl;
//
//
//
//
//
//	std::cout << i8 << std::endl;
//	std::cout << integer8.getByteLenght() << std::endl;
//	std::cout << "press ENTER to finish..." << std::endl;
//
//	std::cin.sync();
//	std::cin.get();
//	return 0;
//}