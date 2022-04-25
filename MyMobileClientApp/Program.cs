using Microsoft.Identity.Client;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MyMobileClientApp
{
    class Program
    {
        private const string _clientId = "520b1329-1434-4686-a19c-2b31f6da7c20";
        private const string _tenantId = "0c44cc27-a1cd-4b20-9c98-607c791fc563";

        public static async Task Main(string[] args)
        {
            IPublicClientApplication app = PublicClientApplicationBuilder
            .Create(_clientId)
            .WithAuthority(AzureCloudInstance.AzurePublic, _tenantId)
            .WithRedirectUri("http://localhost")
            .Build();

            //Invoking Graph API
            string[] scopes = { "user.read" };
            AuthenticationResult result = await app.AcquireTokenInteractive(scopes).ExecuteAsync();
            Console.WriteLine($"Token:\t{result.AccessToken}");

            //string endpoint = "https://graph.microsoft.com/v1.0/me";
            var client = new HttpClient();
            var authHeader = new AuthenticationHeaderValue("Bearer", result.AccessToken);
            client.DefaultRequestHeaders.Authorization = authHeader;
            var response = await client.GetAsync(endpoint);
            string json = await response.Content.ReadAsStringAsync();
            Console.WriteLine(json);

            //Custom API
            string[] scopes1 = { "https://prasantibogyarihotmail.onmicrosoft.com/DssDemoApiApp3456/user_impersonation" };
            AuthenticationResult result1 = await app.AcquireTokenInteractive(scopes1).ExecuteAsync();
            var client1 = new HttpClient();
            var authHeader1 = new AuthenticationHeaderValue("Bearer", result1.AccessToken);
            client1.DefaultRequestHeaders.Authorization = authHeader1;
            string endpoint1 = "https://localhost:44398/weatherforecast"; //URL of WebAPI
            var response1 = await client1.GetAsync(endpoint1);
            string json1 = await response1.Content.ReadAsStringAsync();
            Console.WriteLine(json1);
        }

    }
}
