# Field Engineer <small>Azure Mobile Apps Demo</small>

In this demo, we create a Mobile App which will allow field engineers at our cable company to more easily manage their appointments throughout the day. The app will sync the engineer's jobs for that day onto their device when there is an internet connection. When there isn't an internet connection, the Mobile Apps offline sync feature keeps the records available and edittable; when the engineer connects back to the internet, the local changes are synced and any new Jobs are pulled to their device.

This is a demo where we show off some great features of Azure Mobile Apps and App Service, including:
 - Integrated Authenication with AAD and SalesForce
 - Offline Sync
 - Easy to use client SDK for Xamarin

## Deploying
### Full Demo with SalesForce integration

<div class="alert alert-danger">
  There is currently an issue with deploying API Apps via PowerShell. Once the issue is resolved, we'll update this GitHub repo with instructions on using the full demo.
</div>

### Try It Now Demo

This is a scaled down demo that doesn't require AAD, SalesForce, or Azure SQL DB. It's designed to work with [Try App Service](https://tryappservice.azure.com/).

Requirements:
 - Windows w/ Visual Studio
 - OSX w/ Xamarin Studio
 - Azure Account (for a free trial, use [Try App Service](https://tryappservice.azure.com/) and the instructions there)

1. Switch to the `tryitnow` branch.

2. Deploy the code in the `server` folder to an Azure Mobile App. You can follow the instructions in the [*"Getting Started"* article](https://azure.microsoft.com/en-us/documentation/articles/app-service-mobile-dotnet-backend-xamarin-ios-get-started-preview/) on Azure.com.

3. Open up the Xamarin Project

4. Update the Mobile App Name, Mobile App URL, and Mobile App Gateway strings in the `./Services/JobService.cs`

5. Select an iPad target and start debugging.

## License

See [LICENSE](./LICENSE) for full details.
