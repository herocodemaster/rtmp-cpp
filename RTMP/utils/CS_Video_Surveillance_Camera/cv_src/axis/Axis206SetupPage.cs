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

namespace axis
{
	/// <summary>
	/// Summary description for Axis206SetupPage.
	/// </summary>
	public class Axis206SetupPage : System.Windows.Forms.UserControl, IVideoSourcePage
	{
		private static int[] frameIntervals = new int[] {0, 100, 142, 200, 333, 1000,
															5000, 10000, 15000, 20000, 30000, 60000};
		private static StreamType[] streamTypes = new StreamType[] {StreamType.Jpeg, StreamType.MJpeg};
		private bool completed = false;
		private System.Windows.Forms.ComboBox sizeCombo;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox passwordBox;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox loginBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox serverBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox rateCombo;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.ComboBox streamCombo;
		private System.Windows.Forms.Label label5;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		// new frame event
		public event EventHandler StateChanged;

		// Constructor
		public Axis206SetupPage()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitForm call
			sizeCombo.SelectedIndex = 1;
			streamCombo.SelectedIndex = 0;
			rateCombo.SelectedIndex = 0;
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
			this.sizeCombo = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.passwordBox = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.loginBox = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.serverBox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.rateCombo = new System.Windows.Forms.ComboBox();
			this.label6 = new System.Windows.Forms.Label();
			this.streamCombo = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// sizeCombo
			// 
			this.sizeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.sizeCombo.Items.AddRange(new object[] {
														   "160x120",
														   "320x240",
														   "640x480"});
			this.sizeCombo.Location = new System.Drawing.Point(70, 100);
			this.sizeCombo.Name = "sizeCombo";
			this.sizeCombo.Size = new System.Drawing.Size(70, 21);
			this.sizeCombo.TabIndex = 7;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(10, 103);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(48, 14);
			this.label4.TabIndex = 6;
			this.label4.Text = "Si&ze:";
			// 
			// passwordBox
			// 
			this.passwordBox.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.passwordBox.Location = new System.Drawing.Point(70, 70);
			this.passwordBox.Name = "passwordBox";
			this.passwordBox.Size = new System.Drawing.Size(220, 20);
			this.passwordBox.TabIndex = 5;
			this.passwordBox.Text = "";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(10, 73);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(57, 14);
			this.label3.TabIndex = 4;
			this.label3.Text = "&Password:";
			// 
			// loginBox
			// 
			this.loginBox.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.loginBox.Location = new System.Drawing.Point(70, 40);
			this.loginBox.Name = "loginBox";
			this.loginBox.Size = new System.Drawing.Size(220, 20);
			this.loginBox.TabIndex = 3;
			this.loginBox.Text = "";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(10, 43);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(43, 14);
			this.label2.TabIndex = 2;
			this.label2.Text = "&Login:";
			// 
			// serverBox
			// 
			this.serverBox.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.serverBox.Location = new System.Drawing.Point(70, 10);
			this.serverBox.Name = "serverBox";
			this.serverBox.Size = new System.Drawing.Size(220, 20);
			this.serverBox.TabIndex = 1;
			this.serverBox.Text = "";
			this.serverBox.TextChanged += new System.EventHandler(this.serverBox_TextChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(10, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(45, 14);
			this.label1.TabIndex = 0;
			this.label1.Text = "&Server:";
			// 
			// rateCombo
			// 
			this.rateCombo.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.rateCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.rateCombo.Items.AddRange(new object[] {
														   "Uncontrolled",
														   "10 frames per second",
														   "7 frames per second",
														   "5 frames per second",
														   "3 frames per second",
														   "1 frame per second",
														   "12 frames per minute",
														   "6 frames per minute",
														   "4 frames per minute",
														   "3 frames per minute",
														   "2 frames per minute",
														   "1 frame per minute"});
			this.rateCombo.Location = new System.Drawing.Point(70, 130);
			this.rateCombo.Name = "rateCombo";
			this.rateCombo.Size = new System.Drawing.Size(220, 21);
			this.rateCombo.TabIndex = 11;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(10, 133);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(63, 14);
			this.label6.TabIndex = 10;
			this.label6.Text = "&Frame rate:";
			// 
			// streamCombo
			// 
			this.streamCombo.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.streamCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.streamCombo.Items.AddRange(new object[] {
															 "Jpeg",
															 "MJpeg"});
			this.streamCombo.Location = new System.Drawing.Point(215, 100);
			this.streamCombo.Name = "streamCombo";
			this.streamCombo.Size = new System.Drawing.Size(75, 21);
			this.streamCombo.TabIndex = 9;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(145, 103);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(70, 14);
			this.label5.TabIndex = 8;
			this.label5.Text = "Stream type:";
			// 
			// Axis206SetupPage
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.rateCombo,
																		  this.label6,
																		  this.streamCombo,
																		  this.label5,
																		  this.sizeCombo,
																		  this.label4,
																		  this.passwordBox,
																		  this.label3,
																		  this.loginBox,
																		  this.label2,
																		  this.serverBox,
																		  this.label1});
			this.Name = "Axis206SetupPage";
			this.Size = new System.Drawing.Size(300, 160);
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
			serverBox.Focus();
			serverBox.SelectionStart = serverBox.TextLength;
		}

		// Apply the page
		public bool Apply()
		{
			return true;
		}

		// Get configuration object
		public object GetConfiguration()
		{
			AxisConfiguration	config = new AxisConfiguration();

			config.source	= serverBox.Text;
			config.login	= loginBox.Text;
			config.password	= passwordBox.Text;
			config.resolution = sizeCombo.Text;
			config.stremType = streamTypes[streamCombo.SelectedIndex];
			config.frameInterval = frameIntervals[rateCombo.SelectedIndex];

			return (object) config;
		}

		// Set configuration
		public void SetConfiguration(object config)
		{
			AxisConfiguration	cfg = (AxisConfiguration) config;

			if (cfg != null)
			{
				serverBox.Text = cfg.source;
				loginBox.Text = cfg.login;
				passwordBox.Text = cfg.password;
				sizeCombo.Text = cfg.resolution;
				streamCombo.SelectedIndex = Array.IndexOf(streamTypes, cfg.stremType);
				rateCombo.SelectedIndex = Array.IndexOf(frameIntervals, cfg.frameInterval);
			}
		}

		// Server edit box changed
		private void serverBox_TextChanged(object sender, System.EventArgs e)
		{
			completed = (serverBox.TextLength != 0);

			if (StateChanged != null)
				StateChanged(this, new EventArgs());
		}
	}
}
