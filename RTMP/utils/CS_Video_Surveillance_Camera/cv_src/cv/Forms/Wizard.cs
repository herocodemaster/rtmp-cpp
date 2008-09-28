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
	/// Summary description for Wizard.
	/// </summary>
	public class Wizard : System.Windows.Forms.Form
	{
		private string title;
		private int currentPage;
		private Control currentControl = null;

		private System.Windows.Forms.Panel descPannel;
		private System.Windows.Forms.Panel controlPanel;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button finishButton;
		private System.Windows.Forms.Button nextButton;
		private System.Windows.Forms.Button backButton;
		private System.Windows.Forms.PictureBox line1;
		private System.Windows.Forms.PictureBox line2;
		private System.Windows.Forms.Label descriptionLabel;
		private System.Windows.Forms.Panel imagePanel;
		private System.Windows.Forms.PictureBox line3;
		private System.Windows.Forms.Panel workPanel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;


		// Constructor
		public Wizard()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
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
			this.descPannel = new System.Windows.Forms.Panel();
			this.descriptionLabel = new System.Windows.Forms.Label();
			this.controlPanel = new System.Windows.Forms.Panel();
			this.nextButton = new System.Windows.Forms.Button();
			this.finishButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.backButton = new System.Windows.Forms.Button();
			this.line1 = new System.Windows.Forms.PictureBox();
			this.line2 = new System.Windows.Forms.PictureBox();
			this.imagePanel = new System.Windows.Forms.Panel();
			this.line3 = new System.Windows.Forms.PictureBox();
			this.workPanel = new System.Windows.Forms.Panel();
			this.descPannel.SuspendLayout();
			this.controlPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// descPannel
			// 
			this.descPannel.BackColor = System.Drawing.Color.White;
			this.descPannel.Controls.AddRange(new System.Windows.Forms.Control[] {
																					 this.descriptionLabel});
			this.descPannel.Dock = System.Windows.Forms.DockStyle.Top;
			this.descPannel.Name = "descPannel";
			this.descPannel.Size = new System.Drawing.Size(444, 56);
			this.descPannel.TabIndex = 0;
			// 
			// descriptionLabel
			// 
			this.descriptionLabel.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.descriptionLabel.Location = new System.Drawing.Point(8, 10);
			this.descriptionLabel.Name = "descriptionLabel";
			this.descriptionLabel.Size = new System.Drawing.Size(424, 40);
			this.descriptionLabel.TabIndex = 0;
			this.descriptionLabel.Text = "Hello";
			// 
			// controlPanel
			// 
			this.controlPanel.Controls.AddRange(new System.Windows.Forms.Control[] {
																					   this.nextButton,
																					   this.finishButton,
																					   this.cancelButton,
																					   this.backButton});
			this.controlPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.controlPanel.Location = new System.Drawing.Point(0, 270);
			this.controlPanel.Name = "controlPanel";
			this.controlPanel.Size = new System.Drawing.Size(444, 48);
			this.controlPanel.TabIndex = 1;
			// 
			// nextButton
			// 
			this.nextButton.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.nextButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.nextButton.Location = new System.Drawing.Point(178, 16);
			this.nextButton.Name = "nextButton";
			this.nextButton.TabIndex = 2;
			this.nextButton.Text = "&Next";
			this.nextButton.Click += new System.EventHandler(this.nextButton_Click);
			// 
			// finishButton
			// 
			this.finishButton.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.finishButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.finishButton.Location = new System.Drawing.Point(282, 16);
			this.finishButton.Name = "finishButton";
			this.finishButton.TabIndex = 3;
			this.finishButton.Text = "&Finish";
			this.finishButton.Click += new System.EventHandler(this.finishButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.cancelButton.Location = new System.Drawing.Point(362, 16);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.TabIndex = 4;
			this.cancelButton.Text = "&Cancel";
			// 
			// backButton
			// 
			this.backButton.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.backButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.backButton.Location = new System.Drawing.Point(98, 16);
			this.backButton.Name = "backButton";
			this.backButton.TabIndex = 1;
			this.backButton.Text = "&Back";
			this.backButton.Click += new System.EventHandler(this.backButton_Click);
			// 
			// line1
			// 
			this.line1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.line1.Dock = System.Windows.Forms.DockStyle.Top;
			this.line1.Location = new System.Drawing.Point(0, 56);
			this.line1.Name = "line1";
			this.line1.Size = new System.Drawing.Size(444, 1);
			this.line1.TabIndex = 2;
			this.line1.TabStop = false;
			// 
			// line2
			// 
			this.line2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.line2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.line2.Location = new System.Drawing.Point(0, 268);
			this.line2.Name = "line2";
			this.line2.Size = new System.Drawing.Size(444, 2);
			this.line2.TabIndex = 3;
			this.line2.TabStop = false;
			// 
			// imagePanel
			// 
			this.imagePanel.BackColor = System.Drawing.SystemColors.Highlight;
			this.imagePanel.Dock = System.Windows.Forms.DockStyle.Left;
			this.imagePanel.Location = new System.Drawing.Point(0, 57);
			this.imagePanel.Name = "imagePanel";
			this.imagePanel.Size = new System.Drawing.Size(96, 211);
			this.imagePanel.TabIndex = 4;
			// 
			// line3
			// 
			this.line3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.line3.Dock = System.Windows.Forms.DockStyle.Left;
			this.line3.Location = new System.Drawing.Point(96, 57);
			this.line3.Name = "line3";
			this.line3.Size = new System.Drawing.Size(1, 211);
			this.line3.TabIndex = 5;
			this.line3.TabStop = false;
			// 
			// workPanel
			// 
			this.workPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.workPanel.Location = new System.Drawing.Point(97, 57);
			this.workPanel.Name = "workPanel";
			this.workPanel.Size = new System.Drawing.Size(347, 211);
			this.workPanel.TabIndex = 0;
			// 
			// Wizard
			// 
			this.AcceptButton = this.nextButton;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(444, 318);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.workPanel,
																		  this.line3,
																		  this.imagePanel,
																		  this.line2,
																		  this.line1,
																		  this.controlPanel,
																		  this.descPannel});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Wizard";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Load += new System.EventHandler(this.Wizard_Load);
			this.descPannel.ResumeLayout(false);
			this.controlPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		// Add page
		public void AddPage(IWizardPage page)
		{
			Control ctrl = (Control) page;

			workPanel.Controls.Add(ctrl);
			ctrl.Dock = DockStyle.Fill;

			page.StateChanged += new EventHandler(page_StateChanged);
			page.Reset += new EventHandler(page_Reset);
		}
		
		// On form load
		private void Wizard_Load(object sender, System.EventArgs e)
		{
			// save original title
			title = this.Text;

			// set current page to the first
			SetCurrentPage(0);
		}

		// Update control buttons state
		private void UpdateControlButtons()
		{
			// "Back" button
			backButton.Enabled = (currentPage != 0);
			// "Next" button
			nextButton.Enabled = ((currentPage < workPanel.Controls.Count - 1) && (currentControl != null) && (((IWizardPage) currentControl).Completed));
			// "Finish" button
			finishButton.Enabled = true;
			foreach (IWizardPage page in workPanel.Controls)
			{
				if (!page.Completed)
				{
					finishButton.Enabled = false;
					break;
				}
			}
		}

		// Set current page
		private void SetCurrentPage(int n)
		{
			OnPageChanging(n);

			// hide previous page
			if (currentControl != null)
			{
				currentControl.Hide();
			}

			//
			currentPage = n;

			// update dialog text
			this.Text = title + " - Page " + ((int)(n + 1)).ToString() + " of " + workPanel.Controls.Count.ToString();

			// show new page
			currentControl = workPanel.Controls[currentPage];
			IWizardPage	page = (IWizardPage) currentControl;

			currentControl.Show();

			// description
			descriptionLabel.Text = page.PageDescription;

			// notify the page
			page.Display();

			// update conrol buttons
			UpdateControlButtons();
		}

		// On "Next" button click
		private void nextButton_Click(object sender, System.EventArgs e)
		{
			if (((IWizardPage) currentControl).Apply() == true)
			{
				if (currentPage < workPanel.Controls.Count - 1)
				{
					SetCurrentPage(currentPage + 1);
				}
			}
		}

		// On "Back" button click
		private void backButton_Click(object sender, System.EventArgs e)
		{
			if (currentPage > 0)
			{
				SetCurrentPage(currentPage - 1);
			}
		}

		// On "Finish" button click
		private void finishButton_Click(object sender, System.EventArgs e)
		{
			if (((IWizardPage) currentControl).Apply() == true)
			{
				OnFinish();

				// close the dialog
				this.DialogResult = DialogResult.OK;
				this.Close();
			}
		}

		// On page state changed
		private void page_StateChanged(object sender, System.EventArgs e)
		{
			// update conrol buttons
			UpdateControlButtons();
		}

		// Reset on page
		private void page_Reset(object sender, System.EventArgs e)
		{
			OnResetOnPage(workPanel.Controls.GetChildIndex((Control) sender));

			// update conrol buttons
			UpdateControlButtons();
		}

		// Before page changing
		protected virtual void OnPageChanging(int page)
		{
		}

		// Reset event ocuren on page
		protected virtual void OnResetOnPage(int page)
		{
		}

		// On dialog finish
		protected virtual void OnFinish()
		{
		}
	}
}
