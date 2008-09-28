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
	/// Summary description for ViewStructure.
	/// </summary>
	public class ViewStructure : System.Windows.Forms.UserControl, IWizardPage
	{
		private View view = null;
		private bool completed = true;
		private GroupCollection groups;
		private CameraCollection cameras;

		private CameraViewer.CamerasTreeView camerasTree;
		private System.Windows.Forms.ImageList imageList;
		private CameraViewer.ViewGrid viewGrid;
		private System.Windows.Forms.ContextMenu contextMenu;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.ComponentModel.IContainer components;

		// state changed event
		public event EventHandler StateChanged;
		// reset event
		public event EventHandler Reset;

		// View property
		public View View
		{
			get { return view; }
			set
			{
				if (value != null)
				{
					view = value;

					// ..
				}
			}
		}

		// Constructor
		public ViewStructure()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitForm call
			this.camerasTree.Init();			
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
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ViewStructure));
			this.camerasTree = new CameraViewer.CamerasTreeView();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.viewGrid = new CameraViewer.ViewGrid();
			this.contextMenu = new System.Windows.Forms.ContextMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.SuspendLayout();
			// 
			// camerasTree
			// 
			this.camerasTree.AllowDrop = true;
			this.camerasTree.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left);
			this.camerasTree.CameraImage = 1;
			this.camerasTree.CameraSelectedImage = 1;
			this.camerasTree.CamerasOnly = true;
			this.camerasTree.ImageList = this.imageList;
			this.camerasTree.Location = new System.Drawing.Point(10, 10);
			this.camerasTree.Name = "camerasTree";
			this.camerasTree.Size = new System.Drawing.Size(180, 160);
			this.camerasTree.Sorted = true;
			this.camerasTree.TabIndex = 0;
			this.camerasTree.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.camerasTree_ItemDrag);
			// 
			// imageList
			// 
			this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			this.imageList.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
			this.imageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// viewGrid
			// 
			this.viewGrid.AllowDrop = true;
			this.viewGrid.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.viewGrid.Cols = ((short)(2));
			this.viewGrid.ContextMenu = this.contextMenu;
			this.viewGrid.Location = new System.Drawing.Point(200, 10);
			this.viewGrid.Name = "viewGrid";
			this.viewGrid.Rows = ((short)(2));
			this.viewGrid.Size = new System.Drawing.Size(220, 160);
			this.viewGrid.TabIndex = 1;
			this.viewGrid.DragDrop += new System.Windows.Forms.DragEventHandler(this.viewGrid_DragDrop);
			this.viewGrid.DragOver += new System.Windows.Forms.DragEventHandler(this.viewGrid_DragOver);
			// 
			// contextMenu
			// 
			this.contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						this.menuItem1});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.Text = "Clear";
			this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
			// 
			// ViewStructure
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.viewGrid,
																		  this.camerasTree});
			this.Name = "ViewStructure";
			this.Size = new System.Drawing.Size(430, 180);
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.viewGrid_DragDrop);
			this.DragOver += new System.Windows.Forms.DragEventHandler(this.viewGrid_DragOver);
			this.ResumeLayout(false);

		}
		#endregion

		// PageName property
		public string PageName
		{
			get { return "Structure"; }
		}

		// PageDescription property
		public string PageDescription
		{
			get { return "View structure"; }
		}

		// Completed property
		public bool Completed
		{
			get { return completed; }
		}

		// Show the page
		public void Display()
		{
			viewGrid.Rows = view.Rows;
			viewGrid.Cols = view.Cols;

			UpdateGridLabels();

			camerasTree.Focus();
		}

		// Apply the page
		public bool Apply()
		{
			return true;
		}

		// Build cameras tree
		public void BuildCamerasTree(GroupCollection groups, CameraCollection cameras)
		{
			this.groups = groups;
			this.cameras = cameras;
			this.camerasTree.BuildCamerasTree(groups, cameras);
			UpdateGridLabels();
		}

		// Update grid labels
		private void UpdateGridLabels()
		{
			if (cameras != null)
			{
				for (int i = 0; i < viewGrid.Rows; i++)
				{
					for (int j = 0; j < viewGrid.Cols; j++)
					{
						// get camera ID of the specified cell of the grid
						int cameraID = view.GetCamera(i, j);

						if (cameraID > 0)
						{
							Camera c = cameras.GetCamera(cameraID);

							if (c != null)
								viewGrid.SetLabel(c.FullName, i, j);
						}
					}
				}
			}
		}

		// On dragging beggin
		private void camerasTree_ItemDrag(object sender, System.Windows.Forms.ItemDragEventArgs e)
		{
			TreeNode dragNode = (TreeNode) e.Item;

			// check for camera item
			if (camerasTree.GetNodeType(dragNode) == NodeType.Camera)
			{
				// drag it
				camerasTree.DoDragDrop(camerasTree.GetCameraFullName(dragNode), DragDropEffects.Copy);
			}
		}

		// On dragging onject over view grid
		private void viewGrid_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
		{

			e.Effect = DragDropEffects.Copy;
		}

		// On dragging object dropperd
		private void viewGrid_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
			string		name = (string) e.Data.GetData(typeof(string));
			string[]	nameParts = name.Split('\\');
			Group		group = null;
			Camera		camera = null;

			// get group
			if (nameParts.Length > 1)
				group = groups.GetGroupByName(string.Join("\\", nameParts, 0, nameParts.Length - 1));

			// get camera
			camera = cameras.GetCamera(nameParts[nameParts.Length - 1], group);

			if (camera != null)
			{
				Point cpt = viewGrid.PointToClient(new Point(e.X, e.Y));
				Point pt = viewGrid.SetLabel(name, cpt);

				// set camera
				view.SetCamera(pt.Y, pt.X, camera.ID);
			}
		}

		// On context menu
		private void menuItem1_Click(object sender, System.EventArgs e)
		{
			Point pt = viewGrid.SetLabel(null);

			// set camera
			view.SetCamera(pt.Y, pt.X, 0);
		}
	}
}
