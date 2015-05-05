using Microsoft.Azure.Mobile.Server;

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
    }
}