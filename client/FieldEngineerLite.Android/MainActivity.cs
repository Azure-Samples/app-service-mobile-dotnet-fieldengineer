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
    [Activity(Label = "FieldEngineerLite.Droid",
        Icon = "@drawable/icon",
        MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
        Theme = "@android:style/Theme.Holo.Light")]
    public class MainActivity : FormsApplicationActivity
    {
        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Xamarin.Forms.Forms.Init(this, bundle);
            Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();

            await App.JobService.InitializeAsync();

            App.UIContext = this;

            var app = new App();
            LoadApplication(app);
        }
    }
}

