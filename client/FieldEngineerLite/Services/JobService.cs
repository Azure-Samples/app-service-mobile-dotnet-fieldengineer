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
        public bool LoginInProgress = false;
        public bool Online = false;        
        
        public IMobileServiceClient MobileService = null;
        private IMobileServiceSyncTable<Job> jobTable;
                  
        public async Task InitializeAsync()
        {
            this.MobileService = 
                new MobileServiceClient("https://fieldengineerlite-code.azurewebsites.net/", 
                new LoggingHandler(true));

            var store = new MobileServiceSQLiteStore("local.db");
            store.DefineTable<Job>();

            await MobileService.SyncContext.InitializeAsync(store, StoreTrackingOptions.NotifyLocalAndServerOperations);
            jobTable = MobileService.GetSyncTable<Job>();
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


        //public async Task EnsureLogin()
        //{
        //    LoginInProgress = true;
        //    while (this.AppService.CurrentUser == null) {
        //        //await this.AppService.LoginAsync(
        //        try 
        //        {
        //            await this.AppService.LoginAsync (App.UIContext, 
        //                MobileServiceAuthenticationProvider.WindowsAzureActiveDirectory.ToString());
        //        }
        //        catch(Exception ex)
        //        {
        //            Console.WriteLine("failed to authenticate: " + ex.Message);
        //        }

        //    }

        //    LoginInProgress = false;
        //}
    }
}
