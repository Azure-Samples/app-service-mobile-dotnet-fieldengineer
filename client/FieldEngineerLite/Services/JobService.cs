using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using FieldEngineerLite.Helpers;
using FieldEngineerLite.Models;

namespace FieldEngineerLite
{

    public class JobService
    {
        #region Member variables
        
        public bool LoginInProgress = false;
        public bool Online = false;        
        
        #endregion
        
        // 1. add client initializer
        public IMobileServiceClient MobileService = null;
        
        // 2. add sync table
        private IMobileServiceSyncTable<Job> jobTable;
          
        
        public async Task InitializeAsync()
        {
            this.MobileService = new MobileServiceClient("https://fieldengineerlite-code.azurewebsites.net/", new LoggingHandler(true));
            // 3. initialize local store

            var store = new MobileServiceSQLiteStore("local.db");
            store.DefineTable<Job>();

            await MobileService.SyncContext.InitializeAsync(store);

            jobTable = MobileService.GetSyncTable<Job>();
        }

        public async Task<IEnumerable<Job>> ReadJobs(string search)
        {
            // 4. read from local db
            
            var query = jobTable.CreateQuery();
            if (string.IsNullOrEmpty(search) == false)
            {
                query = query.Where(job => (job.Title == search));
            }
            
            return await query.ToEnumerableAsync();
        }

        public async Task UpdateJobAsync(Job job)
        {
            job.Status = Job.CompleteStatus;
            
            // 5. update local db
            await jobTable.UpdateAsync(job);
        }

        public async Task SyncAsync()
        {
            // 7. add auth

            //await EnsureLogin();
            //5. add sync
            try
            {
                await this.MobileService.SyncContext.PushAsync();
                await jobTable.PullAsync(null, jobTable.CreateQuery());
            }
            catch (Exception)
            { 
            }
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

        public async Task CompleteJobAsync(Job job)
        {
            await UpdateJobAsync(job);

            if (Online)
                await this.SyncAsync();
        }
    }
}
