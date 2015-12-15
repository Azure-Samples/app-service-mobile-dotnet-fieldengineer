using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Tables;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FieldEngineerLiteService.DataObjects
{
    public class Job : ITableData
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

        #region System properties
        public string Id { get; set; }

        public byte[] Version { get; set; }

        #if TRY_APP_SERVICE
        [NotMapped]
        #endif        
        public DateTimeOffset? CreatedAt { get; set; }

        #if TRY_APP_SERVICE
        [NotMapped]
        #endif
        public DateTimeOffset? UpdatedAt { get; set; }

        public bool Deleted { get; set; }
        #endregion

        #if TRY_APP_SERVICE
        // SQL Compact Edition does not support the DateTimeOffset type.
        // These are simple backing properties that store these values using the DateTime type instead.
        public DateTime __createdAtDateTime
        {
            get { return CreatedAt.HasValue ? CreatedAt.Value.DateTime : DateTime.Now; }
            set { CreatedAt = value; }
        }

        public DateTime __updatedAtDateTime
        {
            get { return UpdatedAt.HasValue ? UpdatedAt.Value.DateTime : DateTime.Now; }
            set { UpdatedAt = value; }
        }
        #endif

        public Job()
        {
            Id = Guid.NewGuid().ToString("N");
            AgentId = "Carla Davis";
            JobNumber = "";
            Status = "Not Started";
        }
    }

}