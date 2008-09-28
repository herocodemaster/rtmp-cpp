// Camara Vision
//
// Copyright © Andrew Kirillov, 2005-2006
// andrew.kirillov@gmail.com
//

namespace stardot
{
	using System;
	using videosource;
	using jpeg;

	/// <summary>
	/// StardotExpress6 video server
	/// </summary>
	public class StardotExpress6 : JPEGSource
	{
		private string	server;
		private short	camera = 0;

		// Constructor
		public StardotExpress6()
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
					UpdateVideoSource();
				}
			}
		}
		// Camera property
		public short Camera
		{
			get { return camera; }
			set
			{
				if ((value >= 0) && (value <= 5))
				{
					camera = value;
					UpdateVideoSource();
				}
			}
		}

		// Update video source
		protected void UpdateVideoSource()
		{
			base.VideoSource = "http://" + server + "/jpeg.cgi?" + camera.ToString();
		}
	}
}
