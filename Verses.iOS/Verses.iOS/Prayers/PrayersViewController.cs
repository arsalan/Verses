using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Verses.Core;
 
namespace Verses.iOS
{
	public class PrayersViewController : UIViewController
	{                
		UIBarButtonItem ComposeButton;
		UITableView PrayersTable;

		public PrayersViewController ()
		{

		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			NavigationController.NavigationBar.SetBackgroundImage (Images.PrayersHeader, UIBarMetrics.Default);

			PrayersTable = new UITableView ()
			{
				Frame = new RectangleF(0, 0, View.Bounds.Width, View.Bounds.Height - 49),
				SectionIndexMinimumDisplayRowCount = 20,
				SeparatorStyle = UITableViewCellSeparatorStyle.None,
				Source = new PrayersTableSource (this),
			};

			var composeButton = new UIButton (new RectangleF (0, 0, 20, 20));
			composeButton.SetBackgroundImage (Images.ComposeButton, UIControlState.Normal);
			composeButton.SetBackgroundImage (Images.ComposeButtonHighlighted, UIControlState.Highlighted);
			composeButton.AddTarget((object sender, EventArgs args) => PresentViewController (new PrayerComposeDialog (), true, null), 
			                        UIControlEvent.TouchUpInside);

			ComposeButton = new UIBarButtonItem (composeButton);

			NavigationItem.RightBarButtonItem = ComposeButton;

			View.AddSubview (PrayersTable);
		}

	}
}
