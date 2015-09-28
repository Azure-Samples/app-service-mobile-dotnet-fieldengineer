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
        private const string connectionStringName = "Name=MS_TableConnectionString";

        public JobDbContext()
            : base(connectionStringName)
        {
        }

        public DbSet<Job> JobsDbSet { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            string schema = "mobile";
            if (!string.IsNullOrEmpty(schema))
            {
                modelBuilder.HasDefaultSchema("mobile");
            }

            Database.SetInitializer<JobDbContext>(null); //new JobDbContextInitializer());
            base.OnModelCreating(modelBuilder);
        }
    }

    public class JobDbContextInitializer :
           ClearDatabaseSchemaAlways<JobDbContext>
    {
        protected override void Seed(JobDbContext context)
        {
            List<Job> jobs = new List<Job>
            {
                new Job(),
                new Job()
            };

            foreach (Job job in jobs)
            {
                context.Set<Job>().Add(job);
            }
            base.Seed(context);
        }
    }
}