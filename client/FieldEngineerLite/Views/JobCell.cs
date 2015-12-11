using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using FieldEngineerLite.Helpers;
using FieldEngineerLite.Models;

namespace FieldEngineerLite.Views
{
    public class JobCell : ViewCell
    {
        public JobCell()
        {
            var jobHeader = new JobHeaderView();

            var title = new Label { TextColor = Color.White };
            //title.Font = AppStyle.DefaultFont;
            title.SetBinding<Job>(Label.TextProperty, job => job.Title);

            var customer = new Label { TextColor = Color.White };
            //customer.Font = AppStyle.DefaultFont;
            customer.SetBinding<Job>(Label.TextProperty, job => job.CustomerName);            

            var jobDetails = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Padding = 5,
                Children =
                {
                    new StackLayout 
                    {
                        Orientation = StackOrientation.Horizontal,
                        Children = {
                            new Label { Text = "Customer:",
                                FontSize =  Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                                FontAttributes = FontAttributes.Bold,
                                TextColor = Color.White },
                            customer
                        }
                    },
                    new Label {
                        Text = "Description:",
                        FontSize =  Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                        FontAttributes = FontAttributes.Bold, TextColor = Color.White },
                    title
                }
            };
            jobDetails.SetBinding<Job>(
                StackLayout.BackgroundColorProperty, 
                job => job.Status, 
                converter: new JobStatusToColorConverter(useLightTheme: true));

            var rootLayout = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Spacing = 0,
                Children =
                {
                    jobHeader,
                    jobDetails
                }
            };

            this.Height = 130;
            this.View = rootLayout;
        }
    }
}
