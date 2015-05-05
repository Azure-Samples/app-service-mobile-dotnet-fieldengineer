using System;
using System.Collections.Generic;
using System.Text;
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
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.White,
                VerticalOptions = LayoutOptions.Center
            };

            title.SetBinding(Label.TextProperty, new Binding("Key", stringFormat: "Status: {0}"));

            var layout = new StackLayout
            {
                Padding = 5,
                Orientation = StackOrientation.Horizontal,
                Children = { title }
            };
            layout.SetBinding(StackLayout.BackgroundColorProperty, "Key", converter: new JobStatusToColorConverter());

            this.Height = 35;
            this.View = layout;
        }
    }
}
