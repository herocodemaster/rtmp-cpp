// Camera Vision
//
// Copyright © Andrew Kirillov, 2005-2006
// andrew.kirillov@gmail.com
//
namespace CameraViewer
{
	using System;

	/// <summary>
	/// Group class
	/// </summary>
	public class Group
	{
		private int		id = 0;
		private string	name;
		private string	description = "";
		private Group	parent = null;

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

		// Constructor
		public Group(string name)
		{
			this.name = name;
		}
	}
}
