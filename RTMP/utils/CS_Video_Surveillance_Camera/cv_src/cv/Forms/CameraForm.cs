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
	public class CameraForm : CameraViewer.Wizard
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
			}
		}

		// CheckCameraFunction property
		public CheckCameraHandler CheckCameraFunction
		{
			set { page1.CheckCameraFunction = value; }
		}


		// Construction
		public CameraForm()
		{
			this.AddPage(page1);
			this.AddPage(page2);
			this.Text = "New camera wizard";

			page1.Camera = camera;
			page2.Camera = camera;
		}

		// On page changing
		protected override void OnPageChanging(int page)
		{
			if (page == 1)
			{
				// switching to camera settings
				page2.Provider = page1.VideoProviders[page1.SelectedProviderIndex];
			}
			base.OnPageChanging(page);
		}

		// Reset event ocuren on page
		protected override void OnResetOnPage(int page)
		{
			if (page == 0)
			{
				page2.Provider = null;
			}
		}

		// On finish
		protected override void OnFinish()
		{
		}
	}
}

