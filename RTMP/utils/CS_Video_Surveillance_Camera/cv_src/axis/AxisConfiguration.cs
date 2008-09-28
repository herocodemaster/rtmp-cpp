// Camara Vision
//
// Copyright © Andrew Kirillov, 2005-2006
// andrew.kirillov@gmail.com
//

namespace axis
{
	using System;
	using videosource;

	/// <summary>
	/// AxisConfiguration
	/// </summary>
	public class AxisConfiguration
	{
		public string	source;
		public string	login;
		public string	password;
		public int		frameInterval = 0;
		public StreamType	stremType = StreamType.Jpeg;

		public short	camera = 1;
		public string	resolution;
	}
}
