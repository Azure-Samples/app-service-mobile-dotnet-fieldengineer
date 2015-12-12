using System.Threading.Tasks;
using Xamarin.Forms;
using FieldEngineerLite.Helpers;
using FieldEngineerLite.Models;
using System.Linq;

namespace FieldEngineerLite.Views
{
    public class JobDetailsPage : ContentPage
    {        
        private JobService jobService;
        
        public JobDetailsPage(JobService service)
        {
            this.jobService = service;

            this.Title = "Appointment Details";

            TableSection mainSection = new TableSection("Customer Details");     
            
            mainSection.Add(new DataElementCell("CustomerName", "Customer"));
            mainSection.Add(new DataElementCell("Title", "Customer Notes"));
            mainSection.Add(new DataElementCell("CustomerAddress", "Address") { Height = 60 });
            mainSection.Add(new DataElementCell("CustomerPhoneNumber", "Telephone"));

            var statusCell = new DataElementCell("Status");
            statusCell.ValueLabel.SetBinding<Job>(Label.TextColorProperty, job => job.Status, converter: new JobStatusToColorConverter());
            mainSection.Add(statusCell);

            var workSection = new TableSection("Work Performed");            
            var workRowTemplate = new DataTemplate(typeof(SwitchCell));            
            workRowTemplate.SetBinding(SwitchCell.TextProperty, "Name");
            workRowTemplate.SetBinding(SwitchCell.OnProperty, "Completed");
            //workRowTemplate.SetValue(TextCell.TextColorProperty, Color.White);

            // I don't have images working on Android yet
            //if (Device.OS == TargetPlatform.iOS) 			
            //	equipmentRowTemplate.SetBinding (ImageCell.ImageSourceProperty, "ThumbImage");

            var workListView = new ListView {
                RowHeight = 50,
                ItemTemplate = workRowTemplate
            };
            workListView.SetBinding<Job>(ListView.ItemsSourceProperty, job => job.Items);

            var workCell = new ViewCell { View = workListView };
       
            workSection.Add(workCell);

            var actionsSection = new TableSection("Actions");
            
            TextCell completeJob = new TextCell { 
                Text = "Mark Completed",
                TextColor = AppStyle.DefaultActionColor
            };   
         
            completeJob.Tapped += async delegate {
                await this.CompleteJobAsync();
            };

            actionsSection.Add(completeJob);
            
            var table = new TableView
            {
                Intent = TableIntent.Form,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HasUnevenRows = true,
                Root = new TableRoot("Root")
                {
                    mainSection, workSection, actionsSection, 
                }
            };
            
            this.Content = new ScrollView {
                Orientation = ScrollOrientation.Vertical,
                Content = new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    Children = { new JobHeaderView(leftPadding: 10, colorBackground: true), table }
                }
            };

            this.BindingContextChanged += delegate
            {
                if (SelectedJob != null && SelectedJob.Items != null)
                    workCell.Height = SelectedJob.Items.Count * workListView.RowHeight;
            };
        }

        private async Task CompleteJobAsync()
        {
            var job = this.SelectedJob;

            job.WorkPerformed = string.Join(";", job.Items.Where(i => i.Completed).Select(i => i.Name));

            await jobService.CompleteJobAsync(job);

            // Force a refresh
            this.BindingContext = null;
            this.BindingContext = job;
        }

        private Job SelectedJob
        {
            get { return this.BindingContext as Job; }
        }

        private class DataElementCell : ViewCell
        {
            public Label DescriptionLabel { get; set; }
            public Label ValueLabel { get; set; }

            public DataElementCell(string property, string propertyDescription = null)
            {
                DescriptionLabel = new Label {
                    Text = propertyDescription ?? property,
                    FontAttributes = FontAttributes.Bold,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                };

                ValueLabel = new Label {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalTextAlignment = TextAlignment.End,
                };
                ValueLabel.SetBinding(Label.TextProperty, property);

                this.View = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Padding = new Thickness(15, 10),
                    Children = { DescriptionLabel, ValueLabel }
                };
            }
        }   
    }
}
