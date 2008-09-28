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

namespace CameraViewer
{
	// Check camera delegate
	public delegate bool CheckCameraHandler(Camera camera);

	/// <summary>
	/// Summary description for CameraDescription.
	/// </summary>
	public class CameraDescription : System.Windows.Forms.UserControl, IWizardPage
	{
		private VideoProviderCollection providers = null;
		private Camera camera = null;
		private bool completed = false;

		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox descriptionBox;
		private System.Windows.Forms.TextBox nameBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox videoSourceCombo;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		// state changed event
		public event EventHandler StateChanged;
		// reset event
		public event EventHandler Reset;

		// VideoProviders property
		public VideoProviderCollection VideoProviders
		{
			get { return providers; }
			set
			{
				providers = value;
				BuildSourceCombo();
			}
		}
		// SelectedProviderIndex property
		public int SelectedProviderIndex
		{
			get { return videoSourceCombo.SelectedIndex - 1; }
		}
		// Camera property
		public Camera Camera
		{
			get { return camera; }
			set
			{
				if (value != null)
				{
					camera = value;

					nameBox.Text = camera.Name;
					descriptionBox.Text = camera.Description;

					// provider index
					if (providers != null)
						videoSourceCombo.SelectedIndex = ((IList) providers).IndexOf(camera.Provider) + 1;

					videoSourceCombo.Enabled = (camera.ID == 0);
				}
			}
		}

		// Check camera function
		private CheckCameraHandler checkCameraFunction;

		// CheckCameraFunction property
		public CheckCameraHandler CheckCameraFunction
		{
			set { checkCameraFunction = value; }
		}

		// Constructor
		public CameraDescription()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// Build source providers list
			BuildSourceCombo();
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
			this.label2 = new System.Windows.Forms.Label();
			this.descriptionBox = new System.Windows.Forms.TextBox();
			this.nameBox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.videoSourceCombo = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(10, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(66, 16);
			this.label2.TabIndex = 2;
			this.label2.Text = "&Description:";
			// 
			// descriptionBox
			// 
			this.descriptionBox.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.descriptionBox.Location = new System.Drawing.Point(10, 60);
			this.descriptionBox.Multiline = true;
			this.descriptionBox.Name = "descriptionBox";
			this.descriptionBox.Size = new System.Drawing.Size(300, 80);
			this.descriptionBox.TabIndex = 3;
			this.descriptionBox.Text = "";
			// 
			// nameBox
			// 
			this.nameBox.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.nameBox.BackColor = System.Drawing.SystemColors.Window;
			this.nameBox.Location = new System.Drawing.Point(60, 10);
			this.nameBox.Name = "nameBox";
			this.nameBox.Size = new System.Drawing.Size(250, 20);
			this.nameBox.TabIndex = 1;
			this.nameBox.Text = "";
			this.nameBox.TextChanged += new System.EventHandler(this.nameBox_TextChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(10, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(38, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "N&ame:";
			// 
			// label3
			// 
			this.label3.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.label3.Location = new System.Drawing.Point(10, 153);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(75, 17);
			this.label3.TabIndex = 4;
			this.label3.Text = "Video &source:";
			// 
			// videoSourceCombo
			// 
			this.videoSourceCombo.Anchor = ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.videoSourceCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.videoSourceCombo.Location = new System.Drawing.Point(90, 150);
			this.videoSourceCombo.MaxDropDownItems = 15;
			this.videoSourceCombo.Name = "videoSourceCombo";
			this.videoSourceCombo.Size = new System.Drawing.Size(220, 21);
			this.videoSourceCombo.TabIndex = 5;
			this.videoSourceCombo.SelectedIndexChanged += new System.EventHandler(this.videoSourceCombo_SelectedIndexChanged);
			// 
			// CameraDescription
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.videoSourceCombo,
																		  this.label3,
																		  this.label2,
																		  this.descriptionBox,
																		  this.nameBox,
																		  this.label1});
			this.Name = "CameraDescription";
			this.Size = new System.Drawing.Size(320, 180);
			this.ResumeLayout(false);

		}
		#endregion
	
		// PageName property
		public string PageName
		{
			get { return "Description"; }
		}

		// PageDescription property
		public string PageDescription
		{
			get { return "Camera description"; }
		}

		// Completed property
		public bool Completed
		{
			get { return completed; }
		}

		// Show the page
		public void Display()
		{
			nameBox.Focus();
			nameBox.SelectionStart = nameBox.TextLength;
		}

		// Apply the page
		public bool Apply()
		{
			string name = nameBox.Text.Replace('\\', ' ');

			if (checkCameraFunction != null)
			{
				Camera tmpCamera = new Camera(name);

				tmpCamera.ID = camera.ID;
				tmpCamera.Parent = camera.Parent;

				// check camera
				if (checkCameraFunction(tmpCamera) == false)
				{
					Color	tmp = this.nameBox.BackColor;

					// highligh name edit box
					this.nameBox.BackColor = Color.LightCoral;
					// error message
					MessageBox.Show(this, "A camera with such name is already exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
					// restore & focus name edit box
					this.nameBox.BackColor = tmp;
					this.nameBox.Focus();

					return false;
				}
			}

			// update camera name and description
			camera.Name = name;
			camera.Description = descriptionBox.Text;
			camera.Provider = providers[videoSourceCombo.SelectedIndex - 1];

			return true;
		}

		// Build video source combo
		private void BuildSourceCombo()
		{
			// clean combo
			videoSourceCombo.Items.Clear();

			// add default item
			videoSourceCombo.Items.Add("--- Select video source ---");
			videoSourceCombo.SelectedIndex = 0;

			if (providers != null)
			{
				// add video sources
				foreach (VideoProvider p in providers)
				{
					videoSourceCombo.Items.Add(p.Name);
				}
			}
		}

		// On Name edit box changed
		private void nameBox_TextChanged(object sender, System.EventArgs e)
		{
			UpdateState();
		}

		// On Video Source combo selection changed
		private void videoSourceCombo_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			UpdateState();

			if (Reset != null)
				Reset(this, new EventArgs());
		}

		// Update state
		private void UpdateState()
		{
			completed = ((nameBox.TextLength != 0) && (videoSourceCombo.SelectedIndex != 0));
			
			if (StateChanged != null)
				StateChanged(this, new EventArgs());
		}
	}
}
