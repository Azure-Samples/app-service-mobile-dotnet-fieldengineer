using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using FieldEngineerLite.Helpers;
using FieldEngineerLite.Models;
using Microsoft.Azure.AppService;
using UIKit;
using HomeKit;

namespace FieldEngineerLite.Views
{
    public class JobListPage : ContentPage
    {

        private const bool DEFAULT_ONLINE_STATE = false;

        public ListView JobList;

        public JobListPage()
        {

            JobList = new ListView
            {
                HasUnevenRows = true,
                IsGroupingEnabled = true,
                GroupDisplayBinding = new Binding("Key"),
                GroupHeaderTemplate = new DataTemplate(typeof(JobGroupingHeaderCell)),
                ItemTemplate = new DataTemplate(typeof(JobCell))
            };

            var onlineLabel = new Label { Text = "Online", Font = AppStyle.DefaultFont, YAlign = TextAlignment.Center };
            var onlineSwitch = new Switch { IsToggled = DEFAULT_ONLINE_STATE, VerticalOptions = LayoutOptions.Center };

            App.JobService.Online = onlineSwitch.IsToggled;

            onlineSwitch.Toggled += async (sender, e) =>
            {
                App.JobService.Online = onlineSwitch.IsToggled;

                if (onlineSwitch.IsToggled)
                {
                    await App.JobService.SyncAsync();
                    await this.RefreshAsync();
                }
            };

            var syncButton = new Button
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Font = AppStyle.DefaultFont,
                Text = "Refresh",
                WidthRequest = 100,
            };

            syncButton.Clicked += async (object sender, EventArgs e) =>
            {
                try
                {
                    syncButton.Text = "Refreshing...";
                    await App.JobService.SyncAsync();
                    await this.RefreshAsync();
                }
                finally
                {
                    syncButton.Text = "Refresh";
                }
            };

            //var clearButton = new Button
            //{
            //    HorizontalOptions = LayoutOptions.CenterAndExpand,
            //    VerticalOptions = LayoutOptions.CenterAndExpand,
            //    Font = AppStyle.DefaultFont,
            //    Text = "Clear",
            //    WidthRequest = 100
            //};

            //clearButton.Clicked += async (object sender, EventArgs e) =>
            //{
            //    await App.JobService.ClearAllJobs();
            //    await this.RefreshAsync();
            //};
            
            this.Title = "Appointments";

            var logo = new Image() { Aspect = Aspect.AspectFit };
            logo.Source = ImageSource.FromFile("Fabrikam-small.png");

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += async (s, e) => {
                
                await App.JobService.EnsureLogin();

                var consentlink = await App.JobService.AppService.GetConsentLinkAsync("SalesforceConnector", "javascript:close()");

                var browser = new WebView {
                    
                    //HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    Source = consentlink,
                    //HeightRequest = 500



                };
                var closeButton = new Button
                {
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    Font = AppStyle.DefaultFont,
                    Text = "Close",
                    WidthRequest = 50,
                };
                closeButton.Clicked += async (object sender, EventArgs ev) =>
                {
                    await this.Navigation.PopAsync();
                };

                ContentPage webViewPage = new ContentPage();
                webViewPage.Title = "Authorize";
                webViewPage.Content = new StackLayout {
                    Orientation = StackOrientation.Vertical,
                 
                    Children = {
                       
                        browser
                    }
                };
               

                await this.Navigation.PushAsync(webViewPage);




            };
        logo.GestureRecognizers.Add(tapGestureRecognizer);

            this.Content = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Padding = new Thickness {Top = 15},
                Children = {
                    //searchBar,
                    new StackLayout {
                        
                        Orientation = StackOrientation.Horizontal,
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                        Children = {
                            logo,
                            syncButton, new Label { Text = "   "}, onlineLabel, onlineSwitch
                        }
                    },                                        
                    JobList
                }
            };
            //this.RefreshAsync().Wait();
        }

        public async Task FakeIt()
        {
            while (true)
            {
                if (App.JobService.Online)
                {
                    await App.JobService.SyncAsync();
                    await Task.Delay(3000);
                    await RefreshAsync();
                }
                await Task.Delay(3000);
            }
        }

        Task FakeItTask;
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await this.RefreshAsync();

            FakeItTask = FakeIt();
            System.Diagnostics.Debug.WriteLine(FakeItTask);
        }

        public async Task RefreshAsync()
        {
            //if (App.JobService.LoginInProgress == true) return;
            var groups = from job in await App.JobService.ReadJobs("")
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
