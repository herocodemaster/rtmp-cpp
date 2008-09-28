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
	/// Summary description for CameraInfo.
	/// </summary>
	public class CameraInfo : System.Windows.Forms.Form
	{
		private Camera camera;

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label widthLabel;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label nameLabel;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label descriptionLabel;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label providerLabel;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label heightLabel;
		private System.Windows.Forms.Button closeButton;
		private System.Windows.Forms.PictureBox pictureBox2;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		// Camera property
		public Camera Camera
		{
			get { return camera; }
			set { camera = value; }
		}

		// Constructor
		public CameraInfo()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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
			this.label1 = new System.Windows.Forms.Label();
			this.widthLabel = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.nameLabel = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.descriptionLabel = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.providerLabel = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.heightLabel = new System.Windows.Forms.Label();
			this.closeButton = new System.Windows.Forms.Button();
			this.pictureBox2 = new System.Windows.Forms.PictureBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 136);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(40, 14);
			this.label1.TabIndex = 0;
			this.label1.Text = "Width:";
			// 
			// widthLabel
			// 
			this.widthLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.widthLabel.Location = new System.Drawing.Point(60, 135);
			this.widthLabel.Name = "widthLabel";
			this.widthLabel.Size = new System.Drawing.Size(60, 16);
			this.widthLabel.TabIndex = 1;
			this.widthLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(140, 136);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(40, 14);
			this.label2.TabIndex = 2;
			this.label2.Text = "Height:";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 9);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(40, 14);
			this.label3.TabIndex = 3;
			this.label3.Text = "Name:";
			// 
			// nameLabel
			// 
			this.nameLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.nameLabel.Location = new System.Drawing.Point(60, 8);
			this.nameLabel.Name = "nameLabel";
			this.nameLabel.Size = new System.Drawing.Size(200, 16);
			this.nameLabel.TabIndex = 4;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 30);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(65, 14);
			this.label5.TabIndex = 5;
			this.label5.Text = "Description:";
			// 
			// descriptionLabel
			// 
			this.descriptionLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.descriptionLabel.Location = new System.Drawing.Point(8, 47);
			this.descriptionLabel.Name = "descriptionLabel";
			this.descriptionLabel.Size = new System.Drawing.Size(252, 40);
			this.descriptionLabel.TabIndex = 6;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 96);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(50, 14);
			this.label4.TabIndex = 7;
			this.label4.Text = "Provider:";
			// 
			// providerLabel
			// 
			this.providerLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.providerLabel.Location = new System.Drawing.Point(60, 95);
			this.providerLabel.Name = "providerLabel";
			this.providerLabel.Size = new System.Drawing.Size(200, 16);
			this.providerLabel.TabIndex = 8;
			// 
			// pictureBox1
			// 
			this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pictureBox1.Location = new System.Drawing.Point(8, 120);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(252, 2);
			this.pictureBox1.TabIndex = 9;
			this.pictureBox1.TabStop = false;
			// 
			// heightLabel
			// 
			this.heightLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.heightLabel.Location = new System.Drawing.Point(200, 135);
			this.heightLabel.Name = "heightLabel";
			this.heightLabel.Size = new System.Drawing.Size(60, 16);
			this.heightLabel.TabIndex = 10;
			this.heightLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// closeButton
			// 
			this.closeButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.closeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.closeButton.Location = new System.Drawing.Point(98, 175);
			this.closeButton.Name = "closeButton";
			this.closeButton.TabIndex = 11;
			this.closeButton.Text = "Close";
			// 
			// pictureBox2
			// 
			this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pictureBox2.Location = new System.Drawing.Point(9, 163);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new System.Drawing.Size(252, 2);
			this.pictureBox2.TabIndex = 12;
			this.pictureBox2.TabStop = false;
			// 
			// CameraInfo
			// 
			this.AcceptButton = this.closeButton;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.closeButton;
			this.ClientSize = new System.Drawing.Size(270, 206);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.pictureBox2,
																		  this.closeButton,
																		  this.heightLabel,
																		  this.pictureBox1,
																		  this.providerLabel,
																		  this.label4,
																		  this.descriptionLabel,
																		  this.label5,
																		  this.nameLabel,
																		  this.label3,
																		  this.label2,
																		  this.widthLabel,
																		  this.label1});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CameraInfo";
			this.Opacity = 0.85;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Camera info";
			this.Load += new System.EventHandler(this.CameraInfo_Load);
			this.ResumeLayout(false);

		}
		#endregion

		// On load
		private void CameraInfo_Load(object sender, System.EventArgs e)
		{
			if (camera != null)
			{
				nameLabel.Text = camera.Name;
				descriptionLabel.Text = camera.Description;
				providerLabel.Text = camera.Provider.Name;

				if (camera.Width != -1)
				{
					widthLabel.Text = camera.Width.ToString();
					heightLabel.Text = camera.Height.ToString();
				}
				else
				{
					widthLabel.Text = string.Empty;
					heightLabel.Text = string.Empty;
				}
			}
		}
	}
}
