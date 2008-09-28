// Camera Vision
//
// Copyright © Andrew Kirillov, 2005-2006
// andrew.kirillov@gmail.com
//
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;


namespace CameraViewer
{
	public class ViewPropertiesForm : CameraViewer.PagedWizard
	{
		private View view = new View("");
		private ViewDescription	page1 = new ViewDescription();
		private ViewStructure	page2 = new ViewStructure();

		// View property
		public View View
		{
			get { return view; }
			set
			{
				view = value;

				page1.View = view;
				page2.View = view;
			}
		}

		// CheckViewFunction property
		public CheckViewHandler CheckViewFunction
		{
			set { page1.CheckViewFunction = value; }
		}


		// Construction
		public ViewPropertiesForm()
		{
			this.AddPage(page1);
			this.AddPage(page2);
			this.Text = "View properties";

			page1.View = view;
			page2.View = view;

			this.Size = new Size(540, 370);
		}

		// Build cameras tree
		public void BuildCamerasTree(GroupCollection groups, CameraCollection cameras)
		{
			page2.BuildCamerasTree(groups, cameras);
		}

	}
}

