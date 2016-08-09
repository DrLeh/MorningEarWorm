using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content;

namespace MorningEarWorm.Mobile.Droid
{
	[Activity (Label = "Morning Ear Worm", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity, INativePages
	{
        public void SearchYT(string searchString)
        {
            Intent intent = new Intent(Intent.ActionSearch);
            intent.SetPackage("com.google.android.youtube");
            intent.PutExtra("query", searchString);
            intent.SetFlags(ActivityFlags.NewTask);
            StartActivity(intent);
        }

        protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			global::Xamarin.Forms.Forms.Init (this, bundle);
			LoadApplication (new MorningEarWorm.Mobile.App (this));

		}
	}
}

