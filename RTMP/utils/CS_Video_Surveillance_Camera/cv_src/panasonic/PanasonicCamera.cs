// Camara Vision
//
// Copyright © Andrew Kirillov, 2005-2006
// andrew.kirillov@gmail.com
//

namespace panasonic
{
	using System;
	using videosource;
	using multisource;
	using jpeg;

	/// <summary>
	/// Panasonic network cameras
	/// </summary>
	public class PanasonicCamera : MultimodeVideoSource
	{
		private string	server;
		private string	resolution = "320x240";
		private string	quality = "Standard";
		private int		frameInterval;

		// Constructor
		public PanasonicCamera()
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
		// Quality property
		public string Quality
		{
			get { return quality; }
			set
			{
				quality = value;
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
			}
		}


		// Update video source
		protected override void UpdateVideoSource()
		{
			switch (streamType)
			{
				case StreamType.Jpeg:
					videoSource.VideoSource = "http://" + server + "/SnapshotJPEG?Resolution=" + resolution + "&Quality=" + quality;
					break;
				case StreamType.MJpeg:
					videoSource.VideoSource = "http://" + server + "/nphMotionJpeg?Resolution=" + resolution + "&Quality=" + quality;
					break;
			}
		}
	}
}
