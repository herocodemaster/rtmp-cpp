// Camara Vision
//
// Copyright © Andrew Kirillov, 2005-2006
// andrew.kirillov@gmail.com
//

namespace multisource
{
	using System;
	using System.Collections;
	using videosource;
	using jpeg;
	using mjpeg;

	/// <summary>
	/// MultimodeVideoSource - abstract class for video sources, which support
	/// multiple working mode
	/// </summary>
	public abstract class MultimodeVideoSource : IVideoSource
	{
		protected IVideoSource	videoSource;
		protected StreamType	streamType;
		private ArrayList		delegates = new ArrayList();

		// New frame event
		public event CameraEventHandler NewFrame
		{
			add
			{
				videoSource.NewFrame += value;
				delegates.Add((object) value);
			}
			remove
			{
				videoSource.NewFrame -= value;
				delegates.Remove((object) value);
			}
		}

		// Constructor
		public MultimodeVideoSource()
		{
		}

		// StreamType property
		public virtual StreamType StreamType
		{
			get { return streamType; }
			set
			{
				// update stream type if video source is not running
				if ((streamType != value) && (!videoSource.Running))
				{
					streamType = value;
					
					// save credentials and user data
					object	userData = videoSource.UserData;
					string	login = videoSource.Login;
					string	password = videoSource.Password;

					// create new base video source
					switch (streamType)
					{
						case StreamType.Jpeg:
							videoSource = new JPEGSource();
							break;
						case StreamType.MJpeg:
							videoSource = new MJPEGSource();
							break;
					}

					// set credentials and user data back to the new source
					videoSource.Login		= login;
					videoSource.Password	= password;
					videoSource.UserData	= userData;

					// add delegates to NewFrame event
					foreach (object handler in delegates)
						videoSource.NewFrame += (CameraEventHandler) handler;

					UpdateVideoSource();
				}
			}
		}

		// Video source property
		public abstract string VideoSource
		{
			get;
			set;
		}

		// Login property
		public string Login
		{
			get { return videoSource.Login; }
			set { videoSource.Login = value; }
		}

		// Password property
		public string Password
		{
			get { return videoSource.Password; }
			set { videoSource.Password = value; }
		}

		// FramesReceived property
		public int FramesReceived
		{
			get { return videoSource.FramesReceived; }
		}

		// BytesReceived property
		public int BytesReceived
		{
			get { return videoSource.BytesReceived; }
		}

		// UserData property
		public object UserData
		{
			get { return videoSource.UserData; }
			set { videoSource.UserData = value; }
		}

		// Get state of video source
		public bool Running
		{
			get { return videoSource.Running; }
		}

		// Start receiving video frames
		public void Start()
		{
			videoSource.Start();
		}

		// Stop receiving video frames
		public void SignalToStop()
		{
			videoSource.SignalToStop();
		}

		// Wait for stop
		public void WaitForStop()
		{
			videoSource.WaitForStop();
		}

		// Stop work
		public void Stop()
		{
			videoSource.Stop();
		}

		// Update video source
		protected abstract void UpdateVideoSource();
	}
}
