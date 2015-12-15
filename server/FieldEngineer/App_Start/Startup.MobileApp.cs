using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using FieldEngineerLiteService.Models;
using Microsoft.Azure.Mobile.Server.Config;
using Owin;

namespace FieldEngineer
{
    public partial class Startup
    {
        public static void ConfigureMobileApp(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            config.EnableSystemDiagnosticsTracing();

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            new MobileAppConfiguration()
                .UseDefaultConfiguration()
                .ApplyTo(config);


            Database.SetInitializer(new JobDbContextInitializer());

            app.UseWebApi(config);

            // ----------------------------------------
            // UseDefaultConfiguration() equivalent to:
            // ----------------------------------------
            //new MobileAppConfiguration()
            //    .AddTables(
            //        new MobileAppTableConfiguration()
            //            .MapTableControllers()
            //            .AddEntityFramework())
            //    .MapApiControllers()
            //    .MapLegacyCrossDomainController()
            //    .AddMobileAppHomeController()
            //    .AddAppServiceAuthentication()
            //    .AddPushNotifications()
            //    .ApplyTo(config);




            // To display errors in the browser during development, uncomment the following
            // line. Comment it out again when you deploy your service for production use.
            // config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

        }
    }


}

