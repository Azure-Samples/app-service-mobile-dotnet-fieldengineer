using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using FieldEngineerLite.Views;
using FieldEngineerLite;

#if __IOS__
using UIContext = UIKit.UIViewController;
#elif __ANDROID__
using UIContext = global::Android.Content.Context;
#endif

namespace FieldEngineerLite
{
    public class App : Application
    {
        public static UIContext UIContext { get; set; }
        
        public App()
        {
            MainPage = new JobMasterDetailPage ();
        }
    }

    public static class AppStyle
    {
        public static NamedSize DefaultFontSize =  Device.OnPlatform(
            iOS: NamedSize.Small,
            Android: NamedSize.Medium,
            WinPhone: NamedSize.Medium
        );

        public static Color DefaultActionColor = Device.OnPlatform(
            iOS: Color.Blue,
            Android: Color.Accent,
            WinPhone: Color.Accent
        );
            
        public static readonly Font DefaultFont = Device.OnPlatform(
            iOS: Font.OfSize("Avenir", DefaultFontSize),
            Android: Font.SystemFontOfSize(DefaultFontSize),
            WinPhone: Font.SystemFontOfSize(DefaultFontSize)
        );			
    }
}
