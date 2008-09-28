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
	public class ViewForm : CameraViewer.Wizard
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
		public ViewForm()
		{
			this.AddPage(page1);
			this.AddPage(page2);
			this.Text = "New view wizard";

			page1.View = view;
			page2.View = view;

			this.Size = new Size(600, 400);
		}

		// Build cameras tree
		public void BuildCamerasTree(GroupCollection groups, CameraCollection cameras)
		{
			page2.BuildCamerasTree(groups, cameras);
		}

		// On page changing
		protected override void OnPageChanging(int page)
		{
			base.OnPageChanging(page);
		}
	}
}

