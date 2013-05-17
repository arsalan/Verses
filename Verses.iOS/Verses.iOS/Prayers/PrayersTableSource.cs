using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Verses.Core;

namespace Verses.iOS
{
	public class PrayersTableSource : UITableViewSource
	{
		string[] keys;
		UIViewController managingController;
		NSString PRAYER_CELL;
		Dictionary<string, List<Prayer>> prayers;

		public PrayersTableSource (UIViewController managingController)
		{
			PRAYER_CELL = new NSString ("PRAYER_CELL");
			prayers = new Dictionary<string, List<Prayer>> ();
			this.managingController = managingController;

			var path = DatabaseHelper.GetDatabasePath ("verses.db3");
			var tempPrayers = new List<Prayer> ();

			// Read in prayers from database
			using (DatabaseUtility database = new DatabaseUtility (path)) {
				tempPrayers = database.GetPrayers ();
				tempPrayers.Sort ();
			}

			// Process prayers
			foreach (var prayer in tempPrayers)
			{
				ProcessKey (prayer);
			}
			
			keys = prayers.Keys.ToArray ();
		}

		public override int NumberOfSections(UITableView tableView)
		{
			return keys.Length;
		}
		
		public override int RowsInSection (UITableView tableview, int section)
		{
			return prayers[keys[section]].Count;
		}
		
		public override float GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
		{
			return 107f;
		}

		/*
		public override float GetHeightForHeader(UITableView tableView, int section)
		{
			return 65;
		}

		public override UIView GetViewForHeader(UITableView tableView, int section)
		{
			var letter = keys[section];

			var header = new UIView ()
			{
				BackgroundColor = UIColor.FromPatternImage (Images.TableViewBackground),
				Frame = new RectangleF (0, 0, 320, 65)
			};

			var image = new UIImageView ()
			{
				BackgroundColor = UIColor.Clear,
				Image = UIImage.FromFile ("Images/General/55x55.png"),
				Frame = new RectangleF (5, 5, 55, 55)
			};

			var text = new UILabel ()
			{
				BackgroundColor = UIColor.Clear,
				Font = UIFont.FromName("MuseoSans-500", 15f),
				Frame = new RectangleF (27f, 25, 15, 15),
				Text = letter,
				TextColor = UIColor.White
			};

			header.AddSubview (image);
			header.AddSubview (text);

			return header;
		}*/
	
		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			Console.WriteLine ("Row tapped!");
		}

		public override string[] SectionIndexTitles (UITableView tableView)
		{
			return prayers.Keys.ToArray ();
		}

		/*
		public override void CommitEditingStyle (UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
		{
			var prayer = prayers[keys[indexPath.Section]][indexPath.Row];			
			var path = DatabaseHelper.GetDatabasePath("verses.db3");

			var alert = new UIAlertView ("Delete Prayer", "Are you sure you want to delete?",
			                             null, "No", "Yes");
			alert.Show ();
			
			if (editingStyle == UITableViewCellEditingStyle.Delete) 
			{
				using (var database = new DatabaseUtility(path))
				{
					database.DeletePrayer(prayer);
				}
				
				prayers[keys[indexPath.Section]].RemoveAt(indexPath.Row);
				tableView.DeleteRows(new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Fade);
			}   
		}*/

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell (PRAYER_CELL) as PrayerCell;
			var rowData = prayers[keys[indexPath.Section]][indexPath.Row];
			
			if (cell == null)
				cell = new PrayerCell (PRAYER_CELL);
			
			cell.PopulateCell (rowData);
			
			return cell;
		}

		public void ProcessKey (Prayer prayer)
		{
			var key = prayer.Title[0].ToString ();
			
			if (prayers.ContainsKey (key))
				prayers[key].Add (prayer);
			else
				prayers.Add (key, new List<Prayer> () { prayer });
		}
	}
}