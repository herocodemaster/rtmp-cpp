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
using System.Data;
using System.IO;
using System.Timers;
using System.Threading;

namespace CameraViewer
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
		private static string title = "Camera Vision";
		private Configuration config = new Configuration(Path.GetDirectoryName(Application.ExecutablePath));
		private RunningPool runningPool = new RunningPool();
		private FinalizationPool finalizationPool = new FinalizationPool();

		//
		CameraInfo			cameraInfo;

		// statistics
		private const int	statLength = 15;
		private int			statIndex = 0, statReady = 0;
		private long[]		statReceived = new long[statLength];
		private int[]		statCount = new int[statLength];

		private Camera		cameraToEdit;
		private View		viewToEdit;
		private TreeNode	nodeToEdit;

		private int			openedID;
		private bool		viewOpened;

		private System.Windows.Forms.MainMenu mainMenu;
		private System.Windows.Forms.MenuItem fileItem;
		private System.Windows.Forms.MenuItem exitFileItem;
		private System.Windows.Forms.MenuItem helpItem;
		private System.Windows.Forms.MenuItem aboutHelpItem;
		private System.Windows.Forms.StatusBar statusBar;
		private System.Windows.Forms.ToolBarButton camerasBarButton;
		private System.Windows.Forms.Panel mainPanel;
		private System.Windows.Forms.ToolBar toolBar;
		private System.Windows.Forms.MenuItem viewItem;
		private System.Windows.Forms.MenuItem camerasBarViewItem;
		private System.Windows.Forms.MenuItem camerasItem;
		private System.Windows.Forms.MenuItem addGroupCamerasItem;
		private System.Windows.Forms.MenuItem editGroupCamerasItem;
		private System.Windows.Forms.MenuItem deleteGroupCamerasItem;
		private System.Windows.Forms.ImageList toolBarImageList;
		private System.Windows.Forms.ContextMenu camerasContextMenu;
		private System.Windows.Forms.MenuItem addGroupItem;
		private System.Windows.Forms.MenuItem editGroupItem;
		private System.Windows.Forms.MenuItem deleteGroupItem;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem addCameraCamerasItem;
		private System.Windows.Forms.MenuItem editCameraCamerasItem;
		private System.Windows.Forms.MenuItem deleteCameraCamerasItem;
		private System.Windows.Forms.ToolBarButton sep1;
		private System.Windows.Forms.ToolBarButton fitToScreenButton;
		private System.Windows.Forms.ToolBarButton fullScreenButton;
		private System.Windows.Forms.MenuItem addCameraItem;
		private System.Windows.Forms.MenuItem deleteCameraItem;
		private System.Windows.Forms.MenuItem editCameraItem;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem openCameraCamerasItem;
		private System.Windows.Forms.MenuItem openCameraItem;
		private System.Windows.Forms.MenuItem sep1Item;
		private System.Windows.Forms.MenuItem sep2Item;
		private System.Windows.Forms.ContextMenu cameraContextMenu;
		private System.Windows.Forms.MenuItem closeCameraItem;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem fitToScreenViewItem;
		private System.Windows.Forms.MenuItem fullScreenViewItem;
		private System.Windows.Forms.ToolBarButton sep2;
		private System.Windows.Forms.ToolBarButton aboutButton;
		private System.Windows.Forms.ToolBarButton closeViewButton;
		private System.Windows.Forms.ToolBarButton sep3;
		private System.Windows.Forms.StatusBarPanel infoPanel;
		private System.Windows.Forms.StatusBarPanel fpsPanel;
		private System.Windows.Forms.StatusBarPanel bpsPanel;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem fitToScreenCameraItem;
		private System.Windows.Forms.MenuItem fullScreenCameraItem;
		private System.Windows.Forms.ContextMenu viewContextMenu;
		private System.Windows.Forms.MenuItem fitToSreenItem;
		private System.Windows.Forms.MenuItem fullScreenItem;
		private System.Windows.Forms.MenuItem viewsItem;
		private System.Windows.Forms.MenuItem addGroupViewsItem;
		private System.Windows.Forms.MenuItem editGroupViewsItem;
		private System.Windows.Forms.MenuItem deleteGroupViewsItem;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.MenuItem addViewViewsItem;
		private System.Windows.Forms.MenuItem editViewViewsItem;
		private System.Windows.Forms.MenuItem deleteViewViewsItem;
		private System.Windows.Forms.MenuItem sep3Item;
		private System.Windows.Forms.MenuItem addViewItem;
		private System.Windows.Forms.MenuItem editViewItem;
		private System.Windows.Forms.MenuItem deleteViewItem;
		private CameraViewer.CamerasTreeView camerasTree;
		private System.Windows.Forms.Splitter splitter;
		private System.Windows.Forms.ImageList iconsList;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.MenuItem openViewViewsItem;
		private CameraViewer.Multiplexer multiplexer;
		private System.Windows.Forms.MenuItem closeFileItem;
		private System.Windows.Forms.MenuItem menuItem8;
		private System.Windows.Forms.MenuItem sep4Item;
		private System.Windows.Forms.MenuItem openViewItem;
		private System.Timers.Timer timer;
		private System.Windows.Forms.MenuItem menuItem7;
		private System.Windows.Forms.MenuItem cameraInfoItem;
		private System.ComponentModel.IContainer components;

		public MainForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			this.camerasTree.Init();

			cameraInfo = new CameraInfo();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
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
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainForm));
			this.mainMenu = new System.Windows.Forms.MainMenu();
			this.fileItem = new System.Windows.Forms.MenuItem();
			this.closeFileItem = new System.Windows.Forms.MenuItem();
			this.menuItem8 = new System.Windows.Forms.MenuItem();
			this.exitFileItem = new System.Windows.Forms.MenuItem();
			this.viewItem = new System.Windows.Forms.MenuItem();
			this.camerasBarViewItem = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.fitToScreenViewItem = new System.Windows.Forms.MenuItem();
			this.fullScreenViewItem = new System.Windows.Forms.MenuItem();
			this.camerasItem = new System.Windows.Forms.MenuItem();
			this.addGroupCamerasItem = new System.Windows.Forms.MenuItem();
			this.editGroupCamerasItem = new System.Windows.Forms.MenuItem();
			this.deleteGroupCamerasItem = new System.Windows.Forms.MenuItem();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.addCameraCamerasItem = new System.Windows.Forms.MenuItem();
			this.editCameraCamerasItem = new System.Windows.Forms.MenuItem();
			this.deleteCameraCamerasItem = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.openCameraCamerasItem = new System.Windows.Forms.MenuItem();
			this.viewsItem = new System.Windows.Forms.MenuItem();
			this.addGroupViewsItem = new System.Windows.Forms.MenuItem();
			this.editGroupViewsItem = new System.Windows.Forms.MenuItem();
			this.deleteGroupViewsItem = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.addViewViewsItem = new System.Windows.Forms.MenuItem();
			this.editViewViewsItem = new System.Windows.Forms.MenuItem();
			this.deleteViewViewsItem = new System.Windows.Forms.MenuItem();
			this.menuItem6 = new System.Windows.Forms.MenuItem();
			this.openViewViewsItem = new System.Windows.Forms.MenuItem();
			this.helpItem = new System.Windows.Forms.MenuItem();
			this.aboutHelpItem = new System.Windows.Forms.MenuItem();
			this.statusBar = new System.Windows.Forms.StatusBar();
			this.infoPanel = new System.Windows.Forms.StatusBarPanel();
			this.bpsPanel = new System.Windows.Forms.StatusBarPanel();
			this.fpsPanel = new System.Windows.Forms.StatusBarPanel();
			this.toolBarImageList = new System.Windows.Forms.ImageList(this.components);
			this.toolBar = new System.Windows.Forms.ToolBar();
			this.closeViewButton = new System.Windows.Forms.ToolBarButton();
			this.sep1 = new System.Windows.Forms.ToolBarButton();
			this.camerasBarButton = new System.Windows.Forms.ToolBarButton();
			this.sep2 = new System.Windows.Forms.ToolBarButton();
			this.fitToScreenButton = new System.Windows.Forms.ToolBarButton();
			this.fullScreenButton = new System.Windows.Forms.ToolBarButton();
			this.sep3 = new System.Windows.Forms.ToolBarButton();
			this.aboutButton = new System.Windows.Forms.ToolBarButton();
			this.mainPanel = new System.Windows.Forms.Panel();
			this.multiplexer = new CameraViewer.Multiplexer();
			this.cameraContextMenu = new System.Windows.Forms.ContextMenu();
			this.closeCameraItem = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.fitToScreenCameraItem = new System.Windows.Forms.MenuItem();
			this.fullScreenCameraItem = new System.Windows.Forms.MenuItem();
			this.viewContextMenu = new System.Windows.Forms.ContextMenu();
			this.fitToSreenItem = new System.Windows.Forms.MenuItem();
			this.fullScreenItem = new System.Windows.Forms.MenuItem();
			this.splitter = new System.Windows.Forms.Splitter();
			this.camerasTree = new CameraViewer.CamerasTreeView();
			this.camerasContextMenu = new System.Windows.Forms.ContextMenu();
			this.addGroupItem = new System.Windows.Forms.MenuItem();
			this.editGroupItem = new System.Windows.Forms.MenuItem();
			this.deleteGroupItem = new System.Windows.Forms.MenuItem();
			this.sep1Item = new System.Windows.Forms.MenuItem();
			this.addCameraItem = new System.Windows.Forms.MenuItem();
			this.editCameraItem = new System.Windows.Forms.MenuItem();
			this.deleteCameraItem = new System.Windows.Forms.MenuItem();
			this.sep2Item = new System.Windows.Forms.MenuItem();
			this.openCameraItem = new System.Windows.Forms.MenuItem();
			this.sep3Item = new System.Windows.Forms.MenuItem();
			this.addViewItem = new System.Windows.Forms.MenuItem();
			this.editViewItem = new System.Windows.Forms.MenuItem();
			this.deleteViewItem = new System.Windows.Forms.MenuItem();
			this.sep4Item = new System.Windows.Forms.MenuItem();
			this.openViewItem = new System.Windows.Forms.MenuItem();
			this.iconsList = new System.Windows.Forms.ImageList(this.components);
			this.timer = new System.Timers.Timer();
			this.menuItem7 = new System.Windows.Forms.MenuItem();
			this.cameraInfoItem = new System.Windows.Forms.MenuItem();
			((System.ComponentModel.ISupportInitialize)(this.infoPanel)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.bpsPanel)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.fpsPanel)).BeginInit();
			this.mainPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.timer)).BeginInit();
			this.SuspendLayout();
			// 
			// mainMenu
			// 
			this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.fileItem,
																					 this.viewItem,
																					 this.camerasItem,
																					 this.viewsItem,
																					 this.helpItem});
			// 
			// fileItem
			// 
			this.fileItem.Index = 0;
			this.fileItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.closeFileItem,
																					 this.menuItem8,
																					 this.exitFileItem});
			this.fileItem.Text = "&File";
			// 
			// closeFileItem
			// 
			this.closeFileItem.Index = 0;
			this.closeFileItem.Text = "&Close view";
			this.closeFileItem.Click += new System.EventHandler(this.closeFileItem_Click);
			// 
			// menuItem8
			// 
			this.menuItem8.Index = 1;
			this.menuItem8.Text = "-";
			// 
			// exitFileItem
			// 
			this.exitFileItem.Index = 2;
			this.exitFileItem.Text = "E&xit";
			this.exitFileItem.Click += new System.EventHandler(this.exitFileItem_Click);
			// 
			// viewItem
			// 
			this.viewItem.Index = 1;
			this.viewItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.camerasBarViewItem,
																					 this.menuItem2,
																					 this.fitToScreenViewItem,
																					 this.fullScreenViewItem});
			this.viewItem.Text = "&View";
			// 
			// camerasBarViewItem
			// 
			this.camerasBarViewItem.Index = 0;
			this.camerasBarViewItem.Text = "&Cameras Bar";
			this.camerasBarViewItem.Click += new System.EventHandler(this.camerasBarViewItem_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 1;
			this.menuItem2.Text = "-";
			// 
			// fitToScreenViewItem
			// 
			this.fitToScreenViewItem.Index = 2;
			this.fitToScreenViewItem.Shortcut = System.Windows.Forms.Shortcut.F10;
			this.fitToScreenViewItem.Text = "Fit to screen";
			this.fitToScreenViewItem.Click += new System.EventHandler(this.fitToScreenViewItem_Click);
			// 
			// fullScreenViewItem
			// 
			this.fullScreenViewItem.Index = 3;
			this.fullScreenViewItem.Shortcut = System.Windows.Forms.Shortcut.F11;
			this.fullScreenViewItem.Text = "Full screen";
			this.fullScreenViewItem.Click += new System.EventHandler(this.fullScreenViewItem_Click);
			// 
			// camerasItem
			// 
			this.camerasItem.Index = 2;
			this.camerasItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						this.addGroupCamerasItem,
																						this.editGroupCamerasItem,
																						this.deleteGroupCamerasItem,
																						this.menuItem1,
																						this.addCameraCamerasItem,
																						this.editCameraCamerasItem,
																						this.deleteCameraCamerasItem,
																						this.menuItem3,
																						this.openCameraCamerasItem});
			this.camerasItem.Text = "&Cameras";
			this.camerasItem.Popup += new System.EventHandler(this.camerasItem_Popup);
			// 
			// addGroupCamerasItem
			// 
			this.addGroupCamerasItem.Index = 0;
			this.addGroupCamerasItem.Text = "Add group";
			this.addGroupCamerasItem.Click += new System.EventHandler(this.addGroupCamerasItem_Click);
			// 
			// editGroupCamerasItem
			// 
			this.editGroupCamerasItem.Index = 1;
			this.editGroupCamerasItem.Text = "Edit group";
			this.editGroupCamerasItem.Click += new System.EventHandler(this.editGroupCamerasItem_Click);
			// 
			// deleteGroupCamerasItem
			// 
			this.deleteGroupCamerasItem.Index = 2;
			this.deleteGroupCamerasItem.Text = "Delete group";
			this.deleteGroupCamerasItem.Click += new System.EventHandler(this.deleteGroupCamerasItem_Click);
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 3;
			this.menuItem1.Text = "-";
			// 
			// addCameraCamerasItem
			// 
			this.addCameraCamerasItem.Index = 4;
			this.addCameraCamerasItem.Text = "&Add camera";
			this.addCameraCamerasItem.Click += new System.EventHandler(this.addCameraCamerasItem_Click);
			// 
			// editCameraCamerasItem
			// 
			this.editCameraCamerasItem.Index = 5;
			this.editCameraCamerasItem.Text = "&Edit camera";
			this.editCameraCamerasItem.Click += new System.EventHandler(this.editCameraCamerasItem_Click);
			// 
			// deleteCameraCamerasItem
			// 
			this.deleteCameraCamerasItem.Index = 6;
			this.deleteCameraCamerasItem.Text = "&Delete camera";
			this.deleteCameraCamerasItem.Click += new System.EventHandler(this.deleteCameraCamerasItem_Click);
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 7;
			this.menuItem3.Text = "-";
			// 
			// openCameraCamerasItem
			// 
			this.openCameraCamerasItem.Index = 8;
			this.openCameraCamerasItem.Text = "&Open camera";
			this.openCameraCamerasItem.Click += new System.EventHandler(this.openCameraCamerasItem_Click);
			// 
			// viewsItem
			// 
			this.viewsItem.Index = 3;
			this.viewsItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.addGroupViewsItem,
																					  this.editGroupViewsItem,
																					  this.deleteGroupViewsItem,
																					  this.menuItem5,
																					  this.addViewViewsItem,
																					  this.editViewViewsItem,
																					  this.deleteViewViewsItem,
																					  this.menuItem6,
																					  this.openViewViewsItem});
			this.viewsItem.Text = "&Views";
			this.viewsItem.Popup += new System.EventHandler(this.viewsItem_Popup);
			// 
			// addGroupViewsItem
			// 
			this.addGroupViewsItem.Index = 0;
			this.addGroupViewsItem.Text = "Add group";
			this.addGroupViewsItem.Click += new System.EventHandler(this.addGroupViewsItem_Click);
			// 
			// editGroupViewsItem
			// 
			this.editGroupViewsItem.Index = 1;
			this.editGroupViewsItem.Text = "Edit group";
			this.editGroupViewsItem.Click += new System.EventHandler(this.editGroupViewsItem_Click);
			// 
			// deleteGroupViewsItem
			// 
			this.deleteGroupViewsItem.Index = 2;
			this.deleteGroupViewsItem.Text = "Delete group";
			this.deleteGroupViewsItem.Click += new System.EventHandler(this.deleteGroupViewsItem_Click);
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 3;
			this.menuItem5.Text = "-";
			// 
			// addViewViewsItem
			// 
			this.addViewViewsItem.Index = 4;
			this.addViewViewsItem.Text = "&Add view";
			this.addViewViewsItem.Click += new System.EventHandler(this.addViewViewsItem_Click);
			// 
			// editViewViewsItem
			// 
			this.editViewViewsItem.Index = 5;
			this.editViewViewsItem.Text = "&Edit view";
			this.editViewViewsItem.Click += new System.EventHandler(this.editViewViewsItem_Click);
			// 
			// deleteViewViewsItem
			// 
			this.deleteViewViewsItem.Index = 6;
			this.deleteViewViewsItem.Text = "&Delete view";
			this.deleteViewViewsItem.Click += new System.EventHandler(this.deleteViewViewsItem_Click);
			// 
			// menuItem6
			// 
			this.menuItem6.Index = 7;
			this.menuItem6.Text = "-";
			// 
			// openViewViewsItem
			// 
			this.openViewViewsItem.Index = 8;
			this.openViewViewsItem.Text = "&Open view";
			this.openViewViewsItem.Click += new System.EventHandler(this.openViewViewsItem_Click);
			// 
			// helpItem
			// 
			this.helpItem.Index = 4;
			this.helpItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.aboutHelpItem});
			this.helpItem.Text = "&Help";
			// 
			// aboutHelpItem
			// 
			this.aboutHelpItem.Index = 0;
			this.aboutHelpItem.Text = "&About";
			this.aboutHelpItem.Click += new System.EventHandler(this.aboutHelpItem_Click);
			// 
			// statusBar
			// 
			this.statusBar.Location = new System.Drawing.Point(0, 351);
			this.statusBar.Name = "statusBar";
			this.statusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
																						 this.infoPanel,
																						 this.bpsPanel,
																						 this.fpsPanel});
			this.statusBar.ShowPanels = true;
			this.statusBar.Size = new System.Drawing.Size(560, 22);
			this.statusBar.TabIndex = 0;
			// 
			// infoPanel
			// 
			this.infoPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			this.infoPanel.Width = 404;
			// 
			// bpsPanel
			// 
			this.bpsPanel.Width = 70;
			// 
			// fpsPanel
			// 
			this.fpsPanel.Width = 70;
			// 
			// toolBarImageList
			// 
			this.toolBarImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
			this.toolBarImageList.ImageSize = new System.Drawing.Size(24, 24);
			this.toolBarImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("toolBarImageList.ImageStream")));
			this.toolBarImageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// toolBar
			// 
			this.toolBar.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
			this.toolBar.AutoSize = false;
			this.toolBar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																					   this.closeViewButton,
																					   this.sep1,
																					   this.camerasBarButton,
																					   this.sep2,
																					   this.fitToScreenButton,
																					   this.fullScreenButton,
																					   this.sep3,
																					   this.aboutButton});
			this.toolBar.DropDownArrows = true;
			this.toolBar.ImageList = this.toolBarImageList;
			this.toolBar.Name = "toolBar";
			this.toolBar.ShowToolTips = true;
			this.toolBar.Size = new System.Drawing.Size(560, 33);
			this.toolBar.TabIndex = 1;
			this.toolBar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar_ButtonClick);
			// 
			// closeViewButton
			// 
			this.closeViewButton.ImageIndex = 4;
			this.closeViewButton.ToolTipText = "Close current view";
			// 
			// sep1
			// 
			this.sep1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// camerasBarButton
			// 
			this.camerasBarButton.ImageIndex = 0;
			this.camerasBarButton.ToolTipText = "Show/hide cameras tree";
			// 
			// sep2
			// 
			this.sep2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// fitToScreenButton
			// 
			this.fitToScreenButton.ImageIndex = 1;
			this.fitToScreenButton.ToolTipText = "Fit to screen";
			// 
			// fullScreenButton
			// 
			this.fullScreenButton.ImageIndex = 2;
			this.fullScreenButton.ToolTipText = "Full screen";
			// 
			// sep3
			// 
			this.sep3.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// aboutButton
			// 
			this.aboutButton.ImageIndex = 3;
			this.aboutButton.ToolTipText = "About";
			// 
			// mainPanel
			// 
			this.mainPanel.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.multiplexer,
																					this.splitter,
																					this.camerasTree});
			this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainPanel.Location = new System.Drawing.Point(0, 33);
			this.mainPanel.Name = "mainPanel";
			this.mainPanel.Size = new System.Drawing.Size(560, 318);
			this.mainPanel.TabIndex = 2;
			// 
			// multiplexer
			// 
			this.multiplexer.BackColor = System.Drawing.SystemColors.AppWorkspace;
			this.multiplexer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.multiplexer.CamerasContextMenu = this.cameraContextMenu;
			this.multiplexer.ContextMenu = this.viewContextMenu;
			this.multiplexer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.multiplexer.Location = new System.Drawing.Point(179, 0);
			this.multiplexer.Name = "multiplexer";
			this.multiplexer.Size = new System.Drawing.Size(381, 318);
			this.multiplexer.TabIndex = 2;
			// 
			// cameraContextMenu
			// 
			this.cameraContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																							  this.closeCameraItem,
																							  this.menuItem4,
																							  this.fitToScreenCameraItem,
																							  this.fullScreenCameraItem,
																							  this.menuItem7,
																							  this.cameraInfoItem});
			this.cameraContextMenu.Popup += new System.EventHandler(this.cameraContextMenu_Popup);
			// 
			// closeCameraItem
			// 
			this.closeCameraItem.Index = 0;
			this.closeCameraItem.Text = "Close view";
			this.closeCameraItem.Click += new System.EventHandler(this.closeCameraItem_Click);
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 1;
			this.menuItem4.Text = "-";
			// 
			// fitToScreenCameraItem
			// 
			this.fitToScreenCameraItem.Index = 2;
			this.fitToScreenCameraItem.Shortcut = System.Windows.Forms.Shortcut.F10;
			this.fitToScreenCameraItem.Text = "Fit to screen";
			this.fitToScreenCameraItem.Click += new System.EventHandler(this.fitToScreenViewItem_Click);
			// 
			// fullScreenCameraItem
			// 
			this.fullScreenCameraItem.Index = 3;
			this.fullScreenCameraItem.Shortcut = System.Windows.Forms.Shortcut.F11;
			this.fullScreenCameraItem.Text = "Full screen";
			this.fullScreenCameraItem.Click += new System.EventHandler(this.fullScreenViewItem_Click);
			// 
			// viewContextMenu
			// 
			this.viewContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																							this.fitToSreenItem,
																							this.fullScreenItem});
			// 
			// fitToSreenItem
			// 
			this.fitToSreenItem.Index = 0;
			this.fitToSreenItem.Shortcut = System.Windows.Forms.Shortcut.F10;
			this.fitToSreenItem.Text = "Fit to screen";
			this.fitToSreenItem.Click += new System.EventHandler(this.fitToScreenViewItem_Click);
			// 
			// fullScreenItem
			// 
			this.fullScreenItem.Index = 1;
			this.fullScreenItem.Shortcut = System.Windows.Forms.Shortcut.F11;
			this.fullScreenItem.Text = "Full screen";
			this.fullScreenItem.Click += new System.EventHandler(this.fullScreenViewItem_Click);
			// 
			// splitter
			// 
			this.splitter.Location = new System.Drawing.Point(176, 0);
			this.splitter.Name = "splitter";
			this.splitter.Size = new System.Drawing.Size(3, 318);
			this.splitter.TabIndex = 1;
			this.splitter.TabStop = false;
			// 
			// camerasTree
			// 
			this.camerasTree.CameraImage = 2;
			this.camerasTree.CameraSelectedImage = 2;
			this.camerasTree.CamerasFolderSelectedImage = 1;
			this.camerasTree.ContextMenu = this.camerasContextMenu;
			this.camerasTree.Dock = System.Windows.Forms.DockStyle.Left;
			this.camerasTree.HideSelection = false;
			this.camerasTree.ImageList = this.iconsList;
			this.camerasTree.Name = "camerasTree";
			this.camerasTree.Size = new System.Drawing.Size(176, 318);
			this.camerasTree.Sorted = true;
			this.camerasTree.TabIndex = 0;
			this.camerasTree.ViewImage = 5;
			this.camerasTree.ViewSelectedImage = 5;
			this.camerasTree.ViewsFolderImage = 3;
			this.camerasTree.ViewsFolderSelectedImage = 4;
			this.camerasTree.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.camerasTree_KeyPress);
			this.camerasTree.DoubleClick += new System.EventHandler(this.camerasTree_DoubleClick);
			// 
			// camerasContextMenu
			// 
			this.camerasContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																							   this.addGroupItem,
																							   this.editGroupItem,
																							   this.deleteGroupItem,
																							   this.sep1Item,
																							   this.addCameraItem,
																							   this.editCameraItem,
																							   this.deleteCameraItem,
																							   this.sep2Item,
																							   this.openCameraItem,
																							   this.sep3Item,
																							   this.addViewItem,
																							   this.editViewItem,
																							   this.deleteViewItem,
																							   this.sep4Item,
																							   this.openViewItem});
			this.camerasContextMenu.Popup += new System.EventHandler(this.camerasContextMenu_Popup);
			// 
			// addGroupItem
			// 
			this.addGroupItem.Index = 0;
			this.addGroupItem.Text = "Add group";
			this.addGroupItem.Click += new System.EventHandler(this.addGroupItem_Click);
			// 
			// editGroupItem
			// 
			this.editGroupItem.Index = 1;
			this.editGroupItem.Text = "Edit group";
			this.editGroupItem.Click += new System.EventHandler(this.editGroupItem_Click);
			// 
			// deleteGroupItem
			// 
			this.deleteGroupItem.Index = 2;
			this.deleteGroupItem.Text = "Delete group";
			this.deleteGroupItem.Click += new System.EventHandler(this.deleteGroupItem_Click);
			// 
			// sep1Item
			// 
			this.sep1Item.Index = 3;
			this.sep1Item.Text = "-";
			// 
			// addCameraItem
			// 
			this.addCameraItem.Index = 4;
			this.addCameraItem.Text = "Add camera";
			this.addCameraItem.Click += new System.EventHandler(this.addCameraItem_Click);
			// 
			// editCameraItem
			// 
			this.editCameraItem.Index = 5;
			this.editCameraItem.Text = "Edit camera";
			this.editCameraItem.Click += new System.EventHandler(this.editCameraItem_Click);
			// 
			// deleteCameraItem
			// 
			this.deleteCameraItem.Index = 6;
			this.deleteCameraItem.Text = "Delete camera";
			this.deleteCameraItem.Click += new System.EventHandler(this.deleteCameraItem_Click);
			// 
			// sep2Item
			// 
			this.sep2Item.Index = 7;
			this.sep2Item.Text = "-";
			// 
			// openCameraItem
			// 
			this.openCameraItem.Index = 8;
			this.openCameraItem.Text = "Open camera";
			this.openCameraItem.Click += new System.EventHandler(this.openCameraItem_Click);
			// 
			// sep3Item
			// 
			this.sep3Item.Index = 9;
			this.sep3Item.Text = "-";
			// 
			// addViewItem
			// 
			this.addViewItem.Index = 10;
			this.addViewItem.Text = "Add view";
			this.addViewItem.Click += new System.EventHandler(this.addViewItem_Click);
			// 
			// editViewItem
			// 
			this.editViewItem.Index = 11;
			this.editViewItem.Text = "Edit view";
			this.editViewItem.Click += new System.EventHandler(this.editViewItem_Click);
			// 
			// deleteViewItem
			// 
			this.deleteViewItem.Index = 12;
			this.deleteViewItem.Text = "Delete view";
			this.deleteViewItem.Click += new System.EventHandler(this.deleteViewItem_Click);
			// 
			// sep4Item
			// 
			this.sep4Item.Index = 13;
			this.sep4Item.Text = "-";
			// 
			// openViewItem
			// 
			this.openViewItem.Index = 14;
			this.openViewItem.Text = "Open view";
			this.openViewItem.Click += new System.EventHandler(this.openViewItem_Click);
			// 
			// iconsList
			// 
			this.iconsList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			this.iconsList.ImageSize = new System.Drawing.Size(16, 16);
			this.iconsList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("iconsList.ImageStream")));
			this.iconsList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// timer
			// 
			this.timer.Interval = 1000;
			this.timer.SynchronizingObject = this;
			this.timer.Elapsed += new System.Timers.ElapsedEventHandler(this.timer_Elapsed);
			// 
			// menuItem7
			// 
			this.menuItem7.Index = 4;
			this.menuItem7.Text = "-";
			// 
			// cameraInfoItem
			// 
			this.cameraInfoItem.Index = 5;
			this.cameraInfoItem.Text = "Camera info";
			this.cameraInfoItem.Click += new System.EventHandler(this.cameraInfoItem_Click);
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(560, 373);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.mainPanel,
																		  this.toolBar,
																		  this.statusBar});
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximumSize = new System.Drawing.Size(2000, 2000);
			this.Menu = this.mainMenu;
			this.MinimumSize = new System.Drawing.Size(320, 240);
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Camera Vision";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.MainForm_Closing);
			this.Load += new System.EventHandler(this.MainForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.infoPanel)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.bpsPanel)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.fpsPanel)).EndInit();
			this.mainPanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.timer)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			try
			{
				Application.Run(new MainForm());
			}
			catch (System.IO.FileNotFoundException)
			{
				MessageBox.Show(null, "Some components of the application are missed.\nPlease, reinstall it.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}
		}

		// On "File->Exit" - exit application
		private void exitFileItem_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		// On "File->Clsoe" - close camera or view
		private void closeFileItem_Click(object sender, System.EventArgs e)
		{
			CloseView();
		}

		// Loading the form
		private void MainForm_Load(object sender, System.EventArgs e)
		{
			// load configuration
			if (config.LoadSettings())
			{
				// set window location and size
				this.Location = config.mainWindowLocation;
				this.Size = config.mainWindowSize;

				// show cameras bar
				ShowCamarasBar(config.showCameraBar);
				this.camerasTree.Width = config.cameraBarWidth;

				FitToScreen(config.fitToScreen);
				if (config.fullScreen)
					FullScreen(config.fullScreen);
			}

			// load providers
			config.providers.Load(Path.GetDirectoryName(Application.ExecutablePath));

			// load cameras tree
			config.LoadCameras();
			// load view tree
			config.LoadViews();

			// build cameras & views tree
			camerasTree.BuildCamerasTree(config.camerasGroups, config.cameras);
			camerasTree.BuildViewsTree(config.viewsGroups, config.views);

			// start finalization pool
			finalizationPool.Start();
		}

		// Closing the form
		private void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			// save configuration
			config.cameraBarWidth = this.camerasTree.Width;

			if (!config.fullScreen)
			{
				config.mainWindowLocation = this.Location;
				config.mainWindowSize = this.Size;
			}
			config.SaveSettings();

			multiplexer.CamerasVisible = false;
			multiplexer.CloseAll();

			// close opened view
			CloseView();

			// sleep for a while - give a chance for cameras to stop
			Thread.Sleep(500);

			// stop finalization pool
			finalizationPool.Stop();
		}

		// Show/Hide cameras bar
		private void ShowCamarasBar(bool show)
		{
			config.showCameraBar = show;

			this.camerasBarButton.Pushed = show;
			this.camerasBarViewItem.Checked = show;

			this.camerasTree.Visible = show;
		}

		// Fit to screen
		private void FitToScreen(bool fit)
		{
			config.fitToScreen = fit;

			this.fitToScreenButton.Pushed = fit;
			this.fitToScreenViewItem.Checked = fit;
			this.fitToScreenCameraItem.Checked = fit;
			this.fitToSreenItem.Checked = fit;

			multiplexer.FitToWindow = fit;
/*
			cameraWindow.AutoSize = !fit;

			// update size and position of view window
			if (fit == false)
			{
				cameraWindow.Anchor = AnchorStyles.None;

				// auto update
				cameraWindow.UpdatePosition();
			}
			else
			{
				Rectangle rc = viewPanel.ClientRectangle;

				// manul position update
				cameraWindow.SuspendLayout();
				cameraWindow.Location = new Point(5, 5);
				cameraWindow.Size = new Size(rc.Width - 10, rc.Height - 10);
				cameraWindow.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
				cameraWindow.ResumeLayout();
			}*/
		}

		// Full screen
		private void FullScreen(bool full)
		{
			config.fullScreen = full;

			this.fullScreenButton.Pushed = full;
			this.fullScreenViewItem.Checked = full;
			this.fullScreenCameraItem.Checked = full;
			this.fullScreenItem.Checked = full;

			if (full)
			{
				// save window position
				config.mainWindowLocation = this.Location;
				config.mainWindowSize = this.Size;

				this.FormBorderStyle = FormBorderStyle.None;
				this.Menu = null;

				int	cx = Win32.GetSystemMetrics(Win32.SystemMetrics.CXSCREEN);
				int	cy = Win32.GetSystemMetrics(Win32.SystemMetrics.CYSCREEN);

				this.Location = new Point(-4, -0);
				this.Size = new Size(cx + 5, cy + 1);
			}
			else
			{
				this.FormBorderStyle = FormBorderStyle.Sizable;
				this.Menu = this.mainMenu;

				// restore window position
				this.Location = config.mainWindowLocation;
				this.Size = config.mainWindowSize;
			}

			//
			bool visible = !full;

			// set/reset top most window
			this.TopMost = full;

			// hide/restore cameras bar
			this.camerasTree.Visible = (full) ? visible : config.showCameraBar;
			// hide/show status bar
			this.statusBar.Visible = visible;
			// hide/show tool bar
			this.toolBar.Visible = visible;
		}

		// Display about dialog
		private void About()
		{
			AboutForm form = new AboutForm();

			form.ShowDialog();
		}

		// On "Show cameras bar" toolbar button click
		private void toolBar_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			switch (e.Button.ImageIndex)
			{
				case 0:		// show/hide cameras tree
					ShowCamarasBar(!config.showCameraBar);
					break;
				case 1:		// fit to screen
					FitToScreen(!config.fitToScreen);
					break;
				case 2:		// fool screen
					FullScreen(!config.fullScreen);
					break;
				case 3:		// about
					About();
					break;
				case 4:		// close view or camera
					CloseView();
					break;
			}
		}

		// On "Help->About" menu item click
		private void aboutHelpItem_Click(object sender, System.EventArgs e)
		{
			About();		
		}

		// On "View->Show cameras bar" menu item click
		private void camerasBarViewItem_Click(object sender, System.EventArgs e)
		{
			ShowCamarasBar(!config.showCameraBar);
		}

		// On "View->Fit to screen" item click
		private void fitToScreenViewItem_Click(object sender, System.EventArgs e)
		{
			FitToScreen(!config.fitToScreen);
		}

		// On "View->Full screen" item click
		private void fullScreenViewItem_Click(object sender, System.EventArgs e)
		{
			FullScreen(!config.fullScreen);
		}

		// On "Cameras" popup
		private void camerasItem_Popup(object sender, System.EventArgs e)
		{
			NodeType nodeType = camerasTree.GetNodeType(camerasTree.SelectedNode);

			// edit & delete cameras group items
			editGroupCamerasItem.Enabled = deleteGroupCamerasItem.Enabled = (nodeType == NodeType.CamerasGroup);

			// add camera item
			addCameraCamerasItem.Enabled = ((nodeType == NodeType.CamerasGroup) || (nodeType == NodeType.CamerasRootGroup) || (nodeType == NodeType.Camera));
			// edit & delete camera item
			openCameraCamerasItem.Enabled = editCameraCamerasItem.Enabled = deleteCameraCamerasItem.Enabled = (nodeType == NodeType.Camera);
		}

		// On cameras tree context menu popup
		private void camerasContextMenu_Popup(object sender, System.EventArgs e)
		{
			NodeType nodeType = camerasTree.GetNodeType(camerasTree.LastClickNode);

			// add group item
			addGroupItem.Enabled = ((nodeType == NodeType.CamerasGroup) || (nodeType == NodeType.CamerasRootGroup) ||
								(nodeType == NodeType.ViewsGroup) || (nodeType == NodeType.ViewsRootGroup));
			// edit & delete group items
			editGroupItem.Enabled = deleteGroupItem.Enabled = ((nodeType == NodeType.CamerasGroup) || (nodeType == NodeType.ViewsGroup));

			// camera items
			if ((nodeType == NodeType.CamerasGroup) || (nodeType == NodeType.CamerasRootGroup) || (nodeType == NodeType.Camera))
			{
				sep1Item.Visible = sep2Item.Visible = openCameraItem.Visible =
					addCameraItem.Visible = editCameraItem.Visible = deleteCameraItem.Visible = true;

				openCameraItem.Enabled = editCameraItem.Enabled = deleteCameraItem.Enabled = (nodeType == NodeType.Camera);
			}
			else
			{
				sep1Item.Visible = sep2Item.Visible = openCameraItem.Visible =
					addCameraItem.Visible = editCameraItem.Visible = deleteCameraItem.Visible = false;
			}

			// view items
			if ((nodeType == NodeType.ViewsGroup) || (nodeType == NodeType.ViewsRootGroup) || (nodeType == NodeType.View))
			{
				sep3Item.Visible = sep4Item.Visible = openViewItem.Visible =
					addViewItem.Visible = editViewItem.Visible = deleteViewItem.Visible = true;

				openViewItem.Enabled = editViewItem.Enabled = deleteViewItem.Enabled = (nodeType == NodeType.View);
			}
			else
			{
				sep3Item.Visible = sep4Item.Visible = openViewItem.Visible =
					addViewItem.Visible = editViewItem.Visible = deleteViewItem.Visible = false;
			}
		}

		// Ñheck if the cameras group is already exist
		private bool CheckCamerasGroup(Group group)
		{
			return config.CheckCamerasGroup(group);
		}

		// Ñheck if the camera is already exist
		private bool CheckCamera(Camera camera)
		{
			return config.CheckCamera(camera);
		}

		// Add new cameras group
		private void AddCamerasGroup(TreeNode parentNode)
		{
			GroupForm	form = new GroupForm();
			NodeType	parentType = camerasTree.GetNodeType(parentNode);

			// check parent node type
			if ((parentType != NodeType.CamerasGroup) && (parentType != NodeType.CamerasRootGroup))
				parentNode = camerasTree.CamerasRootNode;

			// set dialog title
			form.Text = "Add cameras group";
			// set callback for group name checking
			form.CheckGroupFunction = new CheckGroupHandler(CheckCamerasGroup);

			// get full name of selected cameras group
			string parentName = camerasTree.GetGroupFullName(parentNode);

			if (parentName != null)
			{
				// get parent group
				form.Group.Parent = config.GetCamerasGroupByName(parentName);
			}

			// show dialog
			if (form.ShowDialog() == DialogResult.OK)
			{
				Group	group = form.Group;

				// add to groups collection
				config.AddCamerasGroup(group);

				// add to tree
				camerasTree.SelectedNode = camerasTree.AddCamerasGroup(group, parentNode);
			}
		}

		// Edit cameras group
		private void EditCamerasGroup(TreeNode node)
		{
			GroupForm	form = new GroupForm();

			// set dialog title
			form.Text = "Edit cameras group";
			// set callback for group name checking
			form.CheckGroupFunction = new CheckGroupHandler(CheckCamerasGroup);

			// get group
			form.Group = config.GetCamerasGroupByName(camerasTree.GetGroupFullName(node));

			// show dialog
			if (form.ShowDialog() == DialogResult.OK)
			{
				// save cameras
				config.SaveCameras();

				// update tree
				node.Text = form.Group.Name;
			}
		}

		// Delete cameras group
		private void DeleteCamerasGroup(TreeNode node)
		{
			string	fullName = camerasTree.GetGroupFullName(node);

			// ask user
			if (MessageBox.Show(this, "Are you sure you want to delete the group \"" + fullName + "\" ?", "Question",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Question,
				MessageBoxDefaultButton.Button2) == DialogResult.Yes)
			{
				// get group
				Group group = config.GetCamerasGroupByName(fullName);

				// delete the group
				if (config.DeleteCamerasGroup(group))
				{
					// delete it from tree
					camerasTree.Nodes.Remove(node);
				}
				else
				{
					// can not delete non empty groups
					MessageBox.Show(this, "Can not delete the group, it is not empty.", "Note",
						MessageBoxButtons.OK,
						MessageBoxIcon.Exclamation);
				}
			}
		}

		// On "Cameras->Add group"
		private void addGroupCamerasItem_Click(object sender, System.EventArgs e)
		{
			AddCamerasGroup(camerasTree.SelectedNode);
		}

		// On "Cameras->Edit group"
		private void editGroupCamerasItem_Click(object sender, System.EventArgs e)
		{
			EditCamerasGroup(camerasTree.SelectedNode);
		}

		// On "Cameras->Delete"
		private void deleteGroupCamerasItem_Click(object sender, System.EventArgs e)
		{
			DeleteCamerasGroup(camerasTree.SelectedNode);
		}

		// On cameras tree context menu "Add Group"
		private void addGroupItem_Click(object sender, System.EventArgs e)
		{
			camerasTree.SelectedNode = camerasTree.LastClickNode;
			
			NodeType type = camerasTree.GetNodeType(camerasTree.LastClickNode);

			if ((type == NodeType.CamerasGroup) || (type == NodeType.CamerasRootGroup))
				AddCamerasGroup(camerasTree.LastClickNode);
			else if ((type == NodeType.ViewsGroup) || (type == NodeType.ViewsRootGroup))
				AddViewsGroup(camerasTree.LastClickNode);
		}

		// On cameras tree context menu "Edit Group"
		private void editGroupItem_Click(object sender, System.EventArgs e)
		{
			camerasTree.SelectedNode = camerasTree.LastClickNode;

			NodeType type = camerasTree.GetNodeType(camerasTree.LastClickNode);

			if (type == NodeType.CamerasGroup)
				EditCamerasGroup(camerasTree.LastClickNode);
			else if (type == NodeType.ViewsGroup)
				EditViewsGroup(camerasTree.LastClickNode);
		}

		// On cameras tree context menu "Delete Group"
		private void deleteGroupItem_Click(object sender, System.EventArgs e)
		{
			camerasTree.SelectedNode = camerasTree.LastClickNode;

			NodeType type = camerasTree.GetNodeType(camerasTree.LastClickNode);

			if (type == NodeType.CamerasGroup)
				DeleteCamerasGroup(camerasTree.LastClickNode);
			else if (type == NodeType.ViewsGroup)
				DeleteViewsGroup(camerasTree.LastClickNode);
		}

		//  Add camera
		private void AddCamera(TreeNode parentNode)
		{
			CameraForm	form = new CameraForm();

			// set providers
			form.VideoProviders = config.providers;
			// set callback for camera name checking
			form.CheckCameraFunction = new CheckCameraHandler(CheckCamera);

			// check for group node
			if (camerasTree.GetNodeType(parentNode) == NodeType.Camera)
				parentNode = parentNode.Parent;

			// get full name of selected cameras group
			string parentName = camerasTree.GetGroupFullName(parentNode);

			if (parentName != null)
			{
				// get parent group
				form.Camera.Parent = config.GetCamerasGroupByName(parentName);
			}

			// show dialogs
			if (form.ShowDialog() == DialogResult.OK)
			{
				Camera	camera = form.Camera;

				// add to cameras collection
				config.AddCamera(camera);

				// add to tree
				camerasTree.SelectedNode = camerasTree.AddCamera(camera, parentNode);
			}
		}

		// Edit camera
		private void EditCamera(TreeNode node)
		{
			CameraPropertiesForm form = new CameraPropertiesForm();
			string	fullName = camerasTree.GetCameraFullName(node);

			// set providers
			form.VideoProviders = config.providers;
			// set callback for camera name checking
			form.CheckCameraFunction = new CheckCameraHandler(CheckCamera);
			// get camera
			form.Camera = cameraToEdit = config.GetCameraByName(fullName);
			// catch Apply event
			form.Apply += new EventHandler(editCamera_Apply);

			nodeToEdit = node;

			if (form.ShowDialog() == DialogResult.OK)
			{
				config.SaveCameras();

				// modify tree
				node.Text = cameraToEdit.Name;
			}

			nodeToEdit = null;
		}

		// On "Apply" button in camera properties window
		private void editCamera_Apply(object sender, System.EventArgs e)
		{
			if (((PagedWizard) sender).SelectedPageIndex == 0)
			{
				// modify tree
				nodeToEdit.Text = cameraToEdit.Name;
			}
			config.SaveCameras();
		}

		//  Delete camera
		private void DeleteCamera(TreeNode node)
		{
			string	fullName = camerasTree.GetCameraFullName(node);

			// ask user
			if (MessageBox.Show(this, "Are you sure you want to delete the camera \"" + fullName + "\" ?", "Question",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Question,
				MessageBoxDefaultButton.Button2) == DialogResult.Yes)
			{
				// get camera
				Camera camera = config.GetCameraByName(fullName);

				// delete the camera
				if (config.DeleteCamera(camera))
				{
					// delete it from tree
					camerasTree.Nodes.Remove(node);
				}
				else
				{
					// failed deleting camera
					MessageBox.Show(this, "Failed deleting the camer \"" + fullName + "\"", "Error",
						MessageBoxButtons.OK,
						MessageBoxIcon.Stop);
				}
			}
		}

		// On "Cameras->Add" - add camera
		private void addCameraCamerasItem_Click(object sender, System.EventArgs e)
		{
			AddCamera(camerasTree.SelectedNode);
		}

		// On "Cameras->Edit" - edit camera
		private void editCameraCamerasItem_Click(object sender, System.EventArgs e)
		{
			EditCamera(camerasTree.SelectedNode);
		}

		// On "Cameras->Delete" - delete camera
		private void deleteCameraCamerasItem_Click(object sender, System.EventArgs e)
		{
			DeleteCamera(camerasTree.SelectedNode);
		}

		// On cameras tree context menu "Add Camera"
		private void addCameraItem_Click(object sender, System.EventArgs e)
		{
			AddCamera(camerasTree.LastClickNode);
		}

		// On cameras tree context menu "Edit Camera"
		private void editCameraItem_Click(object sender, System.EventArgs e)
		{
			camerasTree.SelectedNode = camerasTree.LastClickNode;
			EditCamera(camerasTree.LastClickNode);
		}

		// On cameras tree context menu "Delete Camera"
		private void deleteCameraItem_Click(object sender, System.EventArgs e)
		{
			camerasTree.SelectedNode = camerasTree.LastClickNode;
			DeleteCamera(camerasTree.LastClickNode);
		}

		// Open camera
		private void OpenCamera(TreeNode node)
		{
			string	fullName = camerasTree.GetCameraFullName(node);

			// get camera
			Camera camera = config.GetCameraByName(fullName);

			// check if it is already running
			if ((viewOpened == false) && (openedID == camera.ID))
				return;

			// close previous view
			CloseView();

			// abort the camera, if it is in finalization pool
			finalizationPool.Remove(camera);

			// add camera to running pool
			if (runningPool.Add(camera))
			{
				// attach it to view
				multiplexer.SetCamera(0, 0, camera);
				multiplexer.Rows = 1;
				multiplexer.Cols = 1;
				multiplexer.SingleCameraMode = true;
				multiplexer.CamerasVisible = true;

				// set title
				this.Text = title + " - " + fullName;

				// reset statistics indexes
				statIndex = 0;
				statReady = 0;

				//
				openedID = camera.ID;
				viewOpened = false;

				// start timer
				timer.Start();
			}
		}

		// On "Cameras->Open camera" - open camera
		private void openCameraCamerasItem_Click(object sender, System.EventArgs e)
		{
			OpenCamera(camerasTree.SelectedNode);
		}

		// On cameras tree context menu "Open Camera"
		private void openCameraItem_Click(object sender, System.EventArgs e)
		{
			camerasTree.SelectedNode = camerasTree.LastClickNode;
			OpenCamera(camerasTree.LastClickNode);
		}

		// Double click in cameras tree
		private void camerasTree_DoubleClick(object sender, System.EventArgs e)
		{
			NodeType type = camerasTree.GetNodeType(camerasTree.SelectedNode);

			switch (type)
			{
				case NodeType.Camera:	// open camera
					OpenCamera(camerasTree.SelectedNode);
					break;
				case NodeType.View:		// open view
					OpenView(camerasTree.SelectedNode);
					break;
			}
		}

		// Close camera
		private void CloseView()
		{
			if (runningPool.Count != 0)
			{
				// detach any cameras from view
				multiplexer.CloseAll();
				multiplexer.CamerasVisible = false;

				// stop timer
				timer.Stop();

				fpsPanel.Text = "";
				bpsPanel.Text = "";

				// move all cameras from running pool to finalization pool
				while (runningPool.Count != 0)
				{
					Camera camera = runningPool[0];

					// remove camera from running pool
					runningPool.Remove(camera);
					// add camera to finilization pool
					finalizationPool.Add(camera);
				}

				// set default title
				this.Text = title;

				openedID = 0;
			}
		}

		// On "Close" from camera context menu
		private void closeCameraItem_Click(object sender, System.EventArgs e)
		{
			CloseView();
		}

		// On camera context menu popup
		private void cameraContextMenu_Popup(object sender, System.EventArgs e)
		{
			closeCameraItem.Enabled = (runningPool.Count != 0);
		}

		// Key pressed in camera tree view
		private void camerasTree_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if (e.KeyChar == (char) 13)
			{
				NodeType type = camerasTree.GetNodeType(camerasTree.SelectedNode);

				switch (type)
				{
					case NodeType.Camera:	// open camera
						OpenCamera(camerasTree.SelectedNode);
						break;
					case NodeType.View:		// open view
						OpenView(camerasTree.SelectedNode);
						break;
				}
			}
		}
		
		// On "Views" menu item popup
		private void viewsItem_Popup(object sender, System.EventArgs e)
		{
			NodeType nodeType = camerasTree.GetNodeType(camerasTree.SelectedNode);

			// edit & delete view group items
			editGroupViewsItem.Enabled = deleteGroupViewsItem.Enabled = (nodeType == NodeType.ViewsGroup);

			// add view item
			addViewViewsItem.Enabled = ((nodeType == NodeType.ViewsGroup) || (nodeType == NodeType.ViewsRootGroup) || (nodeType == NodeType.View));
			// edit & delete view item
			editViewViewsItem.Enabled = deleteViewViewsItem.Enabled = (nodeType == NodeType.View);
		}

		// Ñheck if the views group is already exist
		private bool CheckViewsGroup(Group group)
		{
			return config.CheckViewsGroup(group);
		}

		// Ñheck if the view is already exist
		private bool CheckView(View view)
		{
			return config.CheckView(view);
		}

		// Add new views group
		private void AddViewsGroup(TreeNode parentNode)
		{
			GroupForm	form = new GroupForm();
			NodeType	parentType = camerasTree.GetNodeType(parentNode);
			
			// check parent node type
			if ((parentType != NodeType.ViewsGroup) && (parentType != NodeType.ViewsRootGroup))
				parentNode = camerasTree.ViewsRootNode;

			// set dialog title
			form.Text = "Add views group";
			// set callback for group name checking
			form.CheckGroupFunction = new CheckGroupHandler(CheckViewsGroup);

			// get full name of selected cameras group
			string parentName = camerasTree.GetGroupFullName(parentNode);

			if (parentName != null)
			{
				// get parent group
				form.Group.Parent = config.GetViewsGroupByName(parentName);
			}

			// show dialog
			if (form.ShowDialog() == DialogResult.OK)
			{
				Group	group = form.Group;

				// add to groups collection
				config.AddViewsGroup(group);

				// add to tree
				camerasTree.SelectedNode = camerasTree.AddViewsGroup(group, parentNode);
			}
		}

		// Edit views group
		private void EditViewsGroup(TreeNode node)
		{
			GroupForm	form = new GroupForm();

			// set dialog title
			form.Text = "Edit views group";
			// set callback for group name checking
			form.CheckGroupFunction = new CheckGroupHandler(CheckViewsGroup);

			// get group
			form.Group = config.GetViewsGroupByName(camerasTree.GetGroupFullName(node));

			// show dialog
			if (form.ShowDialog() == DialogResult.OK)
			{
				// save cameras
				config.SaveCameras();

				// update tree
				node.Text = form.Group.Name;
			}
		}

		// Delete views group
		private void DeleteViewsGroup(TreeNode node)
		{
			string	fullName = camerasTree.GetGroupFullName(node);

			// ask user
			if (MessageBox.Show(this, "Are you sure you want to delete the group \"" + fullName + "\" ?", "Question",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Question,
				MessageBoxDefaultButton.Button2) == DialogResult.Yes)
			{
				// get group
				Group group = config.GetViewsGroupByName(fullName);

				// delete the group
				if (config.DeleteViewsGroup(group))
				{
					// delete it from tree
					camerasTree.Nodes.Remove(node);
				}
				else
				{
					// can not delete non empty groups
					MessageBox.Show(this, "Can not delete the group, it is not empty.", "Note",
						MessageBoxButtons.OK,
						MessageBoxIcon.Exclamation);
				}
			}
		}

		// On "Views->Add Group" menu item click
		private void addGroupViewsItem_Click(object sender, System.EventArgs e)
		{
			AddViewsGroup(camerasTree.SelectedNode);
		}

		// On "Views->Edit Group" menu item click
		private void editGroupViewsItem_Click(object sender, System.EventArgs e)
		{
			EditViewsGroup(camerasTree.SelectedNode);
		}

		// On "Views->Delete Group" menu item click
		private void deleteGroupViewsItem_Click(object sender, System.EventArgs e)
		{
			DeleteViewsGroup(camerasTree.SelectedNode);
		}

		//  Add view
		private void AddView(TreeNode parentNode)
		{
			ViewForm	form = new ViewForm();

			// build cameras tree
			form.BuildCamerasTree(config.camerasGroups, config.cameras);
			// set callback for view name checking
			form.CheckViewFunction = new CheckViewHandler(CheckView);

			// check for group node
			if (camerasTree.GetNodeType(parentNode) == NodeType.View)
				parentNode = parentNode.Parent;

			// get full name of selected cameras group
			string parentName = camerasTree.GetGroupFullName(parentNode);

			if (parentName != null)
			{
				// get parent group
				form.View.Parent = config.GetViewsGroupByName(parentName);
			}

			// show dialogs
			if (form.ShowDialog() == DialogResult.OK)
			{
				View view = form.View;

				// add to views collection
				config.AddView(view);

				// add to tree
				camerasTree.SelectedNode = camerasTree.AddView(view, parentNode);
			}
		}

		//  Edit view
		private void EditView(TreeNode node)
		{
			ViewPropertiesForm form = new ViewPropertiesForm();
			string	fullName = camerasTree.GetViewFullName(node);

			// build cameras tree
			form.BuildCamerasTree(config.camerasGroups, config.cameras);
			// set callback for view name checking
			form.CheckViewFunction = new CheckViewHandler(CheckView);
			// get view
			form.View = viewToEdit = config.GetViewByName(fullName);
			// catch Apply event
			form.Apply += new EventHandler(editView_Apply);

			nodeToEdit = node;

			if (form.ShowDialog() == DialogResult.OK)
			{
				config.SaveViews();

				// modify tree
				node.Text = viewToEdit.Name;
			}

			nodeToEdit = null;
		}

		// On "Apply" button in view properties window
		private void editView_Apply(object sender, System.EventArgs e)
		{
			if (((PagedWizard) sender).SelectedPageIndex == 0)
			{
				// modify tree
				nodeToEdit.Text = viewToEdit.Name;
			}
			config.SaveViews();
		}

		//  Delete view
		private void DeleteView(TreeNode node)
		{
			string	fullName = camerasTree.GetViewFullName(node);

			// ask user
			if (MessageBox.Show(this, "Are you sure you want to delete the view \"" + fullName + "\" ?", "Question",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Question,
				MessageBoxDefaultButton.Button2) == DialogResult.Yes)
			{
				// get view
				View view = config.GetViewByName(fullName);

				// delete the view
				if (config.DeleteView(view))
				{
					// delete it from tree
					camerasTree.Nodes.Remove(node);
				}
				else
				{
					// failed deleting view
					MessageBox.Show(this, "Failed deleting the view \"" + fullName + "\"", "Error",
						MessageBoxButtons.OK,
						MessageBoxIcon.Stop);
				}
			}
		}

		// On "Views->Add view" menu item click
		private void addViewViewsItem_Click(object sender, System.EventArgs e)
		{
			AddView(camerasTree.SelectedNode);
		}

		// On "Views->Edit view" menu item click
		private void editViewViewsItem_Click(object sender, System.EventArgs e)
		{
			EditView(camerasTree.SelectedNode);
		}

		// On "Views->Delete view" menu item click
		private void deleteViewViewsItem_Click(object sender, System.EventArgs e)
		{
			DeleteView(camerasTree.SelectedNode);
		}

		// On cameras tree context menu "Add View"
		private void addViewItem_Click(object sender, System.EventArgs e)
		{
			AddView(camerasTree.LastClickNode);
		}

		// On cameras tree context menu "Edit View"
		private void editViewItem_Click(object sender, System.EventArgs e)
		{
			camerasTree.SelectedNode = camerasTree.LastClickNode;
			EditView(camerasTree.LastClickNode);
		}

		// On cameras tree context menu "Delete View"
		private void deleteViewItem_Click(object sender, System.EventArgs e)
		{
			camerasTree.SelectedNode = camerasTree.LastClickNode;
			DeleteView(camerasTree.LastClickNode);
		}

		// Open view
		private void OpenView(TreeNode node)
		{
			string	fullName = camerasTree.GetViewFullName(node);

			// get view
			View view = config.GetViewByName(fullName);

			// check if it is already running
			if ((viewOpened == true) && (openedID == view.ID))
				return;
			
			// close previous view
			CloseView();

			// run all cameras
			for (int i = 0; i < view.Rows; i++)
			{
				for (int j = 0; j < view.Cols; j++)
				{
					// get camera
					Camera camera = config.cameras.GetCamera(view.GetCamera(i, j));

					if (camera == null)
						continue;

					// abort it, if it is in finalization pool
					finalizationPool.Remove(camera);

					// add it to running pool
					if (runningPool.Add(camera))
					{
						multiplexer.SetCamera(i, j, camera);
					}
				}
			}

			multiplexer.Rows = view.Rows;
			multiplexer.Cols = view.Cols;
			multiplexer.SingleCameraMode = false;
			multiplexer.CamerasVisible = true;
			multiplexer.CellWidth = view.CellWidth;
			multiplexer.CellHeight = view.CellHeight;

			// set title
			this.Text = title + " - " + fullName;

			// reset statistics indexes
			statIndex = 0;
			statReady = 0;

			//
			openedID = view.ID;
			viewOpened = true;

			// start timer
			timer.Start();
		}

		// On "Views->Open view" menu item click
		private void openViewViewsItem_Click(object sender, System.EventArgs e)
		{
			OpenView(camerasTree.SelectedNode);
		}

		// On cameras tree context menu "Open view"
		private void openViewItem_Click(object sender, System.EventArgs e)
		{
			camerasTree.SelectedNode = camerasTree.LastClickNode;
			OpenView(camerasTree.LastClickNode);
		}

		// On timer event - gather statistic
		private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			if (runningPool.Count != 0)
			{
				float	fps = 0, bps = 0;

				// get number of frames and bytes received for the last second
				statCount[statIndex]	= 0;
				statReceived[statIndex]	= 0;
				foreach (Camera camera in runningPool)
				{
					statCount[statIndex]	+= camera.FramesReceived;
					statReceived[statIndex]	+= camera.BytesReceived;
				}

				// increment indexes
				if (++statIndex >= statLength)
					statIndex = 0;
				if (statReady < statLength)
					statReady++;

				// calculate average value
				for (int i = 0; i < statReady; i++)
				{
					fps += statCount[i];
					bps += statReceived[i];
				}
				fps /= statReady;
				bps /= (statReady * 1024);

				statReceived[statIndex] = 0;
				statCount[statIndex] = 0;

				fpsPanel.Text = fps.ToString("F2") + " fps";
				bpsPanel.Text = bps.ToString("F2") + " Kb/s";
			}
		}

		// On "Camera info" from camera context menu
		private void cameraInfoItem_Click(object sender, System.EventArgs e)
		{
			cameraInfo.Camera = multiplexer.ContextCamera;
			cameraInfo.TopMost = config.fullScreen;
			cameraInfo.ShowDialog();
		}
	}
}
