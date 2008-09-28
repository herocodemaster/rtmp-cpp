// Camera Vision
//
// Copyright © Andrew Kirillov, 2005-2006
// andrew.kirillov@gmail.com
//
namespace CameraViewer
{
	using System;
	using System.Drawing;
	using System.IO;
	using System.Xml;
	using System.Text;

	using videosource;

	/// <summary>
	/// Application configuration
	/// </summary>
	public class Configuration
	{
		// configuration file name
		private string settingsFile;
		private string camerasFile;
		private string viewsFile;

		// main window size and position
		public Point	mainWindowLocation = new Point(100, 50);
		public Size		mainWindowSize = new Size(800, 600);

		// cameras tree view width and status
		public bool		showCameraBar = true;
		public int		cameraBarWidth = 150;

		//
		public bool		fitToScreen = false;
		public bool		fullScreen = false;

		// IDs
		private int		nextCamerasGroupID = 1;
		private int		nextCameraID = 1;
		private int		nextViewsGroupID = 1;
		private int		nextViewID = 1;

		// providers
		public readonly VideoProviderCollection providers = new VideoProviderCollection();
		// cameras groups
		public readonly GroupCollection	camerasGroups = new GroupCollection();
		// cameras
		public readonly CameraCollection cameras = new CameraCollection();
		// view groups
		public readonly GroupCollection	viewsGroups = new GroupCollection();
		// view
		public readonly ViewCollection views = new ViewCollection();

		// Constructor
		public Configuration(string path)
		{
			settingsFile = Path.Combine(path, "app.config");
			camerasFile = Path.Combine(path, "cameras.config");
			viewsFile = Path.Combine(path, "views.config");
		}

		// Add new camera group
		public void AddCamerasGroup(Group group)
		{
			group.ID = nextCamerasGroupID++;
			camerasGroups.Add(group);

			// save
			SaveCameras();
		}

		// Get cameras group by name
		public Group GetCamerasGroupByName(string name)
		{
			return camerasGroups.GetGroupByName(name);
		}

		// Check camera group
		// check if there is already a group with such name
		// return true, if there is no such group
		public bool CheckCamerasGroup(Group group)
		{
			foreach (Group g in camerasGroups)
			{
				if ((group.Name == g.Name) && (group.Parent == g.Parent) && ((group.ID == 0) || (group.ID != g.ID)))
					return false;
			}
			return true;
		}

		// Delete cameras group
		// delete cameras group if it is empty or return false otherwise
		public bool DeleteCamerasGroup(Group group)
		{
			// check if there are subgroups in the group
			foreach (Group g in camerasGroups)
			{
				if (g.Parent == group)
					return false;
			}
			// check if there are cameras in the group
			foreach (Camera c in cameras)
			{
				if (c.Parent == group)
					return false;
			}
			camerasGroups.Remove(group);
			// save
			SaveCameras();
			return true;
		}

		// Add new camera
		public void AddCamera(Camera camera)
		{
			camera.ID = nextCameraID++;
			cameras.Add(camera);

			// save
			SaveCameras();
		}

		// Get camera by name
		public Camera GetCameraByName(string name)
		{
			string[]	nameParts = name.Split('\\');
			Group		group = null;

			// get group
			if (nameParts.Length > 1)
				group = camerasGroups.GetGroupByName(string.Join("\\", nameParts, 0, nameParts.Length - 1));

			// get camera
			return cameras.GetCamera(nameParts[nameParts.Length - 1], group);
		}

		// Check camera
		// check if there is already a camera with such name
		// return true, if there is no such camera
		public bool CheckCamera(Camera camera)
		{
			foreach (Camera c in cameras)
			{
				if ((camera.Name == c.Name) && (camera.Parent == c.Parent) && ((camera.ID == 0) || (camera.ID != c.ID)))
					return false;
			}
			return true;
		}

