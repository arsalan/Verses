using System;
using System.Drawing;
using MonoTouch.UIKit;

namespace Verses.iOS
{
	public class VersesViewController : UIViewController
	{
		UIBarButtonItem ComposeButton, SettingsButton;
		UILabel NavigationBarLabel;

		public UITableView VersesTable { get; set; }

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			NavigationController.NavigationBar.SetBackgroundImage (Images.BlankBar, UIBarMetrics.Default);
			SetupNavigationBar ();

			NavigationBarLabel = InterfaceHelper.LabelForTitle ("VERSES");
			NavigationItem.TitleView = NavigationBarLabel;
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);

			NavigationController.NavigationBar.SetBackgroundImage (Images.BlankBar, UIBarMetrics.Default);
			SetupNavigationBar ();

			VersesTable.Source = new VersesTableSource (this);
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			SetupNavigationBar ();
			SetupUI ();

			VersesTable.Source = new VersesTableSource (this);
		}

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);

			NavigationBarLabel.RemoveFromSuperview ();
		}

		void SetupNavigationBar ()
		{
			var composeButton = new UIButton (new RectangleF (0, 0, 25, 25));
			composeButton.SetBackgroundImage (Images.ComposeButton, UIControlState.Normal);
			composeButton.SetBackgroundImage (Images.ComposeButtonHighlighted, UIControlState.Highlighted);
			composeButton.AddTarget (HandleComposeButtonTapped, UIControlEvent.TouchUpInside);

			ComposeButton = new UIBarButtonItem (composeButton);
			NavigationItem.RightBarButtonItem = ComposeButton;

			var settingsButton = new UIButton (new RectangleF (0, 0, 25, 25));
			settingsButton.SetBackgroundImage (Images.SettingsButton, UIControlState.Normal);
			settingsButton.SetBackgroundImage (Images.SettingsButtonHighlighted, UIControlState.Highlighted);
			settingsButton.AddTarget (HandleSettingsButtonTapped, UIControlEvent.TouchUpInside);

			SettingsButton = new UIBarButtonItem (settingsButton);
			NavigationItem.LeftBarButtonItem = SettingsButton;

			NavigationController.NavigationBar.BarStyle = UIBarStyle.Black;
		}

		void SetupUI ()
		{
			VersesTable = new UITableView {
				Frame = new RectangleF (0, 0, View.Bounds.Width, View.Bounds.Height - 69),
				SectionIndexMinimumDisplayRowCount = 25,
				SeparatorInset = UIEdgeInsets.Zero,
				SeparatorStyle = UITableViewCellSeparatorStyle.None,
				Source = new VersesTableSource (this)
			};

			SetNeedsStatusBarAppearanceUpdate ();

			View.AddSubview (VersesTable);
		}

		void HandleComposeButtonTapped (object sender, EventArgs args)
		{
			PresentViewController (new UINavigationController(new VerseComposeDialog (this)), true, null);
		}

		void HandleSettingsButtonTapped (object sender, EventArgs args)
		{
			PresentViewController (new UINavigationController(new SettingsDialog ()), true, null);
		}
	}
}