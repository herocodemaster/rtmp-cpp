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
	/// CameraCollection class
	/// </summary>
	public class CameraCollection : CollectionBase
	{
		// Constructor
		public CameraCollection()
		{
		}

		// Get came at the specified index
		public Camera this[int index]
		{
			get
			{
				return ((Camera) InnerList[index]);
			}
		}

		// Add new camera to the collection
		public void Add(Camera camera)
		{
			InnerList.Add(camera);
		}

		// Remove camera from the collection
		public void Remove(Camera camera)
		{
			InnerList.Remove(camera);
		}

		// Get camera with specified name
		public Camera GetCamera(string name, Group parent)
		{
			// find the camera
			foreach (Camera camera in InnerList)
			{
				if ((camera.Name == name) && (camera.Parent == parent))
					return camera;
			}
			return null;
		}

		// Get camera with specified ID
		public Camera GetCamera(int cameraID)
		{
			// find the camera
			foreach (Camera camera in InnerList)
			{
				if (camera.ID == cameraID)
					return camera;
			}
			return null;
		}
	}
}