		// Delete camera
		public bool DeleteCamera(Camera camera)
		{
			cameras.Remove(camera);
			// save
			SaveCameras();
			return true;
		}

		// Add new views group
		public void AddViewsGroup(Group group)
		{
			group.ID = nextViewsGroupID++;
			viewsGroups.Add(group);

			// save
			SaveViews();
		}

		// Get views group by name
		public Group GetViewsGroupByName(string name)
		{
			return viewsGroups.GetGroupByName(name);
		}

		// Check views group
		// check if there is already a group with such name
		// return true, if there is no such group
		public bool CheckViewsGroup(Group group)
		{
			foreach (Group g in viewsGroups)
			{
				if ((group.Name == g.Name) && (group.Parent == g.Parent) && ((group.ID == 0) || (group.ID != g.ID)))
					return false;
			}
			return true;
		}

		// Delete views group
		// delete views group if it is empty or return false otherwise
		public bool DeleteViewsGroup(Group group)
		{
			// check if there are subgroups in the group
			foreach (Group g in viewsGroups)
			{
				if (g.Parent == group)
					return false;
			}
			// check if there are view in the group
			foreach (View v in views)
			{
				if (v.Parent == group)
					return false;
			}
			viewsGroups.Remove(group);
			// save
			SaveViews();
			return true;
		}

		// Add new view
		public void AddView(View view)
		{
			view.ID = nextViewID++;
			views.Add(view);

			// save
			SaveViews();
		}

		// Get view by name
		public View GetViewByName(string name)
		{
			string[]	nameParts = name.Split('\\');
			Group		group = null;

			// get group
			if (nameParts.Length > 1)
				group = viewsGroups.GetGroupByName(string.Join("\\", nameParts, 0, nameParts.Length - 1));

			// get camera
			return views.GetView(nameParts[nameParts.Length - 1], group);
		}

		// Check view
		// check if there is already a view with such name
		// return true, if there is no such view
		public bool CheckView(View view)
		{
			foreach (View v in views)
			{
				if ((view.Name == v.Name) && (view.Parent == v.Parent) && ((view.ID == 0) || (view.ID != v.ID)))
					return false;
			}
			return true;
		}

		// Delete view
		public bool DeleteView(View view)
		{
			views.Remove(view);
			// save
			SaveViews();
			return true;
		}

		// Save application settings
		public void SaveSettings()
		{
			// open file
			FileStream		fs = new FileStream(settingsFile, FileMode.Create);
			// create XML writer
			XmlTextWriter	xmlOut = new XmlTextWriter(fs, Encoding.UTF8); 

			// use indenting for readability
			xmlOut.Formatting = Formatting.Indented;

			// start document
			xmlOut.WriteStartDocument();
			xmlOut.WriteComment("CameraViewer configuration file");

			// main node
			xmlOut.WriteStartElement("CameraViewer");

			// main window node
			xmlOut.WriteStartElement("MainWindow");
			xmlOut.WriteAttributeString("x", mainWindowLocation.X.ToString());
			xmlOut.WriteAttributeString("y", mainWindowLocation.Y.ToString());
			xmlOut.WriteAttributeString("width", mainWindowSize.Width.ToString());
			xmlOut.WriteAttributeString("height", mainWindowSize.Height.ToString());
			xmlOut.WriteEndElement();

			// view node
			xmlOut.WriteStartElement("View");
			xmlOut.WriteAttributeString("cameraBar", showCameraBar.ToString());
			xmlOut.WriteAttributeString("cameraBarWidth", cameraBarWidth.ToString());
			xmlOut.WriteEndElement();

			xmlOut.WriteEndElement();

			// close file
			xmlOut.Close();
		}

