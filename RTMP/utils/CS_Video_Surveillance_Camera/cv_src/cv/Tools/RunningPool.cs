// Camera Vision
//
// Copyright © Andrew Kirillov, 2005-2006
// andrew.kirillov@gmail.com
//
namespace CameraViewer
{
	using System;
	using System.Collections;

	/// <summary>
	/// RunningPool
	/// </summary>
	public class RunningPool : CollectionBase
	{
		// Configuration
		public RunningPool()
		{
		}

		// Get group at the specified index
		public Camera this[int index]
		{
			get
			{
				return ((Camera) InnerList[index]);
			}
		}

		// Add new camera to the collection and run it
		public bool Add(Camera camera)
		{
			// create video source
			if (camera.CreateVideoSource())
			{
				// add to the pool
				InnerList.Add(camera);

				camera.Start();
				return true;
			}
			return false;
		}

		// Remove camera from the collection and signal to stop it
		public void Remove(Camera camera)
		{
			InnerList.Remove(camera);

			// signal to stop
			camera.SignalToStop();
		}
	}
}
