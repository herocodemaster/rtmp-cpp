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
	public enum NodeType
	{
		Unknown,
		CamerasRootGroup,
		CamerasGroup,
		Camera,
		ViewsRootGroup,
		ViewsGroup,
		View
	}

	/// <summary>
	/// Summary description for CamerasTreeView.
	/// </summary>
	public class CamerasTreeView : System.Windows.Forms.TreeView
	{
		private TreeNode lastClickNode;
		private TreeNode camerasRootNode;
		private TreeNode viewsRootNode;
		private bool camerasOnly = false;

		private int camerasFolderImage = 0;
		private int camerasFolderSelectedImage = 0;
		private int cameraImage = 0;
		private int cameraSelectedImage = 0;

		private int viewsFolderImage = 0;
		private int viewsFolderSelectedImage = 0;
		private int viewImage = 0;
		private int viewSelectedImage = 0;

		private System.ComponentModel.IContainer components;

		// CamerasOnly - display cameras only
		[DefaultValue(false)]
		public bool CamerasOnly
		{
			get { return camerasOnly; }
			set { camerasOnly = value; }
		}

		// CamerasFolderImage - image index for camera folders
		[DefaultValue(0)]
		public int CamerasFolderImage
		{
			get { return camerasFolderImage; }
			set { camerasFolderImage = value; }
		}

		// CamerasFolderSelectedImage - image index for selected camera folders
		[DefaultValue(0)]
		public int CamerasFolderSelectedImage
		{
			get { return camerasFolderSelectedImage; }
			set { camerasFolderSelectedImage = value; }
		}

		// CameraImage - image index for cameras
		[DefaultValue(0)]
		public int CameraImage
		{
			get { return cameraImage; }
			set { cameraImage = value; }
		}

		// CameraSelectedImage - image index for selected cameras
		[DefaultValue(0)]
		public int CameraSelectedImage
		{
			get { return cameraSelectedImage; }
			set { cameraSelectedImage = value; }
		}

		// ViewsFolderImage - image index for view folders
		[DefaultValue(0)]
		public int ViewsFolderImage
		{
			get { return viewsFolderImage; }
			set { viewsFolderImage = value; }
		}

		// ViewsFolderSelectedImage - image index for selected view folders
		[DefaultValue(0)]
		public int ViewsFolderSelectedImage
		{
			get { return viewsFolderSelectedImage; }
			set { viewsFolderSelectedImage = value; }
		}

		// ViewImage - image index for views
		[DefaultValue(0)]
		public int ViewImage
		{
			get { return viewImage; }
			set { viewImage = value; }
		}

		// ViewSelectedImage - image index for selected views
		[DefaultValue(0)]
		public int ViewSelectedImage
		{
			get { return viewSelectedImage; }
			set { viewSelectedImage = value; }
		}

		// LastClickNode property
		[Browsable(false)]
		public TreeNode LastClickNode
		{
			get { return lastClickNode; }
		}

		// CamerasRootNode property
		[Browsable(false)]
		public TreeNode CamerasRootNode
		{
			get { return camerasRootNode; }
		}

		// ViewsRootNode property
		[Browsable(false)]
		public TreeNode ViewsRootNode
		{
			get { return viewsRootNode; }
		}


		// Constructor
		public CamerasTreeView()
		{
			InitializeComponent();
		}

		// Initialize control
		public void Init()
		{
			if (!camerasOnly)
			{
				if (camerasRootNode == null)
				{
					// create cameras root node
					camerasRootNode = new TreeNode("Cameras", camerasFolderImage, camerasFolderSelectedImage);
					// add to tree
					this.Nodes.Add(camerasRootNode);
				}
				if (viewsRootNode == null)
				{
					// create views root node
					viewsRootNode = new TreeNode("Views", viewsFolderImage, viewsFolderSelectedImage);
					// add to tree
					this.Nodes.Add(viewsRootNode);
				}
			}
		}

		private void InitializeComponent()
		{
			// 
			// CamerasTreeView
			// 
			this.ImageIndex = 0;
			this.SelectedImageIndex = 0;
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CamerasTreeView_MouseDown);

		}

		// On mouse down
		private void CamerasTreeView_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			lastClickNode = this.GetNodeAt(e.X, e.Y);
		}

		// Selected node type
		public NodeType GetNodeType(TreeNode node)
		{
			if (node == null)
				return NodeType.Unknown;

			// check image index			
			if (node.ImageIndex == camerasFolderImage)
			{
				// cameras groups
				if (node == camerasRootNode)
				{
					return NodeType.CamerasRootGroup;
				}
				return NodeType.CamerasGroup;
			}
			else if (node.ImageIndex == cameraImage)
			{
				// camera
				return NodeType.Camera;
			}
			else if (node.ImageIndex == viewsFolderImage)
			{
				// views groups
				if (node == viewsRootNode)
				{
					return NodeType.ViewsRootGroup;
				}
				return NodeType.ViewsGroup;
			}
			else if (node.ImageIndex == viewImage)
			{
				// view
				return NodeType.View;
			}

			return NodeType.Unknown;
		}

		// Add camera group
		public TreeNode AddCamerasGroup(Group group, TreeNode parentNode)
		{
			// create new node
			TreeNode node = new TreeNode(group.Name, camerasFolderImage, camerasFolderSelectedImage);
			// add it to tree
			if (parentNode == null)
				Nodes.Add(node);
			else
				parentNode.Nodes.Add(node);

			return node;
		}

		// Add camera
		public TreeNode AddCamera(Camera camera, TreeNode parentNode)
		{
			// create new 
			TreeNode node = new TreeNode(camera.Name, cameraImage, cameraSelectedImage);
			// add it to tree
			if (parentNode == null)
				Nodes.Add(node);
			else
				parentNode.Nodes.Add(node);

			return node;
		}

		// Add views group
		public TreeNode AddViewsGroup(Group group, TreeNode parentNode)
		{
			// create new node
			TreeNode node = new TreeNode(group.Name, viewsFolderImage, viewsFolderSelectedImage);
			// add it to tree
			parentNode.Nodes.Add(node);

			return node;
		}

		// Add view
		public TreeNode AddView(View view, TreeNode parentNode)
		{
			// create new 
			TreeNode node = new TreeNode(view.Name, viewImage, viewSelectedImage);
			// add it to tree
			parentNode.Nodes.Add(node);

			return node;
		}

		// Get full name of the specified cameras or views group
		public string GetGroupFullName(TreeNode node)
		{
			// check if the node is a node of cameras or views group
			if (((node.ImageIndex != camerasFolderImage) || (node == camerasRootNode)) &&
				((node.ImageIndex != viewsFolderImage) || (node == viewsRootNode)))
			{
				return null;
			}
			return (camerasOnly) ? node.FullPath : node.FullPath.Substring(node.FullPath.IndexOf(this.PathSeparator) + 1);
		}

		// Get full name of the specified camera node
		public string GetCameraFullName(TreeNode node)
		{
			// check if the node is a node of camera
			if (node.ImageIndex != cameraImage)
			{
				return null;
			}
			return (camerasOnly) ? node.FullPath : node.FullPath.Substring(node.FullPath.IndexOf(this.PathSeparator) + 1);
		}

		// Get full name of the specified view node
		public string GetViewFullName(TreeNode node)
		{
			// check if the node is a node of view
			if (node.ImageIndex != viewImage)
			{
				return null;
			}
			return node.FullPath.Substring(node.FullPath.IndexOf(this.PathSeparator) + 1);
		}

		// Build cameras subtree
		public void BuildCamerasTree(GroupCollection groups, CameraCollection cameras)
		{
			BuildCamerasTree(groups, cameras, null, (camerasOnly) ? null : camerasRootNode);
			if (!camerasOnly)
				camerasRootNode.Expand();
		}

		// Build cameras of specified parent
		private void BuildCamerasTree(GroupCollection groups, CameraCollection cameras, Group parent, TreeNode parentNode)
		{
			// build all groups
			foreach (Group group in groups)
			{
				if (group.Parent == parent)
				{
					BuildCamerasTree(groups, cameras, group, AddCamerasGroup(group, parentNode));
				}
			}
			// build all cameras
			foreach (Camera camera in cameras)
			{
				if (camera.Parent == parent)
				{
					AddCamera(camera, parentNode);
				}
			}
		}

		// Build views subtree
		public void BuildViewsTree(GroupCollection groups, ViewCollection views)
		{
			if (!camerasOnly)
			{
				BuildViewsTree(groups, views, null, viewsRootNode);
				camerasRootNode.Expand();
			}
		}

		// Build views of specified parent
		private void BuildViewsTree(GroupCollection groups, ViewCollection views, Group parent, TreeNode parentNode)
		{
			// build all groups
			foreach (Group group in groups)
			{
				if (group.Parent == parent)
				{
					BuildViewsTree(groups, views, group, AddViewsGroup(group, parentNode));
				}
			}
			// build all views
			foreach (View view in views)
			{
				if (view.Parent == parent)
				{
					AddView(view, parentNode);
				}
			}
		}
	}
}
