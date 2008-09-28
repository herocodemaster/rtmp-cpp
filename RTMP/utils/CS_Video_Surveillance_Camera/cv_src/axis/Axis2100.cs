// Camara Vision
//
// Copyright © Andrew Kirillov, 2005-2006
// andrew.kirillov@gmail.com
//

namespace axis
{
	using System;
	using videosource;
	using multisource;
	using jpeg;

	/// <summary>
	/// Axis2100 Network camera
	/// </summary>
	public class Axis2100 : MultimodeVideoSource
	{
		private string	server;
		private string	resolution = "320x240";
		private int		frameInterval = 0;

		// Constructor
		public Axis2100()
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
					videoSource.VideoSource = "http://" + server + "/axis-cgi/jpg/image.cgi?resolution=" + resolution;
					break;
				case StreamType.MJpeg:
				{
					string src = "http://" + server + "/axis-cgi/mjpg/video.cgi?resolution=" + resolution;

					if (frameInterval > 0)
					{
						int fps = (int)(1000 / Math.Min(frameInterval, 1000));
						src += "&des_fps=" + fps.ToString();
					}
					videoSource.VideoSource = src;
					break;
				}
			}
		}
	}
}
