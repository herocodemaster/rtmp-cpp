// Camera Vision
//
// Copyright © Andrew Kirillov, 2005-2006
// andrew.kirillov@gmail.com
//
namespace CameraViewer
{
	using System;

	/// <summary>
	/// IWizardPage - interface of a wizard page
	/// </summary>
	public interface IWizardPage
	{
		/// <summary>
		/// State changed event - notify wizard if the page is completed
		/// and it can proceed to the next page
		/// </summary>
		event EventHandler StateChanged;

		/// <summary>
		/// Reset event - notify wizard if there are such changes on the
		/// page, which can lead to changing completion status of other pages
		/// </summary>
		event EventHandler Reset;

		/// <summary>
		/// PageName property - short name
		/// </summary>
		string PageName { get; }

		/// <summary>
		/// Description property
		/// </summary>
		string PageDescription { get; }

		/// <summary>
		/// Completed property
		/// true, if the page is completed and wizard can proceed to next page
		/// </summary>
		bool Completed { get; }

		/// <summary>
		/// Display - display the page
		/// Wizard call the method after the page was shown
		/// </summary>
		void Display();

		/// <summary>
		/// Apply - check and update all variables
		/// Return false if something wrong and we want to stay on the page
		/// </summary>
		bool Apply();
	}
}
