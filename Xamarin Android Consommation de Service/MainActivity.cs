using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using ServiceStack.ServiceClient.Web;
using ProteinTrackerMVC.Api;
using System.Collections.Generic;
using System.Linq;

namespace ProteinTrackerAndroid
{
	[Activity (Label = "Protein Tracker", MainLauncher = true)]
	public class MainActivity : Activity
	{
		private JsonServiceClient client;
		private IList<User> users;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			client = new JsonServiceClient ("http://mario:56381/api");

			PopulateSelectUsers ();

			Spinner usersSpinner = FindViewById<Spinner> (Resource.Id.usersSpinner);
			usersSpinner.ItemSelected += (object sender, AdapterView.ItemSelectedEventArgs e) => {
				TextView goalTextView = FindViewById<TextView> (Resource.Id.userGoalTextView);
				TextView totalTextView = FindViewById<TextView> (Resource.Id.userTotalTextView);

				var selectedUser = users[e.Position];
				goalTextView.Text = selectedUser.Goal.ToString();
				totalTextView.Text = selectedUser.Total.ToString ();
			};

			var addUserButton = FindViewById<Button> (Resource.Id.addNewUserButton);
			addUserButton.Click += (object sender, EventArgs e) => {
				TextView goalTextView = FindViewById<TextView>(Resource.Id.goalTextView);
				TextView nameTextView = FindViewById<TextView>(Resource.Id.nameTextView);
				var goal = int.Parse (goalTextView.Text);

				var response = client.Send (new AddUser { Goal = goal, Name = nameTextView.Text });
				PopulateSelectUsers();

				goalTextView.Text = string.Empty;
				nameTextView.Text = string.Empty;

				Toast.MakeText (this, "Added new user", ToastLength.Short).Show ();
			};

			var addProteinButton = FindViewById<Button> (Resource.Id.addProteinButton);
			addProteinButton.Click += (object sender, EventArgs e) => {
				TextView amountTextView = FindViewById<TextView> (Resource.Id.amountTextView);
				var amount = int.Parse(amountTextView.Text);
				var selectedUser = users[usersSpinner.SelectedItemPosition];

				var response = client.Send (new AddProtein { Amount = amount, UserId = selectedUser.Id });
				selectedUser.Total = response.NewTotal;

				TextView totalTextView = FindViewById<TextView> (Resource.Id.userTotalTextView);
				totalTextView.Text = selectedUser.Total.ToString ();
				amountTextView.Text = string.Empty;
			};
		}

		void PopulateSelectUsers ()
		{
			var response = client.Get (new Users());
			users = response.Users.ToList ();

			var names = users.Select (u => u.Name);

			var usersSpinner = FindViewById<Spinner> (Resource.Id.usersSpinner);
			usersSpinner.Adapter = new ArrayAdapter<string> (this, Android.Resource.Layout.SimpleListItem1, names.ToArray ());
		}
	}
}