		// Load application settings
		public bool LoadSettings()
		{
			bool	ret = false;

			// check file existance
			if (File.Exists(settingsFile))
			{
				FileStream		fs = null;
				XmlTextReader	xmlIn = null;

				try
				{
					// open file
					fs = new FileStream(settingsFile, FileMode.Open);
					// create XML reader
					xmlIn = new XmlTextReader(fs);

					xmlIn.WhitespaceHandling = WhitespaceHandling.None;
					xmlIn.MoveToContent();

					// check for main node
					if (xmlIn.Name != "CameraViewer")
						throw new ApplicationException("");

					// move to next node
					xmlIn.Read();
					if (xmlIn.NodeType == XmlNodeType.EndElement)
						xmlIn.Read();

					// check for main window node
					if (xmlIn.Name != "MainWindow")
						throw new ApplicationException("");

					// read main window position
					int		x = Convert.ToInt32(xmlIn.GetAttribute("x"));
					int		y = Convert.ToInt32(xmlIn.GetAttribute("y"));
					int		width = Convert.ToInt32(xmlIn.GetAttribute("width"));
					int		height = Convert.ToInt32(xmlIn.GetAttribute("height"));

					// move to next node
					xmlIn.Read();
					if (xmlIn.NodeType == XmlNodeType.EndElement)
						xmlIn.Read();

					// check for view node
					if (xmlIn.Name != "View")
						throw new ApplicationException("");

					showCameraBar = Convert.ToBoolean(xmlIn.GetAttribute("cameraBar"));
					cameraBarWidth = Convert.ToInt32(xmlIn.GetAttribute("cameraBarWidth"));

					mainWindowLocation = new Point(x, y);
					mainWindowSize = new Size(width, height);

					ret = true;
				}
				// catch any exceptions
				catch (Exception)
				{
				}
				finally
				{
					if (xmlIn != null)
						xmlIn.Close();
				}
			}
			return ret;
		}

		// Save cameras list to file
		public void SaveCameras()
		{
			// open file
			FileStream		fs = new FileStream(camerasFile, FileMode.Create);
			// create XML writer
			XmlTextWriter	xmlOut = new XmlTextWriter(fs, Encoding.UTF8);

			// use indenting for readability
			xmlOut.Formatting = Formatting.Indented;

			// start document
			xmlOut.WriteStartDocument();

			// main node
			xmlOut.WriteStartElement("Cameras");

			// save all cameras
			SaveCameras(xmlOut, null);

			// close "Cameras" node
			xmlOut.WriteEndElement();

			// close file
			xmlOut.Close();
		}

		// save cameras of the parent group
		private void SaveCameras(XmlTextWriter writer, Group parent)
		{
			foreach (Group group in camerasGroups)
			{
				if (group.Parent == parent)
				{
					// new "Group" node
					writer.WriteStartElement("Group");

					// write node
					writer.WriteAttributeString("id", group.ID.ToString());
					writer.WriteAttributeString("name", group.Name);
					writer.WriteAttributeString("desc", group.Description);

					// write child groups
					SaveCameras(writer, group);

					// close "Group" node
					writer.WriteEndElement();
				}
			}
			foreach (Camera camera in cameras)
			{
				if (camera.Parent == parent)
				{
					// new "Camera" node
					writer.WriteStartElement("Camera");

					// write node
					writer.WriteAttributeString("id", camera.ID.ToString());
					writer.WriteAttributeString("name", camera.Name);
					writer.WriteAttributeString("desc", camera.Description);

					if (camera.Provider != null)
					{
						// write provider name
						writer.WriteAttributeString("provider", camera.Provider.ProviderName);

						if (camera.Configuration != null)
						{
							// write camera configuration
							camera.Provider.SaveConfiguration(writer, camera.Configuration);
						}
					}

					// close "Camera" node
					writer.WriteEndElement();
				}
			}
		}

