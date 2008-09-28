// Camara Vision
//
// Copyright © Andrew Kirillov, 2005-2006
// andrew.kirillov@gmail.com
//

namespace axis
{
	using System;
	using System.Xml;
	using videosource;

	/// <summary>
	/// Axis2460Description
	/// </summary>
	public class Axis2460Description : IVideoSourceDescription
	{
		// Name property
		public string Name
		{
			get { return "Axis 2460 Network DVR"; }
		}

		// Description property
		public string Description
		{
			get { return "Provides access to Axis 2460 network digital video recorder"; }
		}

		// Return settings page
		public IVideoSourcePage GetSettingsPage()
		{
			return new Axis2460SetupPage();
		}

		// Save configuration
		public void SaveConfiguration(XmlTextWriter writer, object config)
		{
			AxisConfiguration cfg = (AxisConfiguration) config;

			if (cfg != null)
			{
				writer.WriteAttributeString("source", cfg.source);
				writer.WriteAttributeString("login", cfg.login);
				writer.WriteAttributeString("password", cfg.password);
				writer.WriteAttributeString("camera", cfg.camera.ToString());
				writer.WriteAttributeString("stype", ((int) cfg.stremType).ToString());
				writer.WriteAttributeString("interval", cfg.frameInterval.ToString());
			}
		}

		// Load configuration
		public object LoadConfiguration(XmlTextReader reader)
		{
			AxisConfiguration	config = new AxisConfiguration();

			try
			{
				config.source	= reader.GetAttribute("source");
				config.login	= reader.GetAttribute("login");
				config.password	= reader.GetAttribute("password");
				config.camera	= short.Parse(reader.GetAttribute("camera"));
				config.stremType = (StreamType) (int.Parse(reader.GetAttribute("stype")));
				config.frameInterval = int.Parse(reader.GetAttribute("interval"));
			}
			catch (Exception)
			{
			}
			return (object) config;
		}

		// Create video source object
		public IVideoSource CreateVideoSource(object config)
		{
			AxisConfiguration cfg = (AxisConfiguration) config;
			
			if (cfg != null)
			{
				Axis2460 source = new Axis2460();

				source.StreamType	= cfg.stremType;
				source.VideoSource	= cfg.source;
				source.Login		= cfg.login;
				source.Password		= cfg.password;
				source.Camera		= cfg.camera;
				source.FrameInterval = cfg.frameInterval;

				return (IVideoSource) source;
			}
			return null;
		}
	}


	/// <summary>
	/// Axis2100Description
	/// </summary>
	public class Axis2100Description : IVideoSourceDescription
	{
		// Name property
		public string Name
		{
			get { return "Axis 2100 network camera"; }
		}

		// Description property
		public string Description
		{
			get { return "Provides access to Axis 2100 network camera"; }
		}

		// Return settings page
		public IVideoSourcePage GetSettingsPage()
		{
			return new Axis2100SetupPage();
		}

		// Save configuration
		public void SaveConfiguration(XmlTextWriter writer, object config)
		{
			AxisConfiguration cfg = (AxisConfiguration) config;

			if (cfg != null)
			{
				writer.WriteAttributeString("source", cfg.source);
				writer.WriteAttributeString("login", cfg.login);
				writer.WriteAttributeString("password", cfg.password);
				writer.WriteAttributeString("size", cfg.resolution);
				writer.WriteAttributeString("stype", ((int) cfg.stremType).ToString());
				writer.WriteAttributeString("interval", cfg.frameInterval.ToString());
			}
		}

		// Load configuration
		public object LoadConfiguration(XmlTextReader reader)
		{
			AxisConfiguration	config = new AxisConfiguration();

			try
			{
				config.source	= reader.GetAttribute("source");
				config.login	= reader.GetAttribute("login");
				config.password	= reader.GetAttribute("password");
				config.resolution = reader.GetAttribute("size");
				config.stremType = (StreamType) (int.Parse(reader.GetAttribute("stype")));
				config.frameInterval = int.Parse(reader.GetAttribute("interval"));
			}
			catch (Exception)
			{
			}
			return (object) config;
		}

