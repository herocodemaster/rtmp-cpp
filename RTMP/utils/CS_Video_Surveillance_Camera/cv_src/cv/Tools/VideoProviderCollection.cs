// Camera Vision
//
// Copyright © Andrew Kirillov, 2005-2006
// andrew.kirillov@gmail.com
//
namespace CameraViewer
{
	using System;
	using System.Collections;
	using System.IO;
	using System.Reflection;
	using videosource;

	/// <summary>
	/// VideoProviderCollection class - collection of Video Providers
	/// </summary>
	public class VideoProviderCollection : CollectionBase
	{
		// Constructor
		public VideoProviderCollection()
		{
		}

		// Get video provider at the specified index
		public VideoProvider this[int index]
		{
			get
			{
				return ((VideoProvider) InnerList[index]);
			}
		}

		// Get provider by its name
		public VideoProvider GetProviderByName(string name)
		{
			foreach (VideoProvider provider in InnerList)
			{
				if (provider.ProviderName == name)
				{
					return provider;
				}
			}
			return null;
		}

		// Load all video providers
		public void Load(string path)
		{
			// create directory info
			DirectoryInfo	dir = new DirectoryInfo(path);

			// get all dll files from the directory
			FileInfo[]		files = dir.GetFiles("*.dll");

			// walk through all files
			foreach (FileInfo f in files)
			{
				LoadAssembly(Path.Combine(path, f.Name));
			}

			// sort providers list
			InnerList.Sort();
		}

		// Load assembly and find video provider descriptors there
		private void LoadAssembly(string fname)
		{
			Type		typeVideoSourceDesc = typeof(IVideoSourceDescription);
			Assembly	asm = null;

			try
			{
				// try to load assembly
				asm = Assembly.LoadFrom(fname);

				// get types of the assembly
				Type[]	types = asm.GetTypes();

				// check all types
				foreach (Type type in types)
				{
					// get interfaces ot the type
					Type[] interfaces = type.GetInterfaces();

					// check, if the type is inherited from IVideoSourceDescription
					if (Array.IndexOf(interfaces, typeVideoSourceDesc) != -1)
					{
						IVideoSourceDescription	desc = null;

						try
						{
							// create an instance of the type
							desc = (IVideoSourceDescription) Activator.CreateInstance(type);
							// create provider object
							InnerList.Add(new VideoProvider(desc));
						}
						catch (Exception)
						{
							// something failed during instance creatinion
						}
					}
				}
			}
			catch (Exception)
			{
			}
		}
	}
}
