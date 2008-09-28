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
	/// <summary>
	/// ViewGrid
	/// </summary>
	public class ViewGrid : System.Windows.Forms.Control
	{
		private short		cols = 2, rows = 2;
		private string[,]	labels;
		private Point		lastClickPoint;

		// Cols property
		[DefaultValue(2)]
		public short Cols
		{
			get { return cols; }
			set
			{
				cols = value;
				Redim();
				Invalidate();
			}
		}
		// Rows property
		[DefaultValue(2)]
		public short Rows
		{
			get { return rows; }
			set
			{
				rows = value;
				Redim();
				Invalidate();
			}
		}

		// Constructor
		public ViewGrid()
		{
			InitializeComponent();

			labels = new string[rows, cols];

			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer |
						ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
		}

		private void InitializeComponent()
		{
			// 
			// ViewGrid
			// 
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ViewGrid_MouseDown);

		}

		//
		private void Redim()
		{
			string[,] t =labels;
			labels = new string [rows, cols];

			int		r = Math.Min(rows, t.GetLength(0));
			int		c = Math.Min(cols, t.GetLength(1));

			// copy old labels into new
			for (int i = 0; i < r; i++)
			{
				for (int j = 0; j < c; j++)
				{
					labels[i, j] = t[i, j];
				}
			}

		}

		// Paint the control
		protected override void OnPaint(PaintEventArgs pe)
		{
			Graphics	g = pe.Graphics;
			Rectangle	rc = this.ClientRectangle;
			int			cellWidth = rc.Width / cols;
			int			cellHeight = rc.Height / rows;

			Pen			pen = new Pen(Color.Black, 1);
			Brush		brush1 = new SolidBrush(Color.FromArgb(240, 240, 255));
			Brush		brush2 = new SolidBrush(Color.FromArgb(0, 0, 0));
			Font		font = new Font("Arial", 8);

			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < cols; j++)
				{
					g.DrawRectangle(pen, j * cellWidth, i * cellHeight, cellWidth - 2, cellHeight - 2);
					g.FillRectangle(brush1, j * cellWidth + 1, i * cellHeight + 1, cellWidth - 3, cellHeight - 3);

					if (labels[i, j] != null)
						g.DrawString(labels[i, j], font, brush2, new RectangleF(j * cellWidth + 2, i * cellHeight + 2, cellWidth - 6, cellHeight - 6));
				}
			}

			font.Dispose();
			brush1.Dispose();
			brush2.Dispose();
			pen.Dispose();

			// Calling the base class OnPaint
			base.OnPaint(pe);
		}

		// Set label and return it's coordinates in rows and cols
		public Point SetLabel(string label, Point clientPt)
		{
			Point point = ClientToGrid(clientPt);

			labels[point.Y, point.X] = label;
			Invalidate();

			return point;
		}
		public Point SetLabel(string label)
		{
			return SetLabel(label, lastClickPoint);
		}
		public void SetLabel(string label, int row, int col)
		{
			labels[row, col] = label;
			Invalidate();
		}

		// Convert client coordinates to grids row and column
		protected Point ClientToGrid(Point pt)
		{
			Rectangle	rc = this.ClientRectangle;
			int			cellWidth = rc.Width / cols;
			int			cellHeight = rc.Height / rows;

			return new Point(pt.X / cellWidth, pt.Y / cellHeight);
		}

		// On mouse down
		private void ViewGrid_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			lastClickPoint = new Point(e.X, e.Y);
		}
	}
}
