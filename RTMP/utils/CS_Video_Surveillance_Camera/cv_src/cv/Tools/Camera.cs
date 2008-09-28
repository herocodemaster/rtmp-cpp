// Camera Vision
//
// Copyright © Andrew Kirillov, 2005-2006
// andrew.kirillov@gmail.com
//
namespace CameraViewer
{
	using System;
	using System.Drawing;
	using System.Threading;
	using videosource;
    using System.IO; //Fernando:

	/// <summary>
	/// Camera class
	/// </summary>
	public class Camera
	{
		private int		id = 0;
		private string	name;
		private string	description = "";
		private Group	parent = null;
		private object	configuration = null;
		private VideoProvider	provider = null;

		private IVideoSource	videoSource = null;
		private Bitmap			lastFrame = null;

		private int		width = -1, height = -1;

		//
		public event EventHandler	NewFrame;

		// ID property
		public int ID
		{
			get { return id; }
			set { id = value; }
		}	
		// Name property
		public string Name
		{
			get { return name; }
			set { name = value; }
		}
		// Description property
		public string Description
		{
			get { return description; }
			set { description = value; }
		}
		// Parent property
		public Group Parent
		{
			get { return parent; }
			set { parent = value; }
		}
		// FullName property
		public string FullName
		{
			get
			{
				return (parent == null) ? name : (parent.FullName + '\\' + name);
			}
		}

		// Configuration property
		public object Configuration
		{
			get { return configuration; }
			set { configuration = value; }
		}
		// Provider property
		public VideoProvider Provider
		{
			get { return provider; }
			set { provider = value; }
		}
		// LastFrame property
		public Bitmap LastFrame
		{
			get { return lastFrame; }
		}
		// Width property
		public int Width
		{
			get { return width; }
		}
		// Height property
		public int Height
		{
			get { return height; }
		}
		// FramesReceived property
		public int FramesReceived
		{
			get { return (videoSource == null) ? 0 : videoSource.FramesReceived; }
		}
		// BytesReceived property
		public int BytesReceived
		{
			get { return (videoSource == null) ? 0 : videoSource.BytesReceived; }
		}
		// Running property
		public bool Running
		{
			get { return (videoSource == null) ? false : videoSource.Running; }
		}

		// Constructor
		public Camera(string name)
		{
			this.name = name;
		}

		// Create video source
		public bool CreateVideoSource()
		{
			if ((provider != null) && ((videoSource = provider.CreateVideoSource(configuration)) != null))
			{
				videoSource.NewFrame += new CameraEventHandler(video_NewFrame);
				return true;
			}
			return false;
		}

		// Close video source
		public void CloseVideoSource()
		{
			if (videoSource != null)
			{
				videoSource = null;
			}
			// dispose old frame
			if (lastFrame != null)
			{
				lastFrame.Dispose();
				lastFrame = null;
			}
			width = -1;
			height = -1;
		}

		// Start video source
		public void Start()
		{
			if (videoSource != null)
			{
				videoSource.Start();
			}
		}

		// Siganl video source to stop
		public void SignalToStop()
		{
			if (videoSource != null)
			{
				videoSource.SignalToStop();
			}
		}

		// Wait video source for stop
		public void WaitForStop()
		{
			// lock
			Monitor.Enter(this);

			if (videoSource != null)
			{
				videoSource.WaitForStop();
			}
			// unlock
			Monitor.Exit(this);
		}

		// Abort camera
		public void Stop()
		{
			// lock
			Monitor.Enter(this);

			if (videoSource != null)
			{
				videoSource.Stop();
			}
			// unlock
			Monitor.Exit(this);
		}

		// Lock it
		public void Lock()
		{
			Monitor.Enter(this);
		}

		// Unlock it
		public void Unlock()
		{
			Monitor.Exit(this);
		}

		// On new frame
		private void video_NewFrame(object sender, CameraEventArgs e)
		{
			// lock
			Monitor.Enter(this);

			// dispose old frame
			if (lastFrame != null)
			{
				lastFrame.Dispose();
			}

			lastFrame = (Bitmap) e.Bitmap.Clone();

            ////Fernando:
            DateTime now = DateTime.Now;
            string timeStamp = now.ToString("yyyyMMddHHmmssfff");
            string fileName = "image_" + timeStamp + ".jpg";
            lastFrame.Save(@"F:\Development\CPP\RTMP\projects\ffmpeg\bin\img\" + fileName);



            string filePath = @"F:\Development\CPP\RTMP\projects\ffmpeg\bin\img\images.txt";
            //TextWriter tw = new StreamWriter(@"F:\Development\CPP\RTMP\projects\ffmpeg\bin\img\images.txt", true, );
            FileStream fileStream = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
            TextWriter textWriter = new StreamWriter(fileStream);
            textWriter.WriteLine("0|" + timeStamp + "|" + fileName);
            textWriter.Flush();
            textWriter.Close();
            fileStream.Close();


            //Fernando: FIN



			// image dimension
			width = lastFrame.Width;
			height = lastFrame.Height;

			// unlock
			Monitor.Exit(this);

			// notify client
			if (NewFrame != null)
				NewFrame(this, new EventArgs());
		}
	}
}
