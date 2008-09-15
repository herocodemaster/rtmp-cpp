#pragma once //TODO: cambiar

#include <climits>					// for CHAR_BIT
#include <boost/cstdint.hpp>		// for boost::uint16_t, etc.


template<std::size_t K>
class Integer;


template<>
class Integer<8>
{
public:
	Integer(boost::uint8_t data)
		: data_(data)
	{}


	void getByteArray(unsigned char *&value)
	{
		unsigned char temp[1] = { data_ };
		value = temp;
	}

	boost::uint8_t toInteger()
	{
		return this->data_;
	}

	unsigned short getByteLenght()
	{
		return sizeof(data_); //1
	}

private:
	boost::uint8_t data_;
	//unsigned char data_[1];
};


template<>
class Integer<16>
{
public:
	Integer(boost::uint16_t data)
		: data_(data)
	{}

	void getByteArray(unsigned char *&value)
	{
		//value = this->data_;
		//data_[0] = data & 0x00FF;
		//data_[1] = (data>>8);

		unsigned char temp[2] = { data_, data_ };
		value = temp;
	}


	boost::uint16_t native_to_big ( boost::uint16_t  x )
	{
		boost::uint16_t  temp;
		unsigned char *	tp = reinterpret_cast<unsigned char *>( &temp );

		for ( std::size_t i = sizeof(x) ; i > 0 ; --i )
		{
			tp[ i - 1 ] = static_cast<unsigned char>( x );
			x >>= CHAR_BIT;
		}

		return temp;
	}

	boost::uint16_t toInteger()
	{
		return this->data_;
	}

	unsigned short getByteLenght()
	{
		return sizeof(data_);
	}

private:
	boost::uint16_t data_;
	//unsigned char data_[2];
};


//// Class declarations
//template<bool C>
//class _name { };
//
//template<>
//class _name<true> {
//public:
//    static inline void f()
//    { }         // true case
//};
//
//template<>
//class _name<false> {
//public:
//    static inline void f()
//    { }         // false case
//};
//
//// Replacement for 'if/else' statement:
////_name<condition>::f();
//
