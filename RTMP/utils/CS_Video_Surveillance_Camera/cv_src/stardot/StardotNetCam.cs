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
	/// StarDot NetCam network camera
	/// </summary>
	public class StardotNetCam : JPEGSource
	{
		private string	server;

		// Constructor
		public StardotNetCam()
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
					base.VideoSource = "http://" + server + "/netcam.jpg";
				}
			}
		}
	}
}
