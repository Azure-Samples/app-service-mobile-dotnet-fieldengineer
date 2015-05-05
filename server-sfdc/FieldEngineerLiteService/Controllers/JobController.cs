using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.AppService;
using FieldEngineerLiteService.DataObjects;
using FieldEngineerLiteService.Models;
using Salesforce;
using Microsoft.Azure.Mobile.Server.Security;
using Microsoft.Azure.Mobile.Security;
using System;
using Microsoft.Azure.AppService;
using Newtonsoft.Json.Linq;
using System.Configuration;

namespace FieldEngineerLiteService.Controllers
{
    public class JobController : TableController<Job>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            JobDbContext context = new JobDbContext();
            DomainManager = new EntityDomainManager<Job>(context, Request, Services, enableSoftDelete: true);
        }

        // GET tables/Job
        public IQueryable<Job> GetAllJobs()
        {
            return Query();
        }

        // GET tables/Job/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Job> GetJob(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Job/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public async Task<Job> PatchJob(string id, Delta<Job> patch)
        {

            Job job = patch.GetEntity();  // get new value
            var user = this.User as ServiceUser;
            var creds = await user.GetIdentityAsync<AzureActiveDirectoryCredentials>();
            var token = this.Request.Headers.GetValues("x-zumo-auth").First();
            SalesforceClient client = new SalesforceClient(false);
            client.SetUser(user.Id, token);
            this.Services.Log.Info("Patch: userId: " + user.Id + ", token:" + token);            

            await client.UpdateCase( "0000" + job.JobNumber, job.Status, job.WorkPerformed);

            return await UpdateAsync(id, patch);
        }

        // POST tables/Job
        public async Task<IHttpActionResult> PostJob(Job item)
        {
            Job current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Job/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteJob(string id)
        {
            return DeleteAsync(id);
        }
    }
}