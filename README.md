# Field Engineer <small>Azure Mobile Apps Demo</small>

In this demo, we create a Mobile App which will allow field engineers at our cable company to more easily manage their appointments throughout the day. The app will sync the engineer's jobs for that day onto their device when there is an internet connection. When there isn't an internet connection, the Mobile Apps offline sync feature keeps the records available and edittable; when the engineer connects back to the internet, the local changes are synced and any new Jobs are pulled to their device.

This is a demo where we show off some great features of Azure Mobile Apps and App Service, including:
 - Integrated Authentication with AAD and SalesForce
 - Offline Sync
 - Easy to use client SDK for Xamarin

## Deploying

### Full Demo

1. Create a Mobile App.

2. In the Azure Portal, navigate to your Mobile App and open the Settings blade.

3. Click on the Authentication setting. If you have not yet created a Gateway, do so now by following the instructions in the Authentication blade.

4. [Create an AAD Application](https://azure.microsoft.com/en-us/documentation/articles/app-service-mobile-how-to-configure-active-directory-authentication-preview/) and add it to the Mobile App Authentication Azure Active Directory settings menu.

5. Deploy the code in the `./server-sfdc` project. You'll need to do this from Visual Studio.

6. Change the Gateway URL, Mobile App URL, and Mobile App name in the [`./client/FieldEngineerLite/Services/JobService.cs`](./client/FieldEngineerLite/Services/JobService.cs) file.

7. Build and run the client from Xamarin. You'll need a Mac build host.

8. You can then login using one of your AAD identities. 

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
