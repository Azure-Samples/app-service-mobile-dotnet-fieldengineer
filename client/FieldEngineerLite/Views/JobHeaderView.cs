using Xamarin.Forms;
using FieldEngineerLite.Helpers;
using FieldEngineerLite.Models;

namespace FieldEngineerLite
{
    public class JobHeaderView : ContentView
    {
        public JobHeaderView(double leftPadding, bool colorBackground = false)
        {
            var eta = new Label();
            eta.VerticalOptions = LayoutOptions.FillAndExpand;
            eta.HorizontalOptions = LayoutOptions.FillAndExpand;
            eta.VerticalTextAlignment = TextAlignment.Start;
            eta.SetBinding<Job>(Label.TextProperty, job => job.StartTime, stringFormat: "Start time: {0}");

            var rootLayout = new StackLayout {
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(leftPadding, 5, 0, 5),
                Children = { eta }
            };

            if (colorBackground) {
                eta.TextColor = Color.White;
                rootLayout.SetBinding<Job>(StackLayout.BackgroundColorProperty, job => job.Status, converter: new JobStatusToColorConverter());
            }

            this.Content = rootLayout;
        }
    }
}

