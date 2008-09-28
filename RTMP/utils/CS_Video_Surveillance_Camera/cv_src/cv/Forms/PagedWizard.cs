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
	/// Summary description for PagedWizard.
	/// </summary>
	public class PagedWizard : System.Windows.Forms.Form
	{
		private Control currentControl = null;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button applyButton;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		// SelectedPageIndex property
		public int SelectedPageIndex
		{
			get { return tabControl.SelectedIndex; }
		}

		// Apply event
		public event EventHandler Apply;

		// Constructor
		public PagedWizard()
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
			this.tabControl = new System.Windows.Forms.TabControl();
			this.cancelButton = new System.Windows.Forms.Button();
			this.okButton = new System.Windows.Forms.Button();
			this.applyButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// tabControl
			// 
			this.tabControl.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(394, 240);
			this.tabControl.TabIndex = 0;
			this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.cancelButton.Location = new System.Drawing.Point(312, 248);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.TabIndex = 3;
			this.cancelButton.Text = "&Cancel";
			// 
			// okButton
			// 
			this.okButton.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.okButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.okButton.Location = new System.Drawing.Point(232, 248);
			this.okButton.Name = "okButton";
			this.okButton.TabIndex = 2;
			this.okButton.Text = "&Ok";
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// applyButton
			// 
			this.applyButton.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.applyButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.applyButton.Location = new System.Drawing.Point(144, 248);
			this.applyButton.Name = "applyButton";
			this.applyButton.TabIndex = 1;
			this.applyButton.Text = "&Apply";
			this.applyButton.Click += new System.EventHandler(this.applyButton_Click);
			// 
			// PagedWizard
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(394, 278);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.applyButton,
																		  this.okButton,
																		  this.cancelButton,
																		  this.tabControl});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "PagedWizard";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Load += new System.EventHandler(this.PagedWizard_Load);
			this.ResumeLayout(false);

		}
		#endregion

		// Add page
		public void AddPage(IWizardPage page)
		{
			Control ctrl = (Control) page;

			// add new tab
			TabPage tabPage = new TabPage();
			tabPage.TabIndex = tabControl.TabCount;
			tabPage.Text = page.PageName;
			tabControl.Controls.Add(tabPage);

			// add page to tab
			tabPage.Controls.Add(ctrl);
			ctrl.Dock = DockStyle.Fill;

			page.StateChanged += new EventHandler(page_StateChanged);
		}

		// On form load
		private void PagedWizard_Load(object sender, System.EventArgs e)
		{
			// set current page to the first
			SetCurrentPage(0);
		}

		// Update control buttons state
		private void UpdateControlButtons()
		{
			// "Apply" button
			applyButton.Enabled = ((currentControl != null) && (((IWizardPage) currentControl).Completed));
			// "Ok" button
			okButton.Enabled = true;
			foreach (Control ctrl in tabControl.Controls)
			{
				if (!((IWizardPage) ctrl.Controls[0]).Completed)
				{
					okButton.Enabled = false;
					break;
				}
			}
		}

		// Set current page
		private void SetCurrentPage(int n)
		{
			// get current page
			currentControl = tabControl.Controls[n].Controls[0];
			IWizardPage	page = (IWizardPage) currentControl;

			// notify the page
			page.Display();

			// update conrol buttons
			UpdateControlButtons();
		}

		// Selection changed in tab control
		private void tabControl_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			SetCurrentPage(tabControl.SelectedIndex);
		}

		// On "Apply" button click
		private void applyButton_Click(object sender, System.EventArgs e)
		{
			if ((((IWizardPage) currentControl).Apply() == true) && (Apply != null))
			{
				Apply(this, new EventArgs());
			}
		}

		// On "Ok" button click
		private void okButton_Click(object sender, System.EventArgs e)
		{
			// apply all pages
			foreach (Control ctrl in tabControl.Controls)
			{
				if (!((IWizardPage) ctrl.Controls[0]).Apply())
				{
					return;
				}
			}

			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		// On page state changed
		private void page_StateChanged(object sender, System.EventArgs e)
		{
			// update conrol buttons
			UpdateControlButtons();
		}
	}
}
