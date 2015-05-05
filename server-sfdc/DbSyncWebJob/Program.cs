using Newtonsoft.Json.Linq;
using FieldEngineerLiteService.DataObjects;
using Salesforce;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FieldEngineerLiteService.Models;

namespace DbSyncWebJob
{
    // To learn more about Microsoft Azure WebJobs, please see http://go.microsoft.com/fwlink/?LinkID=401557
    class Program
    {
        static void Main()
        {
            for (; ; )
            {
                Sync();
                System.Threading.Thread.Sleep(30000);
            }
        }

        static void Sync()
        {
            SalesforceClient client = new SalesforceClient(true);
            Task<IEnumerable<Case>> task = client.GetActiveCases(null);
            task.Wait();
            IEnumerable<Case> cases = task.Result;

            JobDbContext db = new JobDbContext();
            var caseNumbers = new List<string>();

            foreach (Case c in cases)
            {
                string customer = c.Contact.Name;
                string caseNumber = c.CaseNumber.Substring(4);
                string title = c.Subject;
                string status = SalesforceClient.MapStatus(c.Status);
                caseNumbers.Add(caseNumber);

                Job job = db.JobsDbSet.Where(j => j.JobNumber == caseNumber).FirstOrDefault();

                if (job == null)
                {
                    db.JobsDbSet.Add(
                        new Job()
                        {
                            Id = Guid.NewGuid().ToString(),
                            JobNumber = caseNumber,
                            AgentId = "2",
                            CustomerName = customer,
                            CustomerAddress = "One Microsoft Way, Redmond",
                            CustomerPhoneNumber = "1-206-888-8888",
                            Status = status,
                            Title = title,
                            StartTime = "13:00",
                            EndTime = "14:00"
                        });
                }
                else
                {
                    job.Title = title;
                    job.Status = status;
                    job.CustomerName = customer;
                }
            }

            db.SaveChanges();
            
            // handle deletions
            foreach (Job job in db.JobsDbSet)
            {
                if (caseNumbers.Contains(job.JobNumber) == false)
                {
                    db.Entry(job).Entity.Deleted = true;
                }
            }

            db.SaveChanges();

            Console.WriteLine("WebJob ran at: " + DateTime.Now.ToString());
        }

    }
}
