using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace FieldEngineerLite.Models
{
    public class Job : INotifyPropertyChanged
    {
        public Job()
        {
            Items = new List<WorkItem> 
            {
                new WorkItem {Name = "Replace cable box", Completed = false},
                new WorkItem {Name = "Repair cable outlet", Completed = false},
                new WorkItem {Name = "Fix cable wiring", Completed = false}
            };
        }

        public const string CompleteStatus = "Completed";
        public const string InProgressStatus = "In Progress";
        public const string PendingStatus = "Not Started";

        public string Id { get; set; }
        public string AgentId { get; set; }
        public string JobNumber { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Title { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public string WorkPerformed { get; set; }

        private string status;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public string Status
        {
            get { return status; }
            set
            {
                status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        [Version]
        public string Version { get; set; }

        [JsonIgnore]
        public List<WorkItem> Items { get; set; }
    }
}
