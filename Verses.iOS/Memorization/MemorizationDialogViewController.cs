using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using Verses.Core;

namespace Verses.iOS
{
	public class MemorizationDialogViewController : DialogViewController
	{
		UIBarButtonItem BackButton;
		MemorizationCategory memorizationCategory;
		List<Verse> data;
		UILabel NavigationBarLabel;

		public MemorizationDialogViewController (List<Verse> verses, MemorizationCategory category) : base (null, false)
		{
			data = verses;
			memorizationCategory = category;
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			SetupUI ();
			BuildRootTree ();
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);

			NavigationBarLabel.RemoveFromSuperview ();
		}

		public override void LoadView ()
		{
			base.LoadView ();

			SetupUI ();
			BuildRootTree ();
		}

		public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations ()
		{
			return UIInterfaceOrientationMask.Portrait;
		}

		void SetupUI ()
		{
			NavigationController.NavigationBar.SetBackgroundImage (Images.BlankBar, UIBarMetrics.Default);
			var title = memorizationCategory.ToString ().ToUpper ();
			NavigationBarLabel = InterfaceHelper.LabelForTitle (title); 
			NavigationItem.TitleView = NavigationBarLabel;

			var backButton = new UIButton (new RectangleF (0, 0, 25, 25));
			backButton.SetBackgroundImage (Images.BackButton, UIControlState.Normal);
			backButton.SetBackgroundImage (Images.BackButtonHighlighted, UIControlState.Highlighted);
			backButton.AddTarget((sender, args) => NavigationController.PopViewControllerAnimated (true), 
			                     UIControlEvent.TouchUpInside);

			BackButton = new UIBarButtonItem (backButton);
			NavigationItem.LeftBarButtonItem = BackButton;

			TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
			TableView.SeparatorColor = UIColor.Clear;
			TableView.BackgroundView = null;
			TableView.BackgroundColor = UIColor.FromPatternImage (Images.TableViewBackground);
		}

		Section BuildMemorizationsSection ()
		{
			return new Section ("") 
			{
				data.Where (verse => verse.Memorizable && verse.Category == memorizationCategory)
					.Select (verse => BuildMemorizationElement (verse))
			};
		}

		void BuildRootTree ()
		{
			var root = new RootElement ("") {
			    BuildMemorizationsSection()
			};
		    if (root [0].Count == 0 && memorizationCategory == MemorizationCategory.Review) {
				root.Add (new Section {
					new EmptyReviewSectionElement ()
				});
			} else if (root [0].Count == 0 && memorizationCategory != MemorizationCategory.Review) {
				root.Add (new Section {
					new EmptySectionElement ()
				});
			} else {
				AddActionButtonToRoot (root);
			}
			Root = root;
		}

		Element BuildMemorizationElement (Verse v)
		{
			var element = new MemorizationElement (v);

			return element;
		}

		void AddActionButtonToRoot (RootElement root)
		{
			switch (memorizationCategory) 
			{
				case MemorizationCategory.Sunday:
				case MemorizationCategory.Monday:
				case MemorizationCategory.Tuesday:
				case MemorizationCategory.Wednesday:
				case MemorizationCategory.Thursday:
				case MemorizationCategory.Friday:
				case MemorizationCategory.Saturday:
					root.Add (new Section { new MemorizeButtonElement (), new MoveButtonElement ()});
					break;
				case MemorizationCategory.Queue:
					root.Add (new Section { new MoveButtonElement () });
					break;
				case MemorizationCategory.Review:
					root.Add (new Section { new ReviewButtonElement (), new MoveButtonElement () });
					break;
			}
		}
	}
}