		// Load cameras collection from file
		public void LoadCameras()
		{
			// check file existance
			if (File.Exists(camerasFile))
			{
				FileStream		fs = null;
				XmlTextReader	xmlIn = null;

				try
				{
					// open file
					fs = new FileStream(camerasFile, FileMode.Open);
					// create XML reader
					xmlIn = new XmlTextReader(fs);

					xmlIn.WhitespaceHandling = WhitespaceHandling.None;
					xmlIn.MoveToContent();

					// check for main node
					if (xmlIn.Name != "Cameras")
						throw new ApplicationException("");

					// move to next node
					xmlIn.Read();
					if (xmlIn.NodeType == XmlNodeType.EndElement)
						xmlIn.Read();

					// load cameras
					LoadCameras(xmlIn, null);
				}
				// catch any exceptions
				catch (Exception)
				{
				}
				finally
				{
					if (xmlIn != null)
						xmlIn.Close();
				}
			}
		}

		// Load cameras
		private void LoadCameras(XmlTextReader reader, Group parent)
		{
			// load all groups
			while (reader.Name == "Group")
			{
				int	depth = reader.Depth;

				// create new group
				Group group = new Group(reader.GetAttribute("name"));
				group.ID = int.Parse(reader.GetAttribute("id"));
				group.Description = reader.GetAttribute("desc");
				group.Parent = parent;

				// add group
				camerasGroups.Add(group);

				if (group.ID >= nextCamerasGroupID)
					nextCamerasGroupID = group.ID + 1;

				// move to next node
				reader.Read();

				// move to next element node
				while (reader.NodeType == XmlNodeType.EndElement)
					reader.Read();
				// read children
				if (reader.Depth > depth)
					LoadCameras(reader, group);
				//
				if (reader.Depth < depth)
					return;
			}
			// load all cameras
			while (reader.Name == "Camera")
			{
				int	depth = reader.Depth;

				// create new camera
				Camera camera = new Camera(reader.GetAttribute("name"));

				camera.ID			= int.Parse(reader.GetAttribute("id"));
				camera.Description	= reader.GetAttribute("desc");
				camera.Parent		= parent;
				camera.Provider		= providers.GetProviderByName(reader.GetAttribute("provider"));

				// load configuration
				if (camera.Provider != null)
					camera.Configuration = camera.Provider.LoadConfiguration(reader);

				// add camera
				cameras.Add(camera);

				if (camera.ID >= nextCameraID)
					nextCameraID = camera.ID + 1;

				// move to next node
				reader.Read();

				// move to next element node
				while (reader.NodeType == XmlNodeType.EndElement)
					reader.Read();
				if (reader.Depth < depth)
					return;
			}
		}

		// Save views list to file
		public void SaveViews()
		{
			// open file
			FileStream		fs = new FileStream(viewsFile, FileMode.Create);
			// create XML writer
			XmlTextWriter	xmlOut = new XmlTextWriter(fs, Encoding.UTF8);

			// use indenting for readability
			xmlOut.Formatting = Formatting.Indented;

			// start document
			xmlOut.WriteStartDocument();

			// main node
			xmlOut.WriteStartElement("Views");

			// save all cameras
			SaveViews(xmlOut, null);

			// close "Views" node
			xmlOut.WriteEndElement();

			// close file
			xmlOut.Close();
		}

		// save views of the parent group
		private void SaveViews(XmlTextWriter writer, Group parent)
		{
			foreach (Group group in viewsGroups)
			{
				if (group.Parent == parent)
				{
					// new "Group" node
					writer.WriteStartElement("Group");

					// write node
					writer.WriteAttributeString("id", group.ID.ToString());
					writer.WriteAttributeString("name", group.Name);
					writer.WriteAttributeString("desc", group.Description);

					// write child groups
					SaveViews(writer, group);

					// close "Group" node
					writer.WriteEndElement();
				}
			}
			foreach (View view in views)
			{
				if (view.Parent == parent)
				{
					// new "View" node
					writer.WriteStartElement("View");

					// write node
					writer.WriteAttributeString("id", view.ID.ToString());
					writer.WriteAttributeString("name", view.Name);
					writer.WriteAttributeString("desc", view.Description);

					// view size
					writer.WriteAttributeString("rows", view.Rows.ToString());
					writer.WriteAttributeString("cols", view.Cols.ToString());
					writer.WriteAttributeString("width", view.CellWidth.ToString());
					writer.WriteAttributeString("height", view.CellHeight.ToString());

					// write cameras
					string[] strIDs = new string[View.MaxRows * View.MaxCols];
					for (int i = 0, k = 0; i < View.MaxRows; i++)
					{
						for (int j = 0; j < View.MaxCols; j++, k++)
						{
							strIDs[k] = view.GetCamera(i, j).ToString();
						}
					}
					writer.WriteAttributeString("cameras", string.Join(",", strIDs));

					// close "View" node
					writer.WriteEndElement();
				}
			}
		}

		// Load views collection from file
		public void LoadViews()
		{
			// check file existance
			if (File.Exists(viewsFile))
			{
				FileStream		fs = null;
				XmlTextReader	xmlIn = null;

				try
				{
					// open file
					fs = new FileStream(viewsFile, FileMode.Open);
					// create XML reader
					xmlIn = new XmlTextReader(fs);

					xmlIn.WhitespaceHandling = WhitespaceHandling.None;
					xmlIn.MoveToContent();

					// check for main node
					if (xmlIn.Name != "Views")
						throw new ApplicationException("");

					// move to next node
					xmlIn.Read();
					if (xmlIn.NodeType == XmlNodeType.EndElement)
						xmlIn.Read();

					// load views
					LoadViews(xmlIn, null);
				}
				// catch any exceptions
				catch (Exception)
				{
				}
				finally
				{
					if (xmlIn != null)
						xmlIn.Close();
				}
			}
		}

		// Load views
		private void LoadViews(XmlTextReader reader, Group parent)
		{
			// load all groups
			while (reader.Name == "Group")
			{
				int	depth = reader.Depth;

				// create new group
				Group group = new Group(reader.GetAttribute("name"));
				group.ID = int.Parse(reader.GetAttribute("id"));
				group.Description = reader.GetAttribute("desc");
				group.Parent = parent;

				// add group
				viewsGroups.Add(group);

				if (group.ID >= nextViewsGroupID)
					nextViewsGroupID = group.ID + 1;

				// move to next node
				reader.Read();

				// move to next element node
				while (reader.NodeType == XmlNodeType.EndElement)
					reader.Read();
				// read children
				if (reader.Depth > depth)
					LoadViews(reader, group);
				//
				if (reader.Depth < depth)
					return;
			}
			// load all views
			while (reader.Name == "View")
			{
				int	depth = reader.Depth;

				// create new camera
				View view = new View(reader.GetAttribute("name"));

				view.ID				= int.Parse(reader.GetAttribute("id"));
				view.Description	= reader.GetAttribute("desc");
				view.Parent			= parent;

				// view size
				view.Rows			= short.Parse(reader.GetAttribute("rows"));
				view.Cols			= short.Parse(reader.GetAttribute("cols"));
				view.CellWidth		= short.Parse(reader.GetAttribute("width"));
				view.CellHeight		= short.Parse(reader.GetAttribute("height"));

				// read cameras
				string[] strIDs = reader.GetAttribute("cameras").Split(',');
				for (int i = 0, k = 0; i < View.MaxRows; i++)
				{
					for (int j = 0; j < View.MaxCols; j++, k++)
					{
						view.SetCamera(i, j, int.Parse(strIDs[k]));
					}
				}

				// add view
				views.Add(view);

				if (view.ID >= nextViewID)
					nextViewID = view.ID + 1;

				// move to next node
				reader.Read();

				// move to next element node
				while (reader.NodeType == XmlNodeType.EndElement)
					reader.Read();
				if (reader.Depth < depth)
					return;
			}
		}
	}
}
