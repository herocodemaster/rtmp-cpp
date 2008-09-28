// Camara Vision
//
// Copyright © Andrew Kirillov, 2005-2006
// andrew.kirillov@gmail.com
//

namespace stardot
{
	using System;
	using System.Xml;
	using videosource;

	/// <summary>
	/// StardotNetCamDescription
	/// </summary>
	public class StardotNetCamDescription : IVideoSourceDescription
	{
		// Name property
		public string Name
		{
			get { return "StarDot NetCam network camera"; }
		}

		// Description property
		public string Description
		{
			get { return "Provides access to StarDot NetCam network cameras"; }
		}

		// Return settings page
		public IVideoSourcePage GetSettingsPage()
		{
			return new StardotNetCamSetupPage();
		}

		// Save configuration
		public void SaveConfiguration(XmlTextWriter writer, object config)
		{
			StardotConfiguration cfg = (StardotConfiguration) config;

			if (cfg != null)
			{
				writer.WriteAttributeString("source", cfg.source);
				writer.WriteAttributeString("login", cfg.login);
				writer.WriteAttributeString("password", cfg.password);
				writer.WriteAttributeString("interval", cfg.frameInterval.ToString());
			}
		}

		// Load configuration
		public object LoadConfiguration(XmlTextReader reader)
		{
			StardotConfiguration config = new StardotConfiguration();

			try
			{
				config.source	= reader.GetAttribute("source");
				config.login	= reader.GetAttribute("login");
				config.password	= reader.GetAttribute("password");
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
			StardotConfiguration cfg = (StardotConfiguration) config;
			
			if (cfg != null)
			{
				StardotNetCam source = new StardotNetCam();

				source.VideoSource	= cfg.source;
				source.Login		= cfg.login;
				source.Password		= cfg.password;
				source.FrameInterval = cfg.frameInterval;

				return (IVideoSource) source;
			}
			return null;
		}
	}


	/// <summary>
	/// StardotExpress6Description
	/// </summary>
	public class StardotExpress6Description : IVideoSourceDescription
	{
		// Name property
		public string Name
		{
			get { return "StarDot Express 6 video server"; }
		}

		// Description property
		public string Description
		{
			get { return "Provides access to StarDot Express 6 video serevr"; }
		}

		// Return settings page
		public IVideoSourcePage GetSettingsPage()
		{
			return new StardotExpress6SetupPage();
		}

		// Save configuration
		public void SaveConfiguration(XmlTextWriter writer, object config)
		{
			StardotConfiguration cfg = (StardotConfiguration) config;

			if (cfg != null)
			{
				writer.WriteAttributeString("source", cfg.source);
				writer.WriteAttributeString("login", cfg.login);
				writer.WriteAttributeString("password", cfg.password);
				writer.WriteAttributeString("interval", cfg.frameInterval.ToString());
				writer.WriteAttributeString("camera", cfg.camera.ToString());
			}
		}

		// Load configuration
		public object LoadConfiguration(XmlTextReader reader)
		{
			StardotConfiguration config = new StardotConfiguration();

			try
			{
				config.source	= reader.GetAttribute("source");
				config.login	= reader.GetAttribute("login");
				config.password	= reader.GetAttribute("password");
				config.frameInterval = int.Parse(reader.GetAttribute("interval"));
				config.camera	= short.Parse(reader.GetAttribute("camera"));
			}
			catch (Exception)
			{
			}
			return (object) config;
		}

		// Create video source object
		public IVideoSource CreateVideoSource(object config)
		{
			StardotConfiguration cfg = (StardotConfiguration) config;
			
			if (cfg != null)
			{
				StardotExpress6 source = new StardotExpress6();

				source.VideoSource	= cfg.source;
				source.Login		= cfg.login;
				source.Password		= cfg.password;
				source.FrameInterval = cfg.frameInterval;
				source.Camera		= cfg.camera;

				return (IVideoSource) source;
			}
			return null;
		}
	}
}