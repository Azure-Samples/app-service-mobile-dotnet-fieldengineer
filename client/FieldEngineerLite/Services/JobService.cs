using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using FieldEngineerLite.Helpers;
using FieldEngineerLite.Models;
using Microsoft.WindowsAzure.MobileServices.Eventing;
using System.Diagnostics;

namespace FieldEngineerLite
{
    public class JobService
    {
        public bool Online = false;        
        
        public IMobileServiceClient MobileService = null;
        private IMobileServiceSyncTable<Job> jobTable;

        // Placeholder string for Try App Service is ZUMOAPPURL
        // To use with your own app, use URL in the form https://your-site-name.azurewebsites.net/
        private const string MobileUrl = "ZUMOAPPURL";

        public async Task InitializeAsync()
        {
            this.MobileService = 
                new MobileServiceClient(MobileUrl, new LoggingHandler());

            var store = new MobileServiceSQLiteStore("local.db");
            store.DefineTable<Job>();

            await MobileService.SyncContext.InitializeAsync(store, StoreTrackingOptions.NotifyLocalAndServerOperations);
            jobTable = MobileService.GetSyncTable<Job>();

            // This sample doesn't do any authentication. To add it, see 
            // https://azure.microsoft.com/en-us/documentation/articles/app-service-mobile-xamarin-forms-get-started-users/
        }

        public async Task<IEnumerable<Job>> ReadJobs(string search)
        {
            return await jobTable.ToEnumerableAsync();
        }

        public async Task UpdateJobAsync(Job job)
        {
            job.Status = Job.CompleteStatus;
            
            await jobTable.UpdateAsync(job);
            
            // trigger an event so that the job list is refreshed
            await MobileService.EventManager.PublishAsync(new MobileServiceEvent("JobChanged"));
        }

        public async Task SyncAsync()
        {
            try
            {
                await this.MobileService.SyncContext.PushAsync();
                await jobTable.PullAsync(null, jobTable.CreateQuery());
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        public async Task CompleteJobAsync(Job job)
        {
            await UpdateJobAsync(job);

            if (Online)
                await this.SyncAsync();
        }
    }
}
