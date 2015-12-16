---
services: app-service\mobile
platforms: dotnet
author: lindydonna
---

# Field Engineer 
**Azure Mobile Apps Sample**

**Try out a simple version of this demo and other Xamarin demos on ["Try App Service"](https://aka.ms/trymobile).**

In this demo, we create a Mobile App which will allow field engineers at our cable company to more easily manage their appointments throughout the day. The app will sync the engineer's jobs for that day onto their device when there is an internet connection. When there isn't an internet connection, the Mobile Apps offline sync feature keeps the records available and edittable; when the engineer connects back to the internet, the local changes are synced and any new Jobs are pulled to their device.

This is a demo where we show off some great features of Azure Mobile Apps and App Service, including:
 - Offline Sync
 - Easy to use client SDK for Xamarin

### Demo

The first step of the demo is deploying the environment and code. Checkout the **[Deploying](#deploying)** section below. Or, just click this button!

[![Deploy to Azure](http://azuredeploy.net/deploybutton.png)](https://azuredeploy.net/)

Once you've refreshed the client to get all remote changes, you'll see all the jobs you have waiting to be fulfilled. At this point, you can disconnect your internet and make some changes. All your changes will be saved through a restart of the app. 

If you visit the admin page that comes with your site ({sitename}.azurewebsites.net/admin), you can see that none of the records have been updated. If you connect to the internet on your device again and refresh, your changes will sync to the server. You can see those changes reflected via the admin portal.

[Checkout this gif of the demo!](http://i.imgur.com/J60zc9x.gif)

### See also

 - The Xamarin CRM app utilizing Azure Mobile
   - [Try it now](aka.ms/trymobile)
   - [Source on GitHub](https://github.com/xamarin/app-crm/)
 - [Azure Mobile Apps on Azure.com](https://azure.microsoft.com/en-us/services/app-service/mobile/)
 - [Kirill Gavrylyuk giving this demo on Azure Fridays](https://channel9.msdn.com/Shows/Azure-Friday/Azure-App-Service-Mobile-Apps-with-Kirill-Gavrylyuk)

## Deploying

Just click this button to deploy!

[![Deploy to Azure](http://azuredeploy.net/deploybutton.png)](https://azuredeploy.net/)

### Manual deployment - full demo

The easiest way to deploy is to use the Continuous Integration feature of App Service.

1. Create a new Mobile App from the portal.

2. In the **Mobile** -> **Data** section, create a new Data Connection. For more detailed instructions, see [Create a .NET backend using the Azure portal](https://azure.microsoft.com/en-us/documentation/articles/app-service-mobile-dotnet-backend-how-to-use-server-sdk/#create-app).

3. In the portal, navigate to All Settings for the new app.

4. In the **Publishing** section, select **Continuous deployment**.

5. Configure source control using either the main fieldengineer branch or your own fork.

6. Click the **Sync** button to do an initial deployment.

### Try App Service version

There is a project configuration that uses SQL CE as the server database and therefore does not require SQL Azure. 

To set up this version, after step #2 above, add an app setting with key `TRY_APP_SERVICE` and value `1`. To set an app setting, go to **All Settings** -> **Application Settings** -> **App Settings**. Then, continue with steps 3-6 above.

### Client project

Requirements:
 - Xamarin Studio running on Mac OS X
 - Azure Account (for a free trial, use [Try App Service](https://aka.ms/trymobile) and the instructions there)

Steps:

1. Open up the Xamarin Project

2. Update the Mobile App Name, in `./Services/JobService.cs`

3. Select an iPad target and start debugging.

## Issues

If you're having problems with the App, create an issue on [GitHub](https://github.com/azure/fieldengineer/issues).

We welcome and encourage PRs from the community. Just checkout [Azure's CLA](https://cla.azure.com/) first.

## License

See [LICENSE](./LICENSE) for full details.
