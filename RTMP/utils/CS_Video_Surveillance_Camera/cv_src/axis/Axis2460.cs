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
	using mjpeg;

	/// <summary>
	/// Axis2460 Network DVR source
	/// </summary>
	public class Axis2460 : MultimodeVideoSource
	{
		private string	server;
		private short	camera = 1;
		private int		frameInterval = 0;

		// Constructor
		public Axis2460()
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
		// Camera property
		public short Camera
		{
			get { return camera; }
			set
			{
				if ((value >= 1) && (value <= 4))
				{
					camera = value;
					UpdateVideoSource();
				}
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
		// SeparateConnectioGroup property
		// indicates to open WebRequest in separate connection group
		public bool	SeparateConnectionGroup
		{
			get
			{
				return (streamType != StreamType.MJpeg) ? false : (((MJPEGSource) videoSource).SeparateConnectionGroup);
			}
			set
			{
				if (streamType == StreamType.MJpeg)
				{
					((MJPEGSource) videoSource).SeparateConnectionGroup = value;
				}
			}
		}


		// Update video source
		protected override void UpdateVideoSource()
		{
			switch (streamType)
			{
				case StreamType.Jpeg:
					videoSource.VideoSource = "http://" + server + "/axis-cgi/jpg/image.cgi?camera=" + camera.ToString();
					break;
				case StreamType.MJpeg:
				{
					string src = "http://" + server + "/axis-cgi/mjpg/video.cgi?camera=" + camera.ToString();

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
