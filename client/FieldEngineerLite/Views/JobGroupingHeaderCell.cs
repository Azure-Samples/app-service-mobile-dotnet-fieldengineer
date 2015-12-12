using Xamarin.Forms;
using FieldEngineerLite.Helpers;

namespace FieldEngineerLite.Views
{
    public class JobGroupingHeaderCell : ViewCell
    {
        public JobGroupingHeaderCell()
        {
            var title = new Label
            {
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.White,
                VerticalOptions = LayoutOptions.Center
            };

            title.SetBinding(Label.TextProperty, new Binding("Key", stringFormat: "Status: {0}"));

            var layout = new StackLayout
            {
                Padding = new Thickness(5, 5, 5, 0),
                Orientation = StackOrientation.Horizontal,
                Children = { title }
            };

            layout.SetBinding(StackLayout.BackgroundColorProperty, "Key", converter: new JobStatusToColorConverter());

            this.View = layout;
        }
    }
}
