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
	/// Summary description for Multiplexer.
	/// </summary>
	public class Multiplexer : System.Windows.Forms.Panel
	{
		private const int		MaxRows = 5;
		private const int		MaxCols = 5;
		private CameraWindow[,]	camWindows;

		private bool	fitToWindow = false;
		private bool	singleCameraMode = true;
		private bool	camerasVisible = false;

		private int		rows = 1;
		private int		cols = 1;
		private int		cellWidth = 320;
		private int		cellHeight = 240;

		private CameraWindow	lastClicked;

		private CameraViewer.CameraWindow cameraWindow1;
		private CameraViewer.CameraWindow cameraWindow2;
		private CameraViewer.CameraWindow cameraWindow3;
		private CameraViewer.CameraWindow cameraWindow4;
		private CameraViewer.CameraWindow cameraWindow5;
		private CameraViewer.CameraWindow cameraWindow6;
		private CameraViewer.CameraWindow cameraWindow7;
		private CameraViewer.CameraWindow cameraWindow8;
		private CameraViewer.CameraWindow cameraWindow9;
		private CameraViewer.CameraWindow cameraWindow10;
		private CameraViewer.CameraWindow cameraWindow11;
		private CameraViewer.CameraWindow cameraWindow12;
		private CameraViewer.CameraWindow cameraWindow13;
		private CameraViewer.CameraWindow cameraWindow14;
		private CameraViewer.CameraWindow cameraWindow15;
		private CameraViewer.CameraWindow cameraWindow16;
		private CameraViewer.CameraWindow cameraWindow17;
		private CameraViewer.CameraWindow cameraWindow18;
		private CameraViewer.CameraWindow cameraWindow19;
		private CameraViewer.CameraWindow cameraWindow20;
		private CameraViewer.CameraWindow cameraWindow21;
		private CameraViewer.CameraWindow cameraWindow22;
		private CameraViewer.CameraWindow cameraWindow23;
		private CameraViewer.CameraWindow cameraWindow24;
		private CameraViewer.CameraWindow cameraWindow25;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		// FitToWindow property
		[DefaultValue(false)]
		public bool FitToWindow
		{
			get { return fitToWindow; }
			set
			{
				fitToWindow = value;

				if ((camWindows[0, 0].AutoSize = (!fitToWindow && singleCameraMode)) == true)
				{
					camWindows[0, 0].UpdatePosition();
				}
				else
				{
					UpdateSize();
				}
			}
		}
		// SingleCameraMode property
		[DefaultValue(true)]
		public bool SingleCameraMode
		{
			get { return singleCameraMode; }
			set
			{
				singleCameraMode = value;
				if (!fitToWindow)
					camWindows[0, 0].AutoSize = value;
			}
		}
		// CamerasVisible property
		[DefaultValue(false)]
		public bool CamerasVisible
		{
			get { return camerasVisible; }
			set
			{
				camerasVisible = value;

				// show/hide all cameras
				for (int i = 0; i < rows; i++)
				{
					for (int j = 0; j < cols; j++)
					{
						camWindows[i, j].Visible = value;
					}
				}
			}
		}
		// Rows property
		[DefaultValue(1)]
		public int Rows
		{
			get { return rows; }
			set
			{
				rows = Math.Max(1, Math.Min(MaxRows, value));
				UpdateVisiblity();
				UpdateSize();
			}
		}
		// Cols property
		[DefaultValue(1)]
		public int Cols
		{
			get { return cols; }
			set
			{
				cols = Math.Max(1, Math.Min(MaxCols, value));
				UpdateVisiblity();
				UpdateSize();
			}
		}
		// CellWidth
		[DefaultValue(320)]
		public int CellWidth
		{
			get { return cellWidth; }
			set
			{
				cellWidth = Math.Max(50, Math.Min(800, value));
				UpdateSize();
			}
		}
		// CellHeight
		[DefaultValue(240)]
		public int CellHeight
		{
			get { return cellHeight; }
			set
			{
				cellHeight = Math.Max(50, Math.Min(800, value));
				UpdateSize();
			}
		}
		// Context menu for cameras windows
		[DefaultValue(null)]
		public ContextMenu CamerasContextMenu
		{
			get { return camWindows[0, 0].ContextMenu; }
			set
			{
				for (int i = 0; i < MaxRows; i++)
				{
					for (int j = 0; j < MaxCols; j++)
					{
						camWindows[i, j].ContextMenu = value;
					}
				}
			}
		}
		// Camera of the last click
		[Browsable(false)]
		public Camera ContextCamera
		{
			get { return (lastClicked == null) ? null : lastClicked.Camera; }
		}

		// Constructor
		public Multiplexer()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitForm call
			camWindows = new CameraWindow[MaxRows, MaxCols];

			// row 1
			camWindows[0, 0] = cameraWindow1;
			camWindows[0, 1] = cameraWindow2;
			camWindows[0, 2] = cameraWindow3;
			camWindows[0, 3] = cameraWindow4;
			camWindows[0, 4] = cameraWindow5;
			// row 2
			camWindows[1, 0] = cameraWindow6;
			camWindows[1, 1] = cameraWindow7;
			camWindows[1, 2] = cameraWindow8;
			camWindows[1, 3] = cameraWindow9;
			camWindows[1, 4] = cameraWindow10;
			// row 3
			camWindows[2, 0] = cameraWindow11;
			camWindows[2, 1] = cameraWindow12;
			camWindows[2, 2] = cameraWindow13;
			camWindows[2, 3] = cameraWindow14;
			camWindows[2, 4] = cameraWindow15;
			// row 4
			camWindows[3, 0] = cameraWindow16;
			camWindows[3, 1] = cameraWindow17;
			camWindows[3, 2] = cameraWindow18;
			camWindows[3, 3] = cameraWindow19;
			camWindows[3, 4] = cameraWindow20;
			// row 5
			camWindows[4, 0] = cameraWindow21;
			camWindows[4, 1] = cameraWindow22;
			camWindows[4, 2] = cameraWindow23;
			camWindows[4, 3] = cameraWindow24;
			camWindows[4, 4] = cameraWindow25;
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
			this.cameraWindow1 = new CameraViewer.CameraWindow();
			this.cameraWindow2 = new CameraViewer.CameraWindow();
			this.cameraWindow3 = new CameraViewer.CameraWindow();
			this.cameraWindow4 = new CameraViewer.CameraWindow();
			this.cameraWindow5 = new CameraViewer.CameraWindow();
			this.cameraWindow6 = new CameraViewer.CameraWindow();
			this.cameraWindow7 = new CameraViewer.CameraWindow();
			this.cameraWindow8 = new CameraViewer.CameraWindow();
			this.cameraWindow9 = new CameraViewer.CameraWindow();
			this.cameraWindow10 = new CameraViewer.CameraWindow();
			this.cameraWindow11 = new CameraViewer.CameraWindow();
			this.cameraWindow12 = new CameraViewer.CameraWindow();
			this.cameraWindow13 = new CameraViewer.CameraWindow();
			this.cameraWindow14 = new CameraViewer.CameraWindow();
			this.cameraWindow15 = new CameraViewer.CameraWindow();
			this.cameraWindow16 = new CameraViewer.CameraWindow();
			this.cameraWindow17 = new CameraViewer.CameraWindow();
			this.cameraWindow18 = new CameraViewer.CameraWindow();
			this.cameraWindow19 = new CameraViewer.CameraWindow();
			this.cameraWindow20 = new CameraViewer.CameraWindow();
			this.cameraWindow21 = new CameraViewer.CameraWindow();
			this.cameraWindow22 = new CameraViewer.CameraWindow();
			this.cameraWindow23 = new CameraViewer.CameraWindow();
			this.cameraWindow24 = new CameraViewer.CameraWindow();
			this.cameraWindow25 = new CameraViewer.CameraWindow();
			this.SuspendLayout();
			// 
			// cameraWindow1
			// 
			this.cameraWindow1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.cameraWindow1.Camera = null;
			this.cameraWindow1.Location = new System.Drawing.Point(285, 17);
			this.cameraWindow1.Name = "cameraWindow1";
			this.cameraWindow1.Size = new System.Drawing.Size(75, 64);
			this.cameraWindow1.TabIndex = 0;
			this.cameraWindow1.Text = "cameraWindow1";
			this.cameraWindow1.Visible = false;
			this.cameraWindow1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cameraWindow_MouseDown);
			// 
			// cameraWindow2
			// 
			this.cameraWindow2.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.cameraWindow2.Camera = null;
			this.cameraWindow2.Location = new System.Drawing.Point(151, 17);
			this.cameraWindow2.Name = "cameraWindow2";
			this.cameraWindow2.Size = new System.Drawing.Size(75, 64);
			this.cameraWindow2.TabIndex = 1;
			this.cameraWindow2.Text = "cameraWindow2";
			this.cameraWindow2.Visible = false;
			this.cameraWindow2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cameraWindow_MouseDown);
			// 
			// cameraWindow3
			// 
			this.cameraWindow3.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.cameraWindow3.Camera = null;
			this.cameraWindow3.Location = new System.Drawing.Point(419, 17);
			this.cameraWindow3.Name = "cameraWindow3";
			this.cameraWindow3.Size = new System.Drawing.Size(75, 64);
			this.cameraWindow3.TabIndex = 2;
			this.cameraWindow3.Text = "cameraWindow3";
			this.cameraWindow3.Visible = false;
			this.cameraWindow3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cameraWindow_MouseDown);
			// 
			// cameraWindow4
			// 
			this.cameraWindow4.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.cameraWindow4.Camera = null;
			this.cameraWindow4.Location = new System.Drawing.Point(553, 17);
			this.cameraWindow4.Name = "cameraWindow4";
			this.cameraWindow4.Size = new System.Drawing.Size(75, 64);
			this.cameraWindow4.TabIndex = 3;
			this.cameraWindow4.Text = "cameraWindow4";
			this.cameraWindow4.Visible = false;
			this.cameraWindow4.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cameraWindow_MouseDown);
			// 
			// cameraWindow5
			// 
			this.cameraWindow5.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.cameraWindow5.Camera = null;
			this.cameraWindow5.Location = new System.Drawing.Point(17, 54);
			this.cameraWindow5.Name = "cameraWindow5";
			this.cameraWindow5.Size = new System.Drawing.Size(75, 64);
			this.cameraWindow5.TabIndex = 4;
			this.cameraWindow5.Text = "cameraWindow5";
			this.cameraWindow5.Visible = false;
			this.cameraWindow5.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cameraWindow_MouseDown);
			// 
			// cameraWindow6
			// 
			this.cameraWindow6.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.cameraWindow6.Camera = null;
			this.cameraWindow6.Location = new System.Drawing.Point(17, 91);
			this.cameraWindow6.Name = "cameraWindow6";
			this.cameraWindow6.Size = new System.Drawing.Size(75, 64);
			this.cameraWindow6.TabIndex = 5;
			this.cameraWindow6.Text = "cameraWindow6";
			this.cameraWindow6.Visible = false;
			this.cameraWindow6.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cameraWindow_MouseDown);
			// 
			// cameraWindow7
			// 
			this.cameraWindow7.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.cameraWindow7.Camera = null;
			this.cameraWindow7.Location = new System.Drawing.Point(17, 91);
			this.cameraWindow7.Name = "cameraWindow7";
			this.cameraWindow7.Size = new System.Drawing.Size(75, 64);
			this.cameraWindow7.TabIndex = 6;
			this.cameraWindow7.Text = "cameraWindow7";
			this.cameraWindow7.Visible = false;
			this.cameraWindow7.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cameraWindow_MouseDown);
			// 
			// cameraWindow8
			// 
			this.cameraWindow8.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.cameraWindow8.Camera = null;
			this.cameraWindow8.Location = new System.Drawing.Point(17, 91);
			this.cameraWindow8.Name = "cameraWindow8";
			this.cameraWindow8.Size = new System.Drawing.Size(75, 64);
			this.cameraWindow8.TabIndex = 7;
			this.cameraWindow8.Text = "cameraWindow8";
			this.cameraWindow8.Visible = false;
			this.cameraWindow8.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cameraWindow_MouseDown);
			// 
			// cameraWindow9
			// 
			this.cameraWindow9.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.cameraWindow9.Camera = null;
			this.cameraWindow9.Location = new System.Drawing.Point(151, 91);
			this.cameraWindow9.Name = "cameraWindow9";
			this.cameraWindow9.Size = new System.Drawing.Size(75, 64);
			this.cameraWindow9.TabIndex = 8;
			this.cameraWindow9.Text = "cameraWindow9";
			this.cameraWindow9.Visible = false;
			this.cameraWindow9.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cameraWindow_MouseDown);
			// 
			// cameraWindow10
			// 
			this.cameraWindow10.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.cameraWindow10.Camera = null;
			this.cameraWindow10.Location = new System.Drawing.Point(328, 80);
			this.cameraWindow10.Name = "cameraWindow10";
			this.cameraWindow10.Size = new System.Drawing.Size(75, 64);
			this.cameraWindow10.TabIndex = 9;
			this.cameraWindow10.Text = "cameraWindow10";
			this.cameraWindow10.Visible = false;
			this.cameraWindow10.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cameraWindow_MouseDown);
			// 
			// cameraWindow11
			// 
			this.cameraWindow11.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.cameraWindow11.Camera = null;
			this.cameraWindow11.Location = new System.Drawing.Point(8, 152);
			this.cameraWindow11.Name = "cameraWindow11";
			this.cameraWindow11.Size = new System.Drawing.Size(75, 64);
			this.cameraWindow11.TabIndex = 10;
			this.cameraWindow11.Text = "cameraWindow11";
			this.cameraWindow11.Visible = false;
			this.cameraWindow11.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cameraWindow_MouseDown);
			// 
			// cameraWindow12
			// 
			this.cameraWindow12.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.cameraWindow12.Camera = null;
			this.cameraWindow12.Location = new System.Drawing.Point(88, 152);
			this.cameraWindow12.Name = "cameraWindow12";
			this.cameraWindow12.Size = new System.Drawing.Size(75, 64);
			this.cameraWindow12.TabIndex = 11;
			this.cameraWindow12.Text = "cameraWindow12";
			this.cameraWindow12.Visible = false;
			this.cameraWindow12.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cameraWindow_MouseDown);
			// 
			// cameraWindow13
			// 
			this.cameraWindow13.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.cameraWindow13.Camera = null;
			this.cameraWindow13.Location = new System.Drawing.Point(228, 152);
			this.cameraWindow13.Name = "cameraWindow13";
			this.cameraWindow13.Size = new System.Drawing.Size(75, 64);
			this.cameraWindow13.TabIndex = 12;
			this.cameraWindow13.Text = "cameraWindow13";
			this.cameraWindow13.Visible = false;
			this.cameraWindow13.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cameraWindow_MouseDown);
			// 
			// cameraWindow14
			// 
			this.cameraWindow14.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.cameraWindow14.Camera = null;
			this.cameraWindow14.Location = new System.Drawing.Point(248, 152);
			this.cameraWindow14.Name = "cameraWindow14";
			this.cameraWindow14.Size = new System.Drawing.Size(75, 64);
			this.cameraWindow14.TabIndex = 13;
			this.cameraWindow14.Text = "cameraWindow14";
			this.cameraWindow14.Visible = false;
			this.cameraWindow14.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cameraWindow_MouseDown);
			// 
			// cameraWindow15
			// 
			this.cameraWindow15.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.cameraWindow15.Camera = null;
			this.cameraWindow15.Location = new System.Drawing.Point(388, 152);
			this.cameraWindow15.Name = "cameraWindow15";
			this.cameraWindow15.Size = new System.Drawing.Size(75, 64);
			this.cameraWindow15.TabIndex = 14;
			this.cameraWindow15.Text = "cameraWindow15";
			this.cameraWindow15.Visible = false;
			this.cameraWindow15.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cameraWindow_MouseDown);
			// 
			// cameraWindow16
			// 
			this.cameraWindow16.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.cameraWindow16.Camera = null;
			this.cameraWindow16.Location = new System.Drawing.Point(528, 152);
			this.cameraWindow16.Name = "cameraWindow16";
			this.cameraWindow16.Size = new System.Drawing.Size(75, 64);
			this.cameraWindow16.TabIndex = 15;
			this.cameraWindow16.Text = "cameraWindow16";
			this.cameraWindow16.Visible = false;
			this.cameraWindow16.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cameraWindow_MouseDown);
			// 
			// cameraWindow17
			// 
			this.cameraWindow17.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.cameraWindow17.Camera = null;
			this.cameraWindow17.Location = new System.Drawing.Point(17, 189);
			this.cameraWindow17.Name = "cameraWindow17";
			this.cameraWindow17.Size = new System.Drawing.Size(75, 64);
			this.cameraWindow17.TabIndex = 16;
			this.cameraWindow17.Text = "cameraWindow17";
			this.cameraWindow17.Visible = false;
			this.cameraWindow17.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cameraWindow_MouseDown);
			// 
			// cameraWindow18
			// 
			this.cameraWindow18.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.cameraWindow18.Camera = null;
			this.cameraWindow18.Location = new System.Drawing.Point(157, 189);
			this.cameraWindow18.Name = "cameraWindow18";
			this.cameraWindow18.Size = new System.Drawing.Size(75, 64);
			this.cameraWindow18.TabIndex = 17;
			this.cameraWindow18.Text = "cameraWindow18";
			this.cameraWindow18.Visible = false;
			this.cameraWindow18.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cameraWindow_MouseDown);
			// 
			// cameraWindow19
			// 
			this.cameraWindow19.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.cameraWindow19.Camera = null;
			this.cameraWindow19.Location = new System.Drawing.Point(297, 189);
			this.cameraWindow19.Name = "cameraWindow19";
			this.cameraWindow19.Size = new System.Drawing.Size(75, 64);
			this.cameraWindow19.TabIndex = 18;
			this.cameraWindow19.Text = "cameraWindow19";
			this.cameraWindow19.Visible = false;
			this.cameraWindow19.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cameraWindow_MouseDown);
			// 
			// cameraWindow20
			// 
			this.cameraWindow20.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.cameraWindow20.Camera = null;
			this.cameraWindow20.Location = new System.Drawing.Point(17, 261);
			this.cameraWindow20.Name = "cameraWindow20";
			this.cameraWindow20.Size = new System.Drawing.Size(75, 64);
			this.cameraWindow20.TabIndex = 19;
			this.cameraWindow20.Text = "cameraWindow20";
			this.cameraWindow20.Visible = false;
			this.cameraWindow20.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cameraWindow_MouseDown);
			// 
			// cameraWindow21
			// 
			this.cameraWindow21.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.cameraWindow21.Camera = null;
			this.cameraWindow21.Location = new System.Drawing.Point(17, 298);
			this.cameraWindow21.Name = "cameraWindow21";
			this.cameraWindow21.Size = new System.Drawing.Size(75, 64);
			this.cameraWindow21.TabIndex = 20;
			this.cameraWindow21.Text = "cameraWindow21";
			this.cameraWindow21.Visible = false;
			this.cameraWindow21.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cameraWindow_MouseDown);
			// 
			// cameraWindow22
			// 
			this.cameraWindow22.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.cameraWindow22.Camera = null;
			this.cameraWindow22.Location = new System.Drawing.Point(17, 335);
			this.cameraWindow22.Name = "cameraWindow22";
			this.cameraWindow22.Size = new System.Drawing.Size(75, 64);
			this.cameraWindow22.TabIndex = 21;
			this.cameraWindow22.Text = "cameraWindow22";
			this.cameraWindow22.Visible = false;
			this.cameraWindow22.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cameraWindow_MouseDown);
			// 
			// cameraWindow23
			// 
			this.cameraWindow23.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.cameraWindow23.Camera = null;
			this.cameraWindow23.Location = new System.Drawing.Point(17, 335);
			this.cameraWindow23.Name = "cameraWindow23";
			this.cameraWindow23.Size = new System.Drawing.Size(75, 64);
			this.cameraWindow23.TabIndex = 22;
			this.cameraWindow23.Text = "cameraWindow23";
			this.cameraWindow23.Visible = false;
			this.cameraWindow23.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cameraWindow_MouseDown);
			// 
			// cameraWindow24
			// 
			this.cameraWindow24.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.cameraWindow24.Camera = null;
			this.cameraWindow24.Location = new System.Drawing.Point(17, 335);
			this.cameraWindow24.Name = "cameraWindow24";
			this.cameraWindow24.Size = new System.Drawing.Size(75, 64);
			this.cameraWindow24.TabIndex = 23;
			this.cameraWindow24.Text = "cameraWindow24";
			this.cameraWindow24.Visible = false;
			this.cameraWindow24.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cameraWindow_MouseDown);
			// 
			// cameraWindow25
			// 
			this.cameraWindow25.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.cameraWindow25.Camera = null;
			this.cameraWindow25.Location = new System.Drawing.Point(17, 335);
			this.cameraWindow25.Name = "cameraWindow25";
			this.cameraWindow25.Size = new System.Drawing.Size(75, 64);
			this.cameraWindow25.TabIndex = 24;
			this.cameraWindow25.Text = "cameraWindow25";
			this.cameraWindow25.Visible = false;
			this.cameraWindow25.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cameraWindow_MouseDown);
			// 
			// Multiplexer
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.cameraWindow25,
																		  this.cameraWindow24,
																		  this.cameraWindow23,
																		  this.cameraWindow22,
																		  this.cameraWindow21,
																		  this.cameraWindow20,
																		  this.cameraWindow19,
																		  this.cameraWindow18,
																		  this.cameraWindow17,
																		  this.cameraWindow16,
																		  this.cameraWindow15,
																		  this.cameraWindow14,
																		  this.cameraWindow13,
																		  this.cameraWindow12,
																		  this.cameraWindow11,
																		  this.cameraWindow10,
																		  this.cameraWindow9,
																		  this.cameraWindow8,
																		  this.cameraWindow7,
																		  this.cameraWindow6,
																		  this.cameraWindow5,
																		  this.cameraWindow4,
																		  this.cameraWindow3,
																		  this.cameraWindow2,
																		  this.cameraWindow1});
			this.Size = new System.Drawing.Size(424, 376);
			this.Resize += new System.EventHandler(this.Multiplexer_Resize);
			this.ResumeLayout(false);

		}
		#endregion

		
		// Close all cameras
		public void CloseAll()
		{
			for (int i = 0; i < MaxRows; i++)
			{
				for (int j = 0; j < MaxCols; j++)
				{
					camWindows[i, j].Camera = null;
				}
			}
		}

		// Set camera to the specified position of the multiplexer
		public void SetCamera(int row, int col, Camera camera)
		{
			if ((row >= 0) && (col >= 0) && (row < MaxRows) && (col < MaxCols))
			{
				camWindows[row, col].Camera = camera;
			}
		}

		// Set multiplexer size
		public void SetSize(int rows, int cols, int cellWidth, int cellHeight)
		{
			this.rows = rows;
			this.cols = cols;
			this.cellWidth = cellWidth;
			this.cellHeight = cellHeight;
			UpdateSize();
		}

		// Update cameras visibility
		private void UpdateVisiblity()
		{
			if (camerasVisible)
			{
				for (int i = 0; i < MaxRows; i++)
				{
					for (int j = 0; j < MaxCols; j++)
					{
						camWindows[i, j].Visible = ((i < rows) && (j < cols));
					}
				}
			}
		}

		// Update cameras size and position
		private void UpdateSize()
		{
			int width, height;

			if (!fitToWindow)
			{
				// original width & height
				width = cellWidth;
				height = cellHeight;
			}
			else
			{
				// calculate width & height of cameras to fit the view to the window
				width = (ClientRectangle.Width / cols) - 4;
				height = (ClientRectangle.Height / rows) - 4;
			}

			// starting position of the view
			int startX = (ClientRectangle.Width - cols * (width + 4)) / 2;
			int startY = (ClientRectangle.Height - rows * (height + 4)) / 2;

			this.SuspendLayout();

			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < cols; j++)
				{
					camWindows[i, j].Location = new Point(startX + (width + 4) * j + 1, startY + (height + 4) * i + 1);
					camWindows[i, j].Size = new Size(width + 2, height + 2);
				}
			}

			this.ResumeLayout(false);
		}

		// On size changed
		private void Multiplexer_Resize(object sender, System.EventArgs e)
		{
			UpdateSize();
		}

		// On mouse down in camera window
		private void cameraWindow_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			lastClicked = (CameraWindow) sender;
		}
	}
}
