// Camera Vision
//
// Copyright © Andrew Kirillov, 2005-2006
// andrew.kirillov@gmail.com
//
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CameraViewer
{
	public class CameraPropertiesForm : CameraViewer.PagedWizard
	{
		private Camera camera = new Camera("");
		private CameraDescription	page1 = new CameraDescription();
		private CameraSettings		page2 = new CameraSettings();

		// VideoProviders property
		public VideoProviderCollection VideoProviders
		{
			set { page1.VideoProviders = value; }
		}

		// Camera property
		public Camera Camera
		{
			get { return camera; }
			set
			{
				camera = value;

				page1.Camera = camera;
				page2.Camera = camera;

				page2.Provider = camera.Provider;
			}
		}

		// CheckCameraFunction property
		public CheckCameraHandler CheckCameraFunction
		{
			set { page1.CheckCameraFunction = value; }
		}

		// Constructor
		public CameraPropertiesForm()
		{
			this.AddPage(page1);
			this.AddPage(page2);
			this.Text = "Camera properties";

			page1.Camera = camera;
			page2.Camera = camera;
		}


	}
}

