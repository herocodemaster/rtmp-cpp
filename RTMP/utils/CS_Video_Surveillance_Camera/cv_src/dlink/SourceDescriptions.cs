// Camara Vision
//
// Copyright © Andrew Kirillov, 2005-2006
// andrew.kirillov@gmail.com
//

namespace dlink
{
	using System;
	using System.Xml;
	using videosource;

	/// <summary>
	/// DLinkCameraDescription
	/// </summary>
	public class DLinkCameraDescription : IVideoSourceDescription
	{
		// Name property
		public string Name
		{
			get { return "DLink network camera"; }
		}

		// Description property
		public string Description
		{
			get { return "Provides access to DLink network cameras"; }
		}

		// Return settings page
		public IVideoSourcePage GetSettingsPage()
		{
			return new DLinkCameraSetupPage();
		}

		// Save configuration
		public void SaveConfiguration(XmlTextWriter writer, object config)
		{
			DLinkConfiguration cfg = (DLinkConfiguration) config;

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
			DLinkConfiguration config = new DLinkConfiguration();

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
			DLinkConfiguration cfg = (DLinkConfiguration) config;
			
			if (cfg != null)
			{
				DLinkCamera source = new DLinkCamera();

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