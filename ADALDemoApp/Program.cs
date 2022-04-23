using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;

namespace ADALDemoApp
{
    class Program
    {
        static string appId = "11100d01-4e5f-4862-9bfd-fe9afb489dd0";
        static string secret = "vMn8Q~3yQlKfVixmUEKHFFgsYHnoiyH5cCYA5bH2";
        static string tenantId = "0c44cc27-a1cd-4b20-9c98-607c791fc563";
        static void Main(string[] args)
        {
            var context = new AuthenticationContext("https://login.windows.net/" + tenantId);
            var credential = new ClientCredential(clientId: appId, clientSecret: secret);
            AuthenticationResult result = context.AcquireTokenAsync(appId, credential).Result;
            if (result == null)
                throw new InvalidOperationException("Failed to obtain the JWT token");
            var token = result.AccessToken;
            Console.WriteLine(token);
        }

    }
}