		// Create video source object
		public IVideoSource CreateVideoSource(object config)
		{
			AxisConfiguration cfg = (AxisConfiguration) config;
			
			if (cfg != null)
			{
				Axis2100 source = new Axis2100();

				source.StreamType	= cfg.stremType;
				source.VideoSource	= cfg.source;
				source.Login		= cfg.login;
				source.Password		= cfg.password;
				source.Resolution	= cfg.resolution;
				source.FrameInterval = cfg.frameInterval;

				return (IVideoSource) source;
			}
			return null;
		}
	}


	/// <summary>
	/// Axis2110Description
	/// </summary>
	public class Axis2110Description : IVideoSourceDescription
	{
		// Name property
		public string Name
		{
			get { return "Axis 2110 network camera"; }
		}

		// Description property
		public string Description
		{
			get { return "Provides access to Axis 2110 network camera"; }
		}

		// Return settings page
		public IVideoSourcePage GetSettingsPage()
		{
			return new Axis2110SetupPage();
		}

		// Save configuration
		public void SaveConfiguration(XmlTextWriter writer, object config)
		{
			AxisConfiguration cfg = (AxisConfiguration) config;

			if (cfg != null)
			{
				writer.WriteAttributeString("source", cfg.source);
				writer.WriteAttributeString("login", cfg.login);
				writer.WriteAttributeString("password", cfg.password);
				writer.WriteAttributeString("size", cfg.resolution);
				writer.WriteAttributeString("stype", ((int) cfg.stremType).ToString());
				writer.WriteAttributeString("interval", cfg.frameInterval.ToString());
			}
		}

		// Load configuration
		public object LoadConfiguration(XmlTextReader reader)
		{
			AxisConfiguration	config = new AxisConfiguration();

			try
			{
				config.source	= reader.GetAttribute("source");
				config.login	= reader.GetAttribute("login");
				config.password	= reader.GetAttribute("password");
				config.resolution = reader.GetAttribute("size");
				config.stremType = (StreamType) (int.Parse(reader.GetAttribute("stype")));
				config.frameInterval = int.Parse(reader.GetAttribute("interval"));
			}
			catch (Exception)
			{
			}
			return (object) config;
		}

		// Create video source object
		public IVideoSource CreateVideoSource(object config)
		{
			AxisConfiguration cfg = (AxisConfiguration) config;
			
			if (cfg != null)
			{
				Axis2110 source = new Axis2110();

				source.StreamType	= cfg.stremType;
				source.VideoSource	= cfg.source;
				source.Login		= cfg.login;
				source.Password		= cfg.password;
				source.Resolution	= cfg.resolution;
				source.FrameInterval = cfg.frameInterval;

				return (IVideoSource) source;
			}
			return null;
		}
	}

	
	/// <summary>
	/// Axis2120Description
	/// </summary>
	public class Axis2120Description : IVideoSourceDescription
	{
		// Name property
		public string Name
		{
			get { return "Axis 2120 network camera"; }
		}

		// Description property
		public string Description
		{
			get { return "Provides access to Axis 2120 network camera"; }
		}

		// Return settings page
		public IVideoSourcePage GetSettingsPage()
		{
			return new Axis2120SetupPage();
		}

		// Save configuration
		public void SaveConfiguration(XmlTextWriter writer, object config)
		{
			AxisConfiguration cfg = (AxisConfiguration) config;

			if (cfg != null)
			{
				writer.WriteAttributeString("source", cfg.source);
				writer.WriteAttributeString("login", cfg.login);
				writer.WriteAttributeString("password", cfg.password);
				writer.WriteAttributeString("size", cfg.resolution);
				writer.WriteAttributeString("stype", ((int) cfg.stremType).ToString());
				writer.WriteAttributeString("interval", cfg.frameInterval.ToString());
			}
		}

