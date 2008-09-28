namespace CameraViewer
{
	using System;
	using System.Runtime.InteropServices;

	/// <summary>
	/// Some Win32 API functions
	/// </summary>
	public class Win32
	{
		// GetSystemMetrics - retrieve various system metrics and
		// system configuration settings
		[DllImport("user32.dll")]
		public static extern int GetSystemMetrics(
			[MarshalAs(UnmanagedType.I4)] SystemMetrics metric);

		// System metrics
		public enum SystemMetrics
		{
			CXSCREEN	= 0,
			CYSCREEN	= 1,
			CYCAPTION	= 4,
			CYMENU		= 15,
			CXFRAME		= 32,
			CYFRAME		= 33
		}
	}
}
