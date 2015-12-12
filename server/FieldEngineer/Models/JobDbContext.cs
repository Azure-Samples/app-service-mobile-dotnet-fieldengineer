using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Tables;
using FieldEngineerLiteService.DataObjects;
using System.Collections.Generic;

namespace FieldEngineerLiteService.Models
{
    public class JobDbContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to alter your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
        //
        // To enable Entity Framework migrations in the cloud, please ensure that the 
        // service name, set by the 'MS_MobileServiceName' AppSettings in the local 
        // Web.config, is the same as the service name when hosted in Azure.

#if TRY_APP_SERVICE
        private const string connectionStringName = "Name=MS_TryAppService_TableConnectionString";
#else
        private const string connectionStringName = "Name=MS_TableConnectionString";
#endif

        public JobDbContext()
            : base(connectionStringName)
        {
        }

        public DbSet<Job> JobsDbSet { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Add(
                new AttributeToColumnAnnotationConvention<TableColumnAttribute, string>(
                    "ServiceTableColumn", (property, attributes) => attributes.Single().ColumnType.ToString()));

#if TRY_APP_SERVICE
            // The EntityFramework SQL Compact provider does not support the DateTimeOffset type
            modelBuilder.Types<Job>().Configure(x => x.Ignore(prop => prop.CreatedAt));
            modelBuilder.Types<Job>().Configure(x => x.Ignore(prop => prop.UpdatedAt));
#endif

        }
    }

#if TRY_APP_SERVICE
    public class JobDbContextInitializer : DropCreateDatabaseAlways<JobDbContext>
#else
    public class JobDbContextInitializer : CreateDatabaseIfNotExists<JobDbContext>
#endif
    {
        protected override void Seed(JobDbContext context)
        {
            List<Job> jobs = new List<Job>
            {
                new Job {Title = "Install Deluxe DVR box", CustomerName = "Chris Anderson", CustomerAddress = "123 Fake St", CustomerPhoneNumber = "3079876543"},
                new Job {Title = "Cable box outside is missing", CustomerName = "Kirill Gavrylyuk", CustomerAddress = "987 Real St", CustomerPhoneNumber = "4251234567"},
                new Job {Title = "Add Cable to new room", CustomerName = "Donna Malayeri", CustomerAddress = "456 3rd Dimension", CustomerPhoneNumber = "7860987432"},
                new Job {Title = "Approve free Cable for my employees", CustomerName = "Bill Staples", CustomerAddress = "777 Best Boss Dr.", CustomerPhoneNumber = "4259829322"}
            };

            foreach (Job job in jobs)
            {
                context.Set<Job>().Add(job);
            }
            
            base.Seed(context);
        }
    }
}