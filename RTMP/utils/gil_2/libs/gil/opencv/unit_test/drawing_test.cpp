#include "stdafx.h"

#include <boost\function.hpp>

#include <boost\test\unit_test.hpp>

#include <boost\gil\extension\opencv\drawing.hpp>

#include <boost\gil\extension\io_new\png_write.hpp>

using namespace boost::gil;
using namespace boost::gil::opencv;

template< typename Color
        , typename Line_Type
        >
struct draw_rectangle
{
    draw_rectangle( const boost::gil::opencv::point_t& start
                  , const boost::gil::opencv::point_t& end
                  , const Color&                       color
                  , std::size_t                        thickness
                  , const Line_Type&                   line_type )
    : _start( start )
    , _end  ( end   )
    , _color( color )
    , _thickness( thickness )
    , _line_type( line_type )
    {}

    void operator() ( ipl_image_wrapper img )
    {
        drawRectangle( img
                     , _start
                     , _end
                     , _color
                     , _thickness
                     , _line_type  );
    }

    const boost::gil::opencv::point_t _start;
    const boost::gil::opencv::point_t _end;

    const Color _color;

    const std::size_t _thickness;

    const Line_Type _line_type;
};

template< typename View >
struct draw
{
    typedef boost::function< void ( ipl_image_wrapper ) > operation_t;
    typedef std::vector< operation_t > operation_vec_t;
    operation_vec_t operations;

    void add( operation_t op )
    {
        operations.push_back( op );
    }

    void operator()( View v )
    {
        ipl_image_wrapper ipl_img = create_ipl_image( v );

        operation_vec_t::const_iterator it  = operations.begin();
        operation_vec_t::const_iterator end = operations.end();
        
        for( ; it != end ; ++it )
        {
            const operation_t& o = *it;
            o( ipl_img );
        }
    }
};


BOOST_AUTO_TEST_CASE( test_draw_rectangle )
{
    rgb8_image_t img( 640, 480 );

    fill_pixels( view( img ), rgb8_pixel_t( 0, 0, 0 ));

    typedef boost::function< void (ipl_image_wrapper) > operation_t;

/*
    draw< rgb8_view_t > d;
    d.add( draw_rectangle< rgb8_pixel_t
                         , four_connected_line
                         >( boost::gil::opencv::point_t( 300, 200 )
                         , boost::gil::opencv::point_t( 240, 100 )
                         , rgb8_pixel_t( 245, 100, 33 )
                         , 2
                         , four_connected_line()
                         ) );


    d.add( draw_rectangle< rgb8_pixel_t
                                  , four_connected_line
                                  >( boost::gil::opencv::point_t( 10, 10 )
                                   , boost::gil::opencv::point_t( 500, 100 )
                                   , rgb8_pixel_t( 245, 100, 33 )
                                   , cv_fill::type::value
                                   , four_connected_line()
                                   ) );
    d( view( img ));
*/


    drawCircle( view( img )
              , boost::gil::opencv::point_t( 250, 400 )
              , 75
              , rgb8_pixel_t( 0, 100, 88 )
              , 10
              , cv_aa()
              );

    drawLine( view( img )
            , boost::gil::opencv::point_t( 400, 400 )
            , boost::gil::opencv::point_t( 300, 200 )
            , rgb8_pixel_t( 245, 100, 33 )
            , 1
            , eight_connected_line()
            );

    drawEllipse( view( img )
               , boost::gil::opencv::point_t( 400, 400 ) // center
               , boost::gil::opencv::point_t( 50, 40 ) // axes
               , 185
               , 20
               , 90
               , rgb8_pixel_t( 25, 10, 88 )
               , 25
               , eight_connected_line()
               );

    curve_t c;
    c.push_back( boost::gil::opencv::point_t( 10,10 ));
    c.push_back( boost::gil::opencv::point_t( 10,100 ));
    c.push_back( boost::gil::opencv::point_t( 100,100 ));
    c.push_back( boost::gil::opencv::point_t( 100,10 ));

    curve_t c2;
    c2.push_back( boost::gil::opencv::point_t( 300,300 ));
    c2.push_back( boost::gil::opencv::point_t( 300,330 ));
    c2.push_back( boost::gil::opencv::point_t( 330,330 ));
    c2.push_back( boost::gil::opencv::point_t( 330,300 ));
    c2.push_back( boost::gil::opencv::point_t( 450,310 ));
    
    curve_vec_t cv;
    cv.push_back( c );
    cv.push_back( c2 );
/*
    drawPolyLine( view( img )
                , cv
                , true
                , rgb8_pixel_t( 245, 100, 33 )
                , 2
                , eight_connected_line()
                );
*/
    drawFillPoly( view( img )
                , cv
                , rgb8_pixel_t( 245, 100, 33 )
                , cv_aa()
                );

    write_view( "..\\out\\rectangle.png", view( img ), png_tag() );
}