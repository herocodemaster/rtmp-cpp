// Camera Vision
//
// Copyright © Andrew Kirillov, 2005-2006
// andrew.kirillov@gmail.com
//
namespace CameraViewer
{
	using System;

	/// <summary>
	/// View
	/// </summary>
	public class View
	{
		private int		id = 0;
		private string	name;
		private string	description = "";
		private Group	parent = null;

		private short	cols = 3;
		private short	rows = 3;
		private short	cellWidth = 320;
		private short	cellHeight = 240;

		private int[,]	cameraIDs = new int[5, 5];

		public const int MaxRows = 5;
		public const int MaxCols = 5;

		// ID property
		public int ID
		{
			get { return id; }
			set { id = value; }
		}	
		// Name property
		public string Name
		{
			get { return name; }
			set { name = value; }
		}
		// Description property
		public string Description
		{
			get { return description; }
			set { description = value; }
		}
		// Parent property
		public Group Parent
		{
			get { return parent; }
			set { parent = value; }
		}
		// FullName property
		public string FullName
		{
			get
			{
				return (parent == null) ? name : (parent.FullName + '\\' + name);
			}
		}

		// Cols property
		public short Cols
		{
			get { return cols; }
			set { cols = Math.Max((short) 1, Math.Min((short) MaxCols, value)); }
		}
		// Rows property
		public short Rows
		{
			get { return rows; }
			set { rows = Math.Max((short) 1, Math.Min((short) MaxRows, value)); }
		}
		// CellWidth property
		public short CellWidth
		{
			get { return cellWidth; }
			set { cellWidth = Math.Max((short) 50, Math.Min((short) 800, value)); }
		}
		// CellHeight property
		public short CellHeight
		{
			get { return cellHeight; }
			set { cellHeight = Math.Max((short) 50, Math.Min((short) 800, value)); }
		}


		// Constructor
		public View(string name)
		{
			this.name = name;
		}

		// Set camera
		public void SetCamera(int row, int col, int cameraID)
		{
			cameraIDs[row, col] = cameraID;
		}
		// Get camera
		public int GetCamera(int row, int col)
		{
			if ((row >= 0) && (col >= 0) && (row < MaxRows) && (col < MaxCols))
			{
				return cameraIDs[row, col];
			}
			return -1;
		}
	}
}
