/****************************************************************************
 *   Copyright (C) 2005 by Oliver Kowalke                                   *
 *   oliver.kowalke@gmx.de                                                  *
 *                                                                          *
 *  This library is free software; you can redistribute it and/or           *
 *  modify it under the terms of the GNU Lesser General Public              *
 *  License as published by the Free Software Foundation; either            *
 *  version 2.1 of the License, or (at your option) any later version.      *
 *                                                                          *
 *  This library is distributed in the hope that it will be useful,         *
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of          *
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU       *
 *  Lesser General Public License for more details.                         *
 *                                                                          *
 *  You should have received a copy of the GNU Lesser General Public        *
 *  License along with this library; if not, write to the Free Software     *
 *  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA *
 ****************************************************************************/
#ifndef OGX_MESSAGE_QUEUE_H
#define OGX_MESSAGE_QUEUE_H

#include <queue>

#include <boost/bind.hpp>
#include <boost/shared_ptr.hpp>
#include <boost/thread/condition.hpp>
#include <boost/thread/mutex.hpp>

namespace ogx
{
	template
	<
		typename T,
		typename Q = std::priority_queue< boost::shared_ptr< T > >
	>
	class message_queue
	{
	public:
		typedef	Q								queue_type;
		typedef boost::shared_ptr< T >			msg_type;
		typedef typename queue_type::size_type	size_type;
		
	private:
		queue_type				queue_;
		boost::mutex			mtx_;
		boost::condition		not_empty_cond_;
		boost::condition		not_full_cond_;
		size_type				high_water_mark_;
		size_type				low_water_mark_;
		bool					active_;
		static const size_type	infinity_;
		
		void enqueue_( msg_type const& msg)
		{ queue_.push( msg); }
		
		void dequeue_( msg_type & msg)
		{
			msg = queue_.top();
			queue_.pop();
		}
		
		bool full_() const
		{
			if ( high_water_mark_ == infinity_) return false;
			return queue_.size() > high_water_mark_;
		}
		
		bool empty_() const
		{ return queue_.empty(); }
		
		bool suppliers_activate_() const
		{ return active_ == false || ! full_(); }
		
		bool consumers_activate_() const
		{ return active_ == false || ! empty_(); }
		
		void activate_()
		{
			active_ = true;
		}
		
		void deactivate_()
		{
			if ( active_)
			{
				active_ = false;
				not_empty_cond_.notify_all();
				not_full_cond_.notify_all();
			}
		}
		
		void flush_()
		{ queue_.clear(); }
		
	public:
		message_queue()
		: 
		queue_(), 
		mtx_(), 
		not_empty_cond_(), 
		not_full_cond_(), 
		high_water_mark_( infinity_),
		low_water_mark_( infinity_),
		active_( true)
		{}
		
		message_queue( size_type water_mark)
		: 
		queue_(), 
		mtx_(), 
		not_empty_cond_(), 
		not_full_cond_(), 
		high_water_mark_( water_mark),
		low_water_mark_( water_mark),
		active_( true)
		{}
		
		message_queue( 
			size_type low_water_mark, 
			size_type high_water_mark)
		: 
		queue_(), 
		mtx_(), 
		not_empty_cond_(), 
		not_full_cond_(), 
		high_water_mark_( high_water_mark),
		low_water_mark_( low_water_mark),
		active_( true)
		{}
		
      std::size_t size()
      {
         return queue_.size();
      }


		void activate()
		{
			typename boost::mutex::scoped_lock lock( mtx_);
			activate_();
		}
		
		void deactivate()
		{
			typename boost::mutex::scoped_lock lock( mtx_);
			deactivate_();
		}
		
		bool empty()
		{ 
			typename boost::mutex::scoped_lock lock( mtx_);
			return empty_(); 
		}
		
		bool full()
		{
			typename boost::mutex::scoped_lock lock( mtx_);
			return full_();
		}
		
		bool enqueue( msg_type const& msg)
		{
			typename boost::mutex::scoped_lock lock( mtx_);
			if ( active_ == false) return false;
			not_full_cond_.wait( 
				lock,
				boost::bind(
					& message_queue< T, Q >::suppliers_activate_,
					this) );
			if ( active_ != false)
			{
				enqueue_( msg);
				not_empty_cond_.notify_one();
				return true;
			}
			else
				return false;
		}
		
		bool dequeue( msg_type & msg)
		{
			typename boost::mutex::scoped_lock lock( mtx_);
			if ( active_ == false && empty_() ) return false;
			not_empty_cond_.wait( 
				lock,
				boost::bind(
					& message_queue< T, Q >::consumers_activate_,
					this) );
 			if ( empty_() )
 				msg.reset();
 			else
				dequeue_( msg);
 			if ( active_ == true && queue_.size() <= low_water_mark_)
				not_full_cond_.notify_one();
			return msg ? true : false;
		}
		
		void flush()
		{
			typename boost::mutex::scoped_lock lock( mtx_);
			flush_();
		}
		
		void close()
		{
			typename boost::mutex::scoped_lock lock( mtx_);
			deactivate_();
			flush_();
		}
	};

	template
	<
		typename T,
		typename Q
	>
	const typename message_queue< T, Q >::size_type
	message_queue< T, Q >::infinity_ = -1;
};

#endif // OGX_MESSAGE_QUEUE_H
