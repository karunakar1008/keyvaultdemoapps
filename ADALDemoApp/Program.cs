using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;

namespace ADALDemoApp
{
    class Program
    {
       
            static string appId = "dedb6748-772b-4679-998c-3a39a77dec60";
            static string secret = "~4V8Q~cwygDAyOZaXDfVWzDFHMzNnsRrFUT5ybgA";
            static string tenantId = "0c44cc27-a1cd-4b20-9c98-607c791fc563";
            static void Main(string[] args)
            {
                var context = new AuthenticationContext("https://login.windows.net/" + tenantId);
                var credential = new ClientCredential(clientId: appId, clientSecret: secret);
                AuthenticationResult result = context.AcquireTokenAsync(appId, credential).Result;
                if (result == null)
                    throw new InvalidOperationException("Failed to obtain the JWT token");
                var token = result.AccessToken;
            }

    }
}
