using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using FieldEngineerLite.Helpers;
using FieldEngineerLite.Models;
using System.Threading;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Net.Http;
using Xamarin.Forms;
using System.Reflection;

using Microsoft.Azure.AppService;


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
            
        
        public AppServiceClient AppService = 
            new AppServiceClient("https://fieldengineeref90e9309d7f4a608a99748e0eea69de.azurewebsites.net");
        // 2. add sync table
        private IMobileServiceSyncTable<Job> jobTable;
          
        
        public async Task InitializeAsync()
        {
            this.MobileService = AppService.CreateMobileServiceClient(
                "https://fetechnician-code.azurewebsites.net/",
                "OtFsjAFDBBMENsPCBQFJmItwjvAfaX77");
            // 3. initialize local store

            var store = new MobileServiceSQLiteStore("local-db-fabrikam80");
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

            await EnsureLogin();
            //5. add sync
            try
            {
                await this.MobileService.SyncContext.PushAsync();

                var query = jobTable.CreateQuery()
                    .Where(job => job.AgentId == "2");

                await jobTable.PullAsync(null, query);
            }
            catch (Exception)
            { 
            }
        }
        
        public async Task EnsureLogin()
        {
            LoginInProgress = true;
            while (this.AppService.CurrentUser == null) {
                //await this.AppService.LoginAsync(
                try 
                {
                    await this.AppService.LoginAsync (App.UIContext, 
                        MobileServiceAuthenticationProvider.WindowsAzureActiveDirectory.ToString());
                }
                catch(Exception ex)
                {
                    Console.WriteLine("failed to authenticate: " + ex.Message);
                }
               
            }

            LoginInProgress = false;

        }

        public async Task CompleteJobAsync(Job job)
        {
            await UpdateJobAsync(job);

            if (Online)
                await this.SyncAsync();
        }
    }
}
