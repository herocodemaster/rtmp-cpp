// Camara Vision
//
// Copyright © Andrew Kirillov, 2005-2006
// andrew.kirillov@gmail.com
//

namespace jpeg
{
	using System;
	using System.Xml;
	using videosource;

	/// <summary>
	/// JPEGSourceDescription
	/// </summary>
	public class JPEGSourceDescription : IVideoSourceDescription
	{
		// Name property
		public string Name
		{
			get { return "JPEG stream"; }
		}

		// Description property
		public string Description
		{
			get { return "Downloads separate JPEG files from the specified URL"; }
		}

		// Return settings page
		public IVideoSourcePage GetSettingsPage()
		{
			return new JPEGSourcePage();
		}

		// Save configuration
		public void SaveConfiguration(XmlTextWriter writer, object config)
		{
			JPEGConfiguration	cfg = (JPEGConfiguration) config;

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
			JPEGConfiguration	config = new JPEGConfiguration();

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
			JPEGConfiguration cfg = (JPEGConfiguration) config;
			
			if (cfg != null)
			{
				JPEGSource source = new JPEGSource();

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
