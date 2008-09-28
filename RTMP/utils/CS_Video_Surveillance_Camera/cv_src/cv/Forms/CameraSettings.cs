// Camera Vision
//
// Copyright © Andrew Kirillov, 2005-2006
// andrew.kirillov@gmail.com
//
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using videosource;

namespace CameraViewer
{
	/// <summary>
	/// Summary description for CameraSettings.
	/// </summary>
	public class CameraSettings : System.Windows.Forms.UserControl, IWizardPage
	{
		private bool completed = false;
		private Camera camera = null;
		private VideoProvider provider = null;
		private IVideoSourcePage sourcePage;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		// state changed event
		public event EventHandler StateChanged;
		// reset event
		public event EventHandler Reset;

		// Camera property
		public Camera Camera
		{
			get { return camera; }
			set
			{
				if (value != null)
				{
					camera = value;

					// set configuration
					if (sourcePage != null)
					{
						sourcePage.SetConfiguration(camera.Configuration);

						// completed
						completed = sourcePage.Completed;
					}
				}
			}
		}
		// Provider property
		public VideoProvider Provider
		{
			get { return provider; }
			set
			{
				if ((provider == null) || (provider != value))
				{
					if (sourcePage != null)
					{
						// remove old page
						Controls.Remove((Control) sourcePage);
					}

					completed = false;
					provider = value;

					if (provider != null)
					{
						sourcePage = provider.GetSettingsPage();

						if (sourcePage != null)
						{
							Control	control = (Control) sourcePage;

							// add control
							control.Dock = DockStyle.Fill;
							Controls.Add(control);

							// events
							sourcePage.StateChanged += new EventHandler(page_StateChanged);

							// set configuration
							sourcePage.SetConfiguration(camera.Configuration);

							// completed
							completed = sourcePage.Completed;
						}
					}
				}
			}
		}

		// Constructor
		public CameraSettings()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitForm call

		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// CameraSettings
			// 
			this.Name = "CameraSettings";
			this.Size = new System.Drawing.Size(320, 180);

		}
		#endregion

		// PageName property
		public string PageName
		{
			get { return "Settings"; }
		}

		// PageDescription property
		public string PageDescription
		{
			get
			{
				string str = "Camera settings";
				if (provider != null)
				{
					str += " : " + provider.Name;
				}
				return str;
			}
		}

		// Completed property
		public bool Completed
		{
			get { return completed; }
		}

		// Show the page
		public void Display()
		{
			if (sourcePage != null)
			{
				// show control
				((Control) sourcePage).Show();
				// notify page
				sourcePage.Display();
			}
		}

		// Apply the page
		public bool Apply()
		{
			bool	ret = false;

			if (sourcePage != null)
			{
				if ((ret = sourcePage.Apply()) == true)
				{
					camera.Configuration = sourcePage.GetConfiguration();
				}
			}

			return ret;
		}

		// On source page state changed
		private void page_StateChanged(object sender, System.EventArgs e)
		{
			completed = sourcePage.Completed;

			// notify wizard
			if (StateChanged != null)
				StateChanged(this, new EventArgs());
		}
	}
}
