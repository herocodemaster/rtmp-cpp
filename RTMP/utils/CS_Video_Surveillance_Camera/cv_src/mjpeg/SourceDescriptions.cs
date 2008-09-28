// Camara Vision
//
// Copyright © Andrew Kirillov, 2005-2006
// andrew.kirillov@gmail.com
//

namespace mjpeg
{
	using System;
	using System.Xml;
	using videosource;

	/// <summary>
	/// MJPEGSourceDescription
	/// </summary>
	public class MJPEGSourceDescription : IVideoSourceDescription
	{
		// Name property
		public string Name
		{
			get { return "MJPEG stream"; }
		}

		// Description property
		public string Description
		{
			get { return "Retrieve Motion JPEG stream from the specified URL"; }
		}

		// Return settings page
		public IVideoSourcePage GetSettingsPage()
		{
			return new MJPEGSourcePage();
		}

		// Save configuration
		public void SaveConfiguration(XmlTextWriter writer, object config)
		{
			MJPEGConfiguration	cfg = (MJPEGConfiguration) config;

			if (cfg != null)
			{
				writer.WriteAttributeString("source", cfg.source);
				writer.WriteAttributeString("login", cfg.login);
				writer.WriteAttributeString("password", cfg.password);
			}
		}

		// Load configuration
		public object LoadConfiguration(XmlTextReader reader)
		{
			MJPEGConfiguration	config = new MJPEGConfiguration();

			try
			{
				config.source	= reader.GetAttribute("source");
				config.login	= reader.GetAttribute("login");
				config.password	= reader.GetAttribute("password");
			}
			catch (Exception)
			{
			}

			return (object) config;
		}

		// Create video source object
		public IVideoSource CreateVideoSource(object config)
		{
			MJPEGConfiguration cfg = (MJPEGConfiguration) config;
			
			if (cfg != null)
			{
				MJPEGSource source = new MJPEGSource();

				source.VideoSource	= cfg.source;
				source.Login		= cfg.login;
				source.Password		= cfg.password;

				return (IVideoSource) source;
			}
			return null;
		}
	}
}
