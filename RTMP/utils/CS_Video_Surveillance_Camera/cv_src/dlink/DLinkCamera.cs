// Camara Vision
//
// Copyright © Andrew Kirillov, 2005-2006
// andrew.kirillov@gmail.com
//

namespace dlink
{
	using System;
	using videosource;
	using jpeg;

	/// <summary>
	/// DLink network camera
	/// </summary>
	public class DLinkCamera : JPEGSource
	{
		private string	server;

		// Constructor
		public DLinkCamera()
		{
		}

		// VideoSource property
		public override string VideoSource
		{
			get { return server; }
			set
			{
				if ((value != null) && (value != ""))
				{
					server = value;
					base.VideoSource = "http://" + server + "/cgi-bin/video.jpg";
				}
			}
		}
	}
}
