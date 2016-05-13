using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using FieldEngineerLite.Models;
using Microsoft.WindowsAzure.MobileServices.Eventing;
using System.ComponentModel;
using Microsoft.WindowsAzure.MobileServices.Sync;

namespace FieldEngineerLite.Views
{
    public class JobListPage : ContentPage
    {
        private const bool DEFAULT_ONLINE_STATE = false;
        public ListView JobList;
        private JobService jobService;
        private long pendingChanges;

        public JobListPage(JobService service)
        {
            this.jobService = service;
            
            JobList = new ListView
            {
                HasUnevenRows = true,
                IsGroupingEnabled = true,
                GroupDisplayBinding = new Binding("Key"),
                GroupHeaderTemplate = new DataTemplate(typeof(JobGroupingHeaderCell)),
                ItemTemplate = new DataTemplate(typeof(JobCell))
            };

            var onlineLabel = new Label { Text = "Online", VerticalTextAlignment = TextAlignment.Center };
            var onlineSwitch = new Switch { IsToggled = DEFAULT_ONLINE_STATE, VerticalOptions = LayoutOptions.Center };

            jobService.Online = onlineSwitch.IsToggled;

            onlineSwitch.Toggled += async (sender, e) =>
            {
                jobService.Online = onlineSwitch.IsToggled;

                if (onlineSwitch.IsToggled)
                {
                    await jobService.SyncAsync();
                    await this.RefreshAsync();
                }
            };

            var syncButton = new Button
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Text = "Refresh",
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button)),
                WidthRequest = 120,
            };

            syncButton.Clicked += async (object sender, EventArgs e) =>
            {
                try
                {
                    syncButton.Text = "Refreshing...";
                    await jobService.SyncAsync();
                    await this.RefreshAsync();
                }
                finally
                {
                    syncButton.Text = "Refresh";
                }
            };
            
            this.Title = "Appointments";

            var statusBar = new Label { BackgroundColor = Color.Gray, TextColor = Color.White };
            statusBar.SetBinding(Label.TextProperty, "PendingChanges", stringFormat: "Pending changes: {0}");
            statusBar.BindingContext = this;

            this.Content = new StackLayout {
                Orientation = StackOrientation.Vertical,
                Padding = new Thickness { Top = 15 },
                Children = {
                    new StackLayout {

                        Orientation = StackOrientation.Horizontal,
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                        Children = {
                            syncButton, new Label { Text = "   "}, onlineLabel, onlineSwitch
                        }
                    },
                    JobList,
                    new ContentView {
                        Content = statusBar, Padding = 5, BackgroundColor = Color.Gray,
                    }
                }
            };
        }

        public long PendingChanges
        {
            get
            {
                return pendingChanges;
            }

            set
            {
                pendingChanges = value;
                OnPropertyChanged();
            }
        }

        private void StatusObserver(MobileServiceEvent obj)
        {
            // Refresh the UI if a job was edited on the detail page
            if (obj.Name == "JobChanged") {
                Device.BeginInvokeOnMainThread(async () => {
                    await RefreshAsync();
                });
            }
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await this.RefreshAsync();

            jobService.MobileService.EventManager.Subscribe<MobileServiceEvent>(StatusObserver);
            jobService.MobileService.EventManager.Subscribe<StoreOperationCompletedEvent>(StoreOperationEventHandler);
        }

        private async void StoreOperationEventHandler(StoreOperationCompletedEvent obj)
        {
            await Task.Delay(500);
            PendingChanges = jobService.MobileService.SyncContext.PendingOperations;
        }

        public async Task RefreshAsync()
        {
            var groups = from job in await jobService.ReadJobs("")
                         group job by job.Status into jobGroup
                         select jobGroup;

            JobList.ItemsSource = groups.OrderBy(group => GetJobSortOrder(group.Key));
        }

        private int GetJobSortOrder(string status)
        {
            switch (status)
            {
                case Job.InProgressStatus: return 0;
                case Job.PendingStatus: return 1;
                case Job.CompleteStatus: return 2;
                default: return -1;
            }
        }
    }
}
