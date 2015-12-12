using Xamarin.Forms;
using FieldEngineerLite.Helpers;
using FieldEngineerLite.Models;

namespace FieldEngineerLite.Views
{
    public class JobCell : ViewCell
    {
        public JobCell()
        {
            var jobHeader = new JobHeaderView(leftPadding: 5);

            var title = new Label { FontSize =  Device.GetNamedSize(NamedSize.Small, typeof(Label)) };
            title.SetBinding<Job>(Label.TextProperty, job => job.Title);

            var customer = new Label { FontSize =  Device.GetNamedSize(NamedSize.Small, typeof(Label)) };
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
                            new Label {
                                Text = "Customer:",
                                FontSize =  Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                                FontAttributes = FontAttributes.Bold
                            },
                            customer
                        }
                    },
                    new Label {
                        Text = "Description:",
                        FontSize =  Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                        FontAttributes = FontAttributes.Bold
                    },
                    title
                }
            };

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

            this.Height = 120;
            this.View = rootLayout;
        }
    }
}
