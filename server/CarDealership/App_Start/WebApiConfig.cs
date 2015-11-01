using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Web.Http;
using FieldEngineerLiteService.Models;
using Microsoft.Azure.Mobile.Server.Config;

namespace CarDealership
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            new MobileAppConfiguration()
                .UseDefaultConfiguration()
                .ApplyTo(config);


            Database.SetInitializer(new JobDbContextInitializer());

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