		// Load configuration
		public object LoadConfiguration(XmlTextReader reader)
		{
			AxisConfiguration	config = new AxisConfiguration();

			try
			{
				config.source	= reader.GetAttribute("source");
				config.login	= reader.GetAttribute("login");
				config.password	= reader.GetAttribute("password");
				config.resolution = reader.GetAttribute("size");
				config.stremType = (StreamType) (int.Parse(reader.GetAttribute("stype")));
				config.frameInterval = int.Parse(reader.GetAttribute("interval"));
			}
			catch (Exception)
			{
			}
			return (object) config;
		}

		// Create video source object
		public IVideoSource CreateVideoSource(object config)
		{
			AxisConfiguration cfg = (AxisConfiguration) config;
			
			if (cfg != null)
			{
				Axis2120 source = new Axis2120();

				source.StreamType	= cfg.stremType;
				source.VideoSource	= cfg.source;
				source.Login		= cfg.login;
				source.Password		= cfg.password;
				source.Resolution	= cfg.resolution;
				source.FrameInterval = cfg.frameInterval;

				return (IVideoSource) source;
			}
			return null;
		}
	}

	
	/// <summary>
	/// Axis2130RDescription
	/// </summary>
	public class Axis2130RDescription : IVideoSourceDescription
	{
		// Name property
		public string Name
		{
			get { return "Axis 2130/2130R network camera"; }
		}

		// Description property
		public string Description
		{
			get { return "Provides access to Axis 2130 network camera"; }
		}

		// Return settings page
		public IVideoSourcePage GetSettingsPage()
		{
			return new Axis2130RSetupPage();
		}

		// Save configuration
		public void SaveConfiguration(XmlTextWriter writer, object config)
		{
			AxisConfiguration cfg = (AxisConfiguration) config;

			if (cfg != null)
			{
				writer.WriteAttributeString("source", cfg.source);
				writer.WriteAttributeString("login", cfg.login);
				writer.WriteAttributeString("password", cfg.password);
				writer.WriteAttributeString("size", cfg.resolution);
				writer.WriteAttributeString("stype", ((int) cfg.stremType).ToString());
				writer.WriteAttributeString("interval", cfg.frameInterval.ToString());
			}
		}

		// Load configuration
		public object LoadConfiguration(XmlTextReader reader)
		{
			AxisConfiguration	config = new AxisConfiguration();

			try
			{
				config.source	= reader.GetAttribute("source");
				config.login	= reader.GetAttribute("login");
				config.password	= reader.GetAttribute("password");
				config.resolution = reader.GetAttribute("size");
				config.stremType = (StreamType) (int.Parse(reader.GetAttribute("stype")));
				config.frameInterval = int.Parse(reader.GetAttribute("interval"));
			}
			catch (Exception)
			{
			}
			return (object) config;
		}

		// Create video source object
		public IVideoSource CreateVideoSource(object config)
		{
			AxisConfiguration cfg = (AxisConfiguration) config;
			
			if (cfg != null)
			{
				Axis2130R source = new Axis2130R();

				source.StreamType	= cfg.stremType;
				source.VideoSource	= cfg.source;
				source.Login		= cfg.login;
				source.Password		= cfg.password;
				source.Resolution	= cfg.resolution;
				source.FrameInterval = cfg.frameInterval;

				return (IVideoSource) source;
			}
			return null;
		}
	}


	/// <summary>
	/// Axis205Description
	/// </summary>
	public class Axis205Description : IVideoSourceDescription
	{
		// Name property
		public string Name
		{
			get { return "Axis 205 network camera"; }
		}

		// Description property
		public string Description
		{
			get { return "Provides access to Axis 205 network camera"; }
		}

		// Return settings page
		public IVideoSourcePage GetSettingsPage()
		{
			return new Axis205SetupPage();
		}

		// Save configuration
		public void SaveConfiguration(XmlTextWriter writer, object config)
		{
			AxisConfiguration cfg = (AxisConfiguration) config;

			if (cfg != null)
			{
				writer.WriteAttributeString("source", cfg.source);
				writer.WriteAttributeString("login", cfg.login);
				writer.WriteAttributeString("password", cfg.password);
				writer.WriteAttributeString("size", cfg.resolution);
				writer.WriteAttributeString("stype", ((int) cfg.stremType).ToString());
				writer.WriteAttributeString("interval", cfg.frameInterval.ToString());
			}
		}

		// Load configuration
		public object LoadConfiguration(XmlTextReader reader)
		{
			AxisConfiguration	config = new AxisConfiguration();

			try
			{
				config.source	= reader.GetAttribute("source");
				config.login	= reader.GetAttribute("login");
				config.password	= reader.GetAttribute("password");
				config.resolution = reader.GetAttribute("size");
				config.stremType = (StreamType) (int.Parse(reader.GetAttribute("stype")));
				config.frameInterval = int.Parse(reader.GetAttribute("interval"));
			}
			catch (Exception)
			{
			}
			return (object) config;
		}

		// Create video source object
		public IVideoSource CreateVideoSource(object config)
		{
			AxisConfiguration cfg = (AxisConfiguration) config;
			
			if (cfg != null)
			{
				Axis205 source = new Axis205();

				source.StreamType	= cfg.stremType;
				source.VideoSource	= cfg.source;
				source.Login		= cfg.login;
				source.Password		= cfg.password;
				source.Resolution	= cfg.resolution;
				source.FrameInterval = cfg.frameInterval;

				return (IVideoSource) source;
			}
			return null;
		}
	}


	/// <summary>
	/// Axis206Description
	/// </summary>
	public class Axis206Description : IVideoSourceDescription
	{
		// Name property
		public string Name
		{
			get { return "Axis 206 network camera"; }
		}

		// Description property
		public string Description
		{
			get { return "Provides access to Axis 206 network camera"; }
		}

		// Return settings page
		public IVideoSourcePage GetSettingsPage()
		{
			return new Axis206SetupPage();
		}

		// Save configuration
		public void SaveConfiguration(XmlTextWriter writer, object config)
		{
			AxisConfiguration cfg = (AxisConfiguration) config;

			if (cfg != null)
			{
				writer.WriteAttributeString("source", cfg.source);
				writer.WriteAttributeString("login", cfg.login);
				writer.WriteAttributeString("password", cfg.password);
				writer.WriteAttributeString("size", cfg.resolution);
				writer.WriteAttributeString("stype", ((int) cfg.stremType).ToString());
				writer.WriteAttributeString("interval", cfg.frameInterval.ToString());
			}
		}

		// Load configuration
		public object LoadConfiguration(XmlTextReader reader)
		{
			AxisConfiguration	config = new AxisConfiguration();

			try
			{
				config.source	= reader.GetAttribute("source");
				config.login	= reader.GetAttribute("login");
				config.password	= reader.GetAttribute("password");
				config.resolution = reader.GetAttribute("size");
				config.stremType = (StreamType) (int.Parse(reader.GetAttribute("stype")));
				config.frameInterval = int.Parse(reader.GetAttribute("interval"));
			}
			catch (Exception)
			{
			}
			return (object) config;
		}

		// Create video source object
		public IVideoSource CreateVideoSource(object config)
		{
			AxisConfiguration cfg = (AxisConfiguration) config;
			
			if (cfg != null)
			{
				Axis206 source = new Axis206();

				source.StreamType	= cfg.stremType;
				source.VideoSource	= cfg.source;
				source.Login		= cfg.login;
				source.Password		= cfg.password;
				source.Resolution	= cfg.resolution;
				source.FrameInterval = cfg.frameInterval;

				return (IVideoSource) source;
			}
			return null;
		}
	}


	/// <summary>
	/// Axis2400Description
	/// </summary>
	public class Axis2400Description : IVideoSourceDescription
	{
		// Name property
		public string Name
		{
			get { return "Axis 2400+ video server"; }
		}

		// Description property
		public string Description
		{
			get { return "Provides access to Axis 2400+ video server"; }
		}

		// Return settings page
		public IVideoSourcePage GetSettingsPage()
		{
			return new Axis2400SetupPage();
		}

		// Save configuration
		public void SaveConfiguration(XmlTextWriter writer, object config)
		{
			AxisConfiguration cfg = (AxisConfiguration) config;

			if (cfg != null)
			{
				writer.WriteAttributeString("source", cfg.source);
				writer.WriteAttributeString("login", cfg.login);
				writer.WriteAttributeString("password", cfg.password);
				writer.WriteAttributeString("camera", cfg.camera.ToString());
				writer.WriteAttributeString("stype", ((int) cfg.stremType).ToString());
				writer.WriteAttributeString("interval", cfg.frameInterval.ToString());
			}
		}

		// Load configuration
		public object LoadConfiguration(XmlTextReader reader)
		{
			AxisConfiguration	config = new AxisConfiguration();

			try
			{
				config.source	= reader.GetAttribute("source");
				config.login	= reader.GetAttribute("login");
				config.password	= reader.GetAttribute("password");
				config.camera	= short.Parse(reader.GetAttribute("camera"));
				config.stremType = (StreamType) (int.Parse(reader.GetAttribute("stype")));
				config.frameInterval = int.Parse(reader.GetAttribute("interval"));
			}
			catch (Exception)
			{
			}
			return (object) config;
		}

		// Create video source object
		public IVideoSource CreateVideoSource(object config)
		{
			AxisConfiguration cfg = (AxisConfiguration) config;
			
			if (cfg != null)
			{
				Axis2400 source = new Axis2400();

				source.StreamType	= cfg.stremType;
				source.VideoSource	= cfg.source;
				source.Login		= cfg.login;
				source.Password		= cfg.password;
				source.Camera		= cfg.camera;
				source.FrameInterval = cfg.frameInterval;

				return (IVideoSource) source;
			}
			return null;
		}
	}


	/// <summary>
	/// Axis2401Description
	/// </summary>
	public class Axis2401Description : IVideoSourceDescription
	{
		// Name property
		public string Name
		{
			get { return "Axis 2401+ video server"; }
		}

		// Description property
		public string Description
		{
			get { return "Provides access to Axis 2401+ video server"; }
		}

		// Return settings page
		public IVideoSourcePage GetSettingsPage()
		{
			return new Axis2401SetupPage();
		}

		// Save configuration
		public void SaveConfiguration(XmlTextWriter writer, object config)
		{
			AxisConfiguration cfg = (AxisConfiguration) config;

			if (cfg != null)
			{
				writer.WriteAttributeString("source", cfg.source);
				writer.WriteAttributeString("login", cfg.login);
				writer.WriteAttributeString("password", cfg.password);
				writer.WriteAttributeString("stype", ((int) cfg.stremType).ToString());
				writer.WriteAttributeString("interval", cfg.frameInterval.ToString());
			}
		}

		// Load configuration
		public object LoadConfiguration(XmlTextReader reader)
		{
			AxisConfiguration	config = new AxisConfiguration();

			try
			{
				config.source	= reader.GetAttribute("source");
				config.login	= reader.GetAttribute("login");
				config.password	= reader.GetAttribute("password");
				config.stremType = (StreamType) (int.Parse(reader.GetAttribute("stype")));
				config.frameInterval = int.Parse(reader.GetAttribute("interval"));
			}
			catch (Exception)
			{
			}
			return (object) config;
		}

		// Create video source object
		public IVideoSource CreateVideoSource(object config)
		{
			AxisConfiguration cfg = (AxisConfiguration) config;
			
			if (cfg != null)
			{
				Axis2401 source = new Axis2401();

				source.StreamType	= cfg.stremType;
				source.VideoSource	= cfg.source;
				source.Login		= cfg.login;
				source.Password		= cfg.password;
				source.FrameInterval = cfg.frameInterval;

				return (IVideoSource) source;
			}
			return null;
		}
	}
}
