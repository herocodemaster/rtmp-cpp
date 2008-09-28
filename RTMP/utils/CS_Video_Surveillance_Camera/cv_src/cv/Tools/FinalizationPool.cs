// Camera Vision
//
// Copyright © Andrew Kirillov, 2005-2006
// andrew.kirillov@gmail.com
//
namespace CameraViewer
{
	using System;
	using System.Collections;
	using System.Threading;

	/// <summary>
	/// FinalizationPool
	/// </summary>
	public class FinalizationPool : CollectionBase
	{
		private Thread	thread;
		private ManualResetEvent stopEvent = null;

		// Constructor
		public FinalizationPool()
		{
		}

		// Start the pool
		public void Start()
		{
			// create events
			stopEvent	= new ManualResetEvent(false);
				
			// create and start new thread
			thread = new Thread(new ThreadStart(WorkerThread));
			thread.Start();
		}

		// Stop the pool
		public void Stop()
		{
			if (thread != null)
			{
				// signal to stop
				stopEvent.Set();
				// wait for thread stop
				thread.Join();

				thread = null;

				// release events
				stopEvent.Close();
				stopEvent = null;
			}
		}

		// Thread entry point
		private void WorkerThread()
		{
			while (!stopEvent.WaitOne(0, true))
			{
				// lock
				Monitor.Enter(this);

                int n = InnerList.Count;

				// check each camera
				for (int i = 0; i < n; i++)
				{
					Camera camera = (Camera) InnerList[i];

					if (!camera.Running)
					{
						camera.CloseVideoSource();
						InnerList.RemoveAt(i);
						i--;
						n--;
					}
				}
				// unlock
				Monitor.Exit(this);

				// sleep for a while
				Thread.Sleep(300);
			}

			// Exiting, so kill'em all
			foreach (Camera camera in InnerList)
			{
				camera.Stop();
			}
		}

		// Add new camera to the collection and run it
		public void Add(Camera camera)
		{
			// lock
			Monitor.Enter(this);
			// add to the pool
			InnerList.Add(camera);
			// unlock
			Monitor.Exit(this);
		}

		// Ensure the camera is stopped
		public void Remove(Camera camera)
		{
			// lock
			Monitor.Enter(this);

			int n = InnerList.Count;
			for (int i = 0; i < n; i++)
			{
				if (InnerList[i] == camera)
				{
					if (camera.Running)
						camera.Stop();
					camera.CloseVideoSource();
					InnerList.RemoveAt(i);
					break;
				}
			}

			// unlock
			Monitor.Exit(this);
		}
	}
}
