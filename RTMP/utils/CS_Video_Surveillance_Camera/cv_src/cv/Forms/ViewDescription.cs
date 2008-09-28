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
	// Check view delegate
	public delegate bool CheckViewHandler(View view);

	/// <summary>
	/// Summary description for ViewDescription.
	/// </summary>
	public class ViewDescription : System.Windows.Forms.UserControl, IWizardPage
	{
		private View view = null;
		private bool completed = false;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox nameBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox descriptionBox;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox cellWidthBox;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox cellHeightBox;
		private System.Windows.Forms.ComboBox rowsCombo;
		private System.Windows.Forms.ComboBox colsCombo;

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

					nameBox.Text = view.Name;
					descriptionBox.Text = view.Description;

					colsCombo.SelectedIndex = view.Cols - 1;
					rowsCombo.SelectedIndex = view.Rows - 1;
					cellWidthBox.Text = view.CellWidth.ToString();
					cellHeightBox.Text = view.CellHeight.ToString();
				}
			}
		}

		// Check view function
		private CheckViewHandler checkViewFunction;

		// CheckViewFunction property
		public CheckViewHandler CheckViewFunction
		{
			set { checkViewFunction = value; }
		}

		// Constructor
		public ViewDescription()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitForm call

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
			this.label1 = new System.Windows.Forms.Label();
			this.nameBox = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.descriptionBox = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.cellHeightBox = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.cellWidthBox = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.rowsCombo = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.colsCombo = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(10, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(40, 15);
			this.label1.TabIndex = 0;
			this.label1.Text = "&Name:";
			// 
			// nameBox
			// 
			this.nameBox.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.nameBox.Location = new System.Drawing.Point(60, 10);
			this.nameBox.Name = "nameBox";
			this.nameBox.Size = new System.Drawing.Size(290, 20);
			this.nameBox.TabIndex = 1;
			this.nameBox.Text = "";
			this.nameBox.TextChanged += new System.EventHandler(this.nameBox_TextChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(10, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(70, 15);
			this.label2.TabIndex = 2;
			this.label2.Text = "&Description:";
			// 
			// descriptionBox
			// 
			this.descriptionBox.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.descriptionBox.Location = new System.Drawing.Point(10, 60);
			this.descriptionBox.Multiline = true;
			this.descriptionBox.Name = "descriptionBox";
			this.descriptionBox.Size = new System.Drawing.Size(340, 80);
			this.descriptionBox.TabIndex = 3;
			this.descriptionBox.Text = "";
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.groupBox1.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.cellHeightBox,
																					this.label6,
																					this.cellWidthBox,
																					this.label5,
																					this.rowsCombo,
																					this.label4,
																					this.colsCombo,
																					this.label3});
			this.groupBox1.Location = new System.Drawing.Point(10, 150);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(340, 80);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "View size";
			// 
			// cellHeightBox
			// 
			this.cellHeightBox.Location = new System.Drawing.Point(220, 50);
			this.cellHeightBox.Name = "cellHeightBox";
			this.cellHeightBox.Size = new System.Drawing.Size(50, 20);
			this.cellHeightBox.TabIndex = 7;
			this.cellHeightBox.Text = "";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(150, 53);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(70, 15);
			this.label6.TabIndex = 6;
			this.label6.Text = "Cell height:";
			// 
			// cellWidthBox
			// 
			this.cellWidthBox.Location = new System.Drawing.Point(80, 50);
			this.cellWidthBox.Name = "cellWidthBox";
			this.cellWidthBox.Size = new System.Drawing.Size(50, 20);
			this.cellWidthBox.TabIndex = 5;
			this.cellWidthBox.Text = "";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(10, 53);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(70, 15);
			this.label5.TabIndex = 4;
			this.label5.Text = "Cell width:";
			// 
			// rowsCombo
			// 
			this.rowsCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.rowsCombo.Items.AddRange(new object[] {
														   "1",
														   "2",
														   "3",
														   "4",
														   "5"});
			this.rowsCombo.Location = new System.Drawing.Point(220, 20);
			this.rowsCombo.Name = "rowsCombo";
			this.rowsCombo.Size = new System.Drawing.Size(50, 21);
			this.rowsCombo.TabIndex = 3;
			this.rowsCombo.SelectedIndexChanged += new System.EventHandler(this.rowsCombo_SelectedIndexChanged);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(150, 23);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(70, 15);
			this.label4.TabIndex = 2;
			this.label4.Text = "Rows:";
			// 
			// colsCombo
			// 
			this.colsCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.colsCombo.Items.AddRange(new object[] {
														   "1",
														   "2",
														   "3",
														   "4",
														   "5"});
			this.colsCombo.Location = new System.Drawing.Point(80, 20);
			this.colsCombo.Name = "colsCombo";
			this.colsCombo.Size = new System.Drawing.Size(50, 21);
			this.colsCombo.TabIndex = 1;
			this.colsCombo.SelectedIndexChanged += new System.EventHandler(this.colsCombo_SelectedIndexChanged);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(10, 23);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(65, 15);
			this.label3.TabIndex = 0;
			this.label3.Text = "Columns:";
			// 
			// ViewDescription
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.groupBox1,
																		  this.descriptionBox,
																		  this.label2,
																		  this.nameBox,
																		  this.label1});
			this.Name = "ViewDescription";
			this.Size = new System.Drawing.Size(360, 240);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		// PageName property
		public string PageName
		{
			get { return "Description"; }
		}

		// PageDescription property
		public string PageDescription
		{
			get { return "View description"; }
		}

		// Completed property
		public bool Completed
		{
			get { return completed; }
		}

		// Show the page
		public void Display()
		{
			nameBox.Focus();
			nameBox.SelectionStart = nameBox.TextLength;
		}

		// Apply the page
		public bool Apply()
		{
			string name = nameBox.Text.Replace('\\', ' ');

			if (checkViewFunction != null)
			{
				View tmpView = new View(name);

				tmpView.ID = view.ID;
				tmpView.Parent = view.Parent;

				// check view
				if (checkViewFunction(tmpView) == false)
				{
					Color	tmp = this.nameBox.BackColor;

					// highlight name edit box
					this.nameBox.BackColor = Color.LightCoral;
					// error message
					MessageBox.Show(this, "A view with such name is already exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
					// restore & focus name edit box
					this.nameBox.BackColor = tmp;
					this.nameBox.Focus();

					return false;
				}
			}

			// update view name and description
			view.Name = name;
			view.Description = descriptionBox.Text;

			try
			{
				view.CellWidth = short.Parse(cellWidthBox.Text);
				view.CellHeight = short.Parse(cellHeightBox.Text);
			}
			catch (Exception)
			{
			}

			return true;
		}

		// On Name edit box changed
		private void nameBox_TextChanged(object sender, System.EventArgs e)
		{
			completed = (nameBox.TextLength != 0);

			if (StateChanged != null)
				StateChanged(this, new EventArgs());
		}

		// Cols changed
		private void colsCombo_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			// update view size
			view.Cols = (short) (colsCombo.SelectedIndex + 1);
		}

		// Rows changed
		private void rowsCombo_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			// update view size
			view.Rows = (short) (rowsCombo.SelectedIndex + 1);
		}
	}
}
