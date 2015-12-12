using System.Linq;
using Xamarin.Forms;
using FieldEngineerLite.Models;
using FieldEngineerLite.Views;

namespace FieldEngineerLite
{
    public class MyNavigationPage : NavigationPage
    {
        public MyNavigationPage(Page root)
            : base(root)
        {
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }

    public class JobMasterDetailPage : MasterDetailPage
    {
        private JobService jobService = new JobService();
        
        public JobMasterDetailPage()
        {
            JobListPage listPage = new JobListPage(jobService);
            listPage.JobList.ItemSelected += (sender, e) =>
            {
                var selectedJob = e.SelectedItem as Job;
                if (selectedJob != null)
                {
                    NavigateTo(e.SelectedItem as Job);
                }
            };

            var listNavigationPage = new MyNavigationPage(listPage);
            listNavigationPage.Title = "Appointments";
            Master = listNavigationPage;
            JobDetailsPage details = new JobDetailsPage(jobService);

            details.Content.IsVisible = false;
            Detail = new MyNavigationPage(details);
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await jobService.InitializeAsync();
            
            var jobs = await jobService.ReadJobs("");
            if (jobs.Count() > 0)
            {
                Job job = jobs.First();
                NavigateTo(job);
            }
        }

        public void NavigateTo(Job item)
        {
            JobDetailsPage page = new JobDetailsPage(jobService);
            page.BindingContext = item;
            Detail = new NavigationPage(page);
            IsPresented = false;
        }
    }
}

