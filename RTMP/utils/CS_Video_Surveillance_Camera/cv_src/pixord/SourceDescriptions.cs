// Camara Vision
//
// Copyright © Andrew Kirillov, 2005-2006
// andrew.kirillov@gmail.com
//

namespace pixord
{
	using System;
	using System.Xml;
	using videosource;

	/// <summary>
	/// PixordCameraDescription
	/// </summary>
	public class PixordCameraDescription : IVideoSourceDescription
	{
		// Name property
		public string Name
		{
			get { return "PiXORD network camera"; }
		}

		// Description property
		public string Description
		{
			get { return "Provides access to PiXORD network cameras"; }
		}

		// Return settings page
		public IVideoSourcePage GetSettingsPage()
		{
			return new PixordCameraSetupPage();
		}

		// Save configuration
		public void SaveConfiguration(XmlTextWriter writer, object config)
		{
			PixordConfiguration cfg = (PixordConfiguration) config;

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
			PixordConfiguration	config = new PixordConfiguration();

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
			PixordConfiguration cfg = (PixordConfiguration) config;
			
			if (cfg != null)
			{
				PixordCamera source = new PixordCamera();

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
}