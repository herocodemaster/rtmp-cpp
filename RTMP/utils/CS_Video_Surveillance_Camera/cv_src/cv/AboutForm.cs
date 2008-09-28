// Camera Vision
//
// Copyright © Andrew Kirillov, 2005-2006
// andrew.kirillov@gmail.com
//
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace CameraViewer
{
	/// <summary>
	/// Summary description for AboutForm.
	/// </summary>
	public class AboutForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.PictureBox pictureBox2;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.LinkLabel siteLink;
		private System.Windows.Forms.Label label1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public AboutForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			siteLink.Links.Add(0, siteLink.Text.Length, "mailto:andrew.kirillov@gmail.com");
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.okButton = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.pictureBox2 = new System.Windows.Forms.PictureBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.siteLink = new System.Windows.Forms.LinkLabel();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pictureBox1.Location = new System.Drawing.Point(10, 135);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(375, 2);
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// okButton
			// 
			this.okButton.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.okButton.Location = new System.Drawing.Point(160, 150);
			this.okButton.Name = "okButton";
			this.okButton.TabIndex = 1;
			this.okButton.Text = "Ok";
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.White;
			this.panel1.Controls.AddRange(new System.Windows.Forms.Control[] {
																				 this.label1});
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.ForeColor = System.Drawing.Color.Black;
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(394, 46);
			this.panel1.TabIndex = 2;
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.label1.ForeColor = System.Drawing.Color.Navy;
			this.label1.Location = new System.Drawing.Point(10, 5);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(300, 30);
			this.label1.TabIndex = 0;
			this.label1.Text = "Camera Vision";
			// 
			// pictureBox2
			// 
			this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Top;
			this.pictureBox2.Location = new System.Drawing.Point(0, 46);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new System.Drawing.Size(394, 1);
			this.pictureBox2.TabIndex = 3;
			this.pictureBox2.TabStop = false;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(136, 55);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(122, 15);
			this.label2.TabIndex = 4;
			this.label2.Text = "Camera Vision 1.0.0";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(60, 80);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(275, 16);
			this.label3.TabIndex = 5;
			this.label3.Text = "Copyright © 2005, Andrew Kirillov, All rights reserved";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// siteLink
			// 
			this.siteLink.Location = new System.Drawing.Point(105, 110);
			this.siteLink.Name = "siteLink";
			this.siteLink.Size = new System.Drawing.Size(184, 13);
			this.siteLink.TabIndex = 6;
			this.siteLink.TabStop = true;
			this.siteLink.Text = "andrew.kirillov@gmail.com";
			this.siteLink.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.siteLink.VisitedLinkColor = System.Drawing.Color.Blue;
			this.siteLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
			// 
			// AboutForm
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.okButton;
			this.ClientSize = new System.Drawing.Size(394, 178);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.siteLink,
																		  this.label3,
																		  this.label2,
																		  this.pictureBox2,
																		  this.panel1,
																		  this.okButton,
																		  this.pictureBox1});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AboutForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "About";
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		// On link clicked
		private void linkLabel1_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
		}
	}
}
