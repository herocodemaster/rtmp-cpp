// Camera Vision
//
// Copyright © Andrew Kirillov, 2005-2006
// andrew.kirillov@gmail.com
//
namespace CameraViewer
{
	using System;
	using System.Collections;

	/// <summary>
	/// GroupCollection class
	/// </summary>
	public class GroupCollection : CollectionBase
	{
		// Constructor
		public GroupCollection()
		{
		}

		// Get group at the specified index
		public Group this[int index]
		{
			get
			{
				return ((Group) InnerList[index]);
			}
		}

		// Add new group to the collection
		public void Add(Group group)
		{
			InnerList.Add(group);
		}

		// Remove group from the collection
		public void Remove(Group group)
		{
			InnerList.Remove(group);
		}

		// Get group by it`s full name
		public Group GetGroupByName(string fullName)
		{
			string[]	names = fullName.Split('\\');
			Group		group = null;

			foreach (string name in names)
			{
				group = GetGroup(name, group);
				if (group == null)
					return null;
			}
			return group;
		}

		// Get group with specified name
		public Group GetGroup(string name, Group parent)
		{
			// find the group
			foreach (Group group in InnerList)
			{
				if ((group.Name == name) && (group.Parent == parent))
					return group;
			}
			return null;
		}
	}
}
