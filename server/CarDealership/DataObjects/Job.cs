using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Tables;
using System;

namespace FieldEngineerLiteService.DataObjects
{
    public class Job : EntityData
    {
        public string AgentId { get; set; }

        public string JobNumber { get; set; }

        public string Title { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Status { get; set; }

        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerPhoneNumber { get; set; }

        public string WorkPerformed { get; set; }








        public Job()
        {
            Id = Guid.NewGuid().ToString("N");
            AgentId = "Carla Davis";
            JobNumber = "";
            Status = "Not Started";
        }
    }

}