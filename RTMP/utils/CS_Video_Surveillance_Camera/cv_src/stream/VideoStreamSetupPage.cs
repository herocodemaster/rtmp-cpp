// Camara Vision
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

namespace stream
{
	/// <summary>
	/// Summary description for VideoStreamSetupPage.
	/// </summary>
	public class VideoStreamSetupPage : System.Windows.Forms.UserControl, IVideoSourcePage
	{
		private bool completed = false;
		private System.Windows.Forms.TextBox urlBox;
		private System.Windows.Forms.Label label1;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		// state changed event
		public event EventHandler StateChanged;

		// Constructor
		public VideoStreamSetupPage()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

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
			this.urlBox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// urlBox
			// 
			this.urlBox.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.urlBox.Location = new System.Drawing.Point(50, 10);
			this.urlBox.Name = "urlBox";
			this.urlBox.Size = new System.Drawing.Size(240, 20);
			this.urlBox.TabIndex = 1;
			this.urlBox.Text = "";
			this.urlBox.TextChanged += new System.EventHandler(this.urlBox_TextChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(10, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(30, 14);
			this.label1.TabIndex = 0;
			this.label1.Text = "&URL:";
			// 
			// VideoStreamSetupPage
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.urlBox,
																		  this.label1});
			this.Name = "VideoStreamSetupPage";
			this.Size = new System.Drawing.Size(300, 150);
			this.ResumeLayout(false);

		}
		#endregion

		// Completed property
		public bool Completed
		{
			get { return completed; }
		}

		// Show the page
		public void Display()
		{
			urlBox.Focus();
			urlBox.SelectionStart = urlBox.TextLength;
		}

		// Apply the page
		public bool Apply()
		{
			return true;
		}

		// Get configuration object
		public object GetConfiguration()
		{
			StreamConfiguration config = new StreamConfiguration();

			config.source = urlBox.Text;

			return (object) config;
		}

		// Set configuration
		public void SetConfiguration(object config)
		{
			StreamConfiguration cfg = (StreamConfiguration) config;

			if (cfg != null)
			{
				urlBox.Text = cfg.source;
			}
		}

		// URL changed
		private void urlBox_TextChanged(object sender, System.EventArgs e)
		{
			completed = (urlBox.TextLength != 0);

			if (StateChanged != null)
				StateChanged(this, new EventArgs());
		}
	}
}
