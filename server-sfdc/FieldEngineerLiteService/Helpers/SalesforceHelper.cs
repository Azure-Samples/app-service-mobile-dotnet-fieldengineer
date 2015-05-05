using Microsoft.Azure.AppService;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Salesforce.Common;
using Salesforce.Common.Models;
using Salesforce.Force;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace Salesforce
{
    public class Contact
    {
        public string Name { get; set; }
    }

    public class Case
    {
        public string Id { get; set; }
        public string CaseNumber {get; set;}
        public string Subject {get; set;}
        public Contact Contact {get; set;}
        public string Status {get; set;}
    }

    public class SalesforceClient
    {
        bool isWebJob = false;
        public string _userid;
        public string _zumotoken;
        string UserId { get { return _userid; } }
        string ZumoToken { get { return _zumotoken; } }
        ForceClient _client = null;

        public SalesforceClient (bool isWebJob)
        {
            this.isWebJob = isWebJob;
        }
        public void SetUser(string userid, string zumotoken)
        {
            this._userid = userid;
            this._zumotoken = zumotoken;
        }
        internal async Task<ForceClient> GetClientForWebJob()
        {

            var consumerkey = ConfigurationManager.AppSettings["ConsumerKey"];
            var consumersecret = ConfigurationManager.AppSettings["ConsumerSecret"];
            var user = ConfigurationManager.AppSettings["User"];
            var password = ConfigurationManager.AppSettings["Password"];
            
            
            var auth = new AuthenticationClient();
            await auth.UsernamePasswordAsync(consumerkey, consumersecret, user, password);
            
            var client = new ForceClient(auth.InstanceUrl, auth.AccessToken, auth.ApiVersion);
            Console.WriteLine("Connected to Salesforce, apiVersion:" + auth.ApiVersion);

            return client;

        }
        internal async Task<ForceClient> GetClientForApp()
        {
            
            var gatewayUrl = ConfigurationManager.AppSettings["emA_RuntimeUrl"];
            

            AppServiceClient appServiceClient = new AppServiceClient(gatewayUrl);
            appServiceClient.SetCurrentUser(this.UserId, this.ZumoToken);
            var gateway = appServiceClient.CreateApiAppClient(new System.Uri(gatewayUrl));
            var result = await gateway.GetAsync(String.Format("/api/tokens?api-version=2015-01-14&tokenName={0}", "salesforce"));
            var jsonString = await result.Content.ReadAsStringAsync();
            JToken json = JToken.Parse(jsonString);
            if (json["Properties"] == null)
            {
                return null;
            }

            var accessToken = json["Properties"]["AccessToken"].ToString();
            var instanceUrl = json["Properties"]["InstanceUrl"].ToString();
            var apiVersion = "v32.0"; //need to get dynamically
            Debug.WriteLine("App Service Token:" + jsonString);
            
            
            var client = new ForceClient(instanceUrl, accessToken, apiVersion);
            Console.WriteLine("Connected to Salesforce, apiVersion:" + apiVersion);

            return client;
            

        }
        public async Task<ForceClient> GetClient()
        {
            if (_client != null) return _client;
            if (isWebJob == true)
            {
                _client =  await GetClientForWebJob();
            }
            else
            {
                _client = await GetClientForApp();
            }
            return _client;
        }
            
            
        
         public async Task<string> InsertComment(string caseRecordId, string comment)
        {
            ForceClient client = await GetClient();
            dynamic caseComment = new ExpandoObject();
            caseComment.ParentId = caseRecordId;
            caseComment.CommentBody = comment;
            return await client.CreateAsync("CaseComment", caseComment);

        }
        
         public async Task<string> UpdateCase(string caseNumber, string status, string internalComments)
        {
            status = MapMobileStatus(status);
            var client = await GetClient();

            var recordId = await GetCase(caseNumber);

            if (recordId == null)
                return null;

            string insertResult = await InsertComment(recordId, internalComments);

            dynamic updated = new ExpandoObject();
            updated.Status = status;
            
            var response = await client.UpdateAsync("Case", recordId, updated);
            return response.Success;
        }

         private async Task<string> GetCase(string caseNumber)
        {
            ForceClient client = await GetClient();

            string query = 
                "SELECT Id, CaseNumber, Subject, Contact.Name, Status FROM Case where CaseNumber = '" + caseNumber + "'";

            var cases = await client.QueryAsync<Case>(query);

            if (cases == null || cases.TotalSize == 0)
                return null;

            return cases.Records.First().Id;
        }

         public async Task<IEnumerable<Case>> GetActiveCases(string caseNumber)
        {
            string customers = ConfigurationManager.AppSettings["customers"];
            ForceClient client = await GetClient();
            
            string query = "SELECT Id, CaseNumber, Subject, Contact.Name, Status FROM Case where Contact.Name IN (" + customers + ")";
            if (caseNumber != null)
            {
                query += " AND CaseNumber = '" + caseNumber + "'";
            }

            var cases =  await client.QueryAsync<Case>(query);
            
            if (cases == null) return null;
            if (cases.TotalSize == 0) return null;
            return cases.Records;
        }

        static public string MapMobileStatus(string mobileStatus)
        {
            switch (mobileStatus)
            {
                case "Not Started": return "New";
                case "In Progress": return "Working";
                case "Completed": return "Closed";
                default: return "New";
            }
        }
        static public string MapStatus(string salesforceStatus)
        {
            switch (salesforceStatus)
            {
                case "New": return "Not Started";
                case "Working": return "In Progress";
                case "Closed": return "Completed";
                default: return "In Progress";
            }
        }
    }
}