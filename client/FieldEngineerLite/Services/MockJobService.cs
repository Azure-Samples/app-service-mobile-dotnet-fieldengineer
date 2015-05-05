/*using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FieldEngineerLite.Models;

namespace FieldEngineerLite
{
    public class MockData
    {
        public MockData()
        {
            Jobs = GetDummyData();            
        }
        
        public List<Job> Jobs;

        private static List<Job> GetDummyData()
        {
            return new List<Job> {
                new Job 
                { 
                    AgentId = "agent1",
                    CustomerName = "Lorem Ipsum",

                    StartTime = "08:30",
                    EndTime =  "09:30",
                    Id = "1",
                    JobNumber = "1",
                    Status = Job.InProgressStatus,
                    Title = "Dolor sit amet"
                },
                new Job 
                { 
                    AgentId = "agent1", 
                    CustomerName = "Consectetur Adipiscing",
                    StartTime = "10:30",
                    EndTime = "11:30",
                    Id = "2",
                    JobNumber = "2",
                    Status = Job.PendingStatus,
                    Title = "Incididunt ut labore et dolore"
                },
                new Job 
                { 
                    AgentId = "agent1", 
                    CustomerName = "Magna Aliqua",

                    StartTime = "11:30",
                    EndTime = "12:30",
                    Id = "3",
                    JobNumber = "3",
                    Status = Job.CompleteStatus,
                    Title = "Ut enim ad minim veniam"
                }
            };
        }

    }
}
*/
