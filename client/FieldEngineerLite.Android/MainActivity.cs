using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Xamarin.Forms.Platform.Android;

namespace FieldEngineerLite.Droid
{
    [Activity(Label = "FieldEngineerLite", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : AndroidActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Xamarin.Forms.Forms.Init(this, bundle);
            App.JobService.InitializeAsync().Wait();

            App.UIContext = this;
            SetPage(App.GetMainPage());
        }
    }
}

