// Camara Vision
//
// Copyright © Andrew Kirillov, 2005-2006
// andrew.kirillov@gmail.com
//

namespace pixord
{
	using System;
	using videosource;
	using multisource;
	using jpeg;

	/// <summary>
	/// Pixord network cameras
	/// </summary>
	public class PixordCamera : MultimodeVideoSource
	{
		private string	server;
		private short	channel = 1;
		private string	resolution = "sif";
		private int		frameInterval;

		// Constructor
		public PixordCamera()
		{
			videoSource = new JPEGSource();
			streamType = StreamType.Jpeg;
		}

		// StreamType property
		public override StreamType StreamType
		{
			get { return base.StreamType; }
			set
			{
				if ((value != StreamType.Jpeg) &&
					(value != StreamType.MJpeg))
					throw new ArgumentException("Invalid stream type");

				base.StreamType = value;
			}
		}
		// VideoSource property
		public override string VideoSource
		{
			get { return server; }
			set
			{
				server = value;
				UpdateVideoSource();
			}
		}
		// Resolution property
		public string Resolution
		{
			get { return resolution; }
			set
			{
				resolution = value;
				UpdateVideoSource();
			}
		}
		// FrameInterval property - interval between frames
		public int FrameInterval
		{
			get { return frameInterval; }
			set
			{
				frameInterval = value;

				if (streamType == StreamType.Jpeg)
				{
					((JPEGSource) videoSource).FrameInterval = frameInterval;
				}
				else
				{
					UpdateVideoSource();
				}
			}
		}


		// Update video source
		protected override void UpdateVideoSource()
		{
			switch (streamType)
			{
				case StreamType.Jpeg:
					videoSource.VideoSource = "http://" + server + "/images" + channel.ToString() + resolution;
					break;
				case StreamType.MJpeg:
				{
					string src = "http://" + server + "/getimage?camera=" + channel.ToString() + "&fmt=" + resolution;

					if (frameInterval > 0)
					{
						src += "&delay=" + ((int)(frameInterval / 10)).ToString();
					}
					videoSource.VideoSource = src;
					break;
				}
			}
		}
	}
}
