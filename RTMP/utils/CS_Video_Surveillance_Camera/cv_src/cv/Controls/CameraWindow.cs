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
using System.Threading;

namespace CameraViewer
{
	/// <summary>
	/// Summary description for CameraWindow.
	/// </summary>
	public class CameraWindow : System.Windows.Forms.Control
	{
		private Camera	camera = null;
		private bool	autosize = false;
		private bool	needSizeUpdate = false;
		private bool	firstFrame = true;

		// AutoSize property
		[DefaultValue(false)]
		public bool AutoSize
		{
			get { return autosize; }
			set
			{
				autosize = value;
				UpdatePosition();
			}
		}

		// Camera property
		[Browsable(false)]
		public Camera Camera
		{
			get { return camera; }
			set
			{
				// lock
				Monitor.Enter(this);

				// detach event
				if (camera != null)
				{
					camera.NewFrame -= new EventHandler(camera_NewFrame);
				}

				camera = value;
				needSizeUpdate = true;
				firstFrame = true;

				// atach event
				if (camera != null)
				{
					camera.NewFrame += new EventHandler(camera_NewFrame);
				}

				// unlock
				Monitor.Exit(this);
			}
		}

		// Constructor
		public CameraWindow()
		{
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer |
				ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
		}

		// Paint control
		protected override void OnPaint(PaintEventArgs pe)
		{
			if ((needSizeUpdate) || (firstFrame))
			{
				UpdatePosition();
				needSizeUpdate = false;
			}

			// lock
			Monitor.Enter(this);

			Graphics	g = pe.Graphics;
			Rectangle	rc = this.ClientRectangle;
			Pen			pen = new Pen(Color.Black, 1);

			// draw rectangle
			g.DrawRectangle(pen, rc.X, rc.Y, rc.Width - 1, rc.Height - 1);

			if (camera != null)
			{
				camera.Lock();

				// draw frame
				if (camera.LastFrame != null)
				{
					g.DrawImage(camera.LastFrame, rc.X + 1, rc.Y + 1, rc.Width - 2, rc.Height - 2);
					firstFrame = false;
				}
				else
				{
					// Create font and brush
					Font drawFont = new Font("Arial", 12);
					SolidBrush drawBrush = new SolidBrush(Color.White);

					g.DrawString("Connecting ...", drawFont, drawBrush, new PointF(5, 5));

					drawBrush.Dispose();
					drawFont.Dispose();
				}

				camera.Unlock();
			}

			pen.Dispose();

			// unlock
			Monitor.Exit(this);

			base.OnPaint(pe);
		}

		// update position and size of the control
		public void UpdatePosition()
		{
			// lock
			Monitor.Enter(this);

			if (autosize)
			{
				Rectangle	rc = this.Parent.ClientRectangle;
				int			width = 320;
				int			height = 240;

				if (camera != null)
				{
					camera.Lock();

					// get frame dimension
					if (camera.LastFrame != null)
					{
						width = camera.LastFrame.Width;
						height = camera.LastFrame.Height;
					}
					camera.Unlock();
				}

				//
				this.SuspendLayout();
				this.Location = new Point((rc.Width - width - 2) / 2, (rc.Height - height - 2) / 2);
				this.Size = new Size(width + 2, height + 2);
				this.ResumeLayout();

			}
			// unlock
			Monitor.Exit(this);
		}

		// On new frame ready
		private void camera_NewFrame(object sender, System.EventArgs e)
		{
			Invalidate();
		}
	}
}
