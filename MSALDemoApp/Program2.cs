using Microsoft.Identity.Client;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MSALDemoApp
{
    class Program2
    {
        static string _clientId = "11100d01-4e5f-4862-9bfd-fe9afb489dd0";
        public static async Task Main(string[] args)
        {
            var app = PublicClientApplicationBuilder
                        .Create(_clientId)
                        .WithAuthority(AzureCloudInstance.AzurePublic, "common")
                        .WithRedirectUri("http://localhost")
                        .Build();

            string[] scopes = { "user.read" };
            AuthenticationResult result = await app.AcquireTokenInteractive(scopes).ExecuteAsync();
            Console.WriteLine($"Token:\t{result.AccessToken}");
            string endpoint = "https://graph.microsoft.com/v1.0/me";

            var client = new HttpClient();
            var authHeader = new AuthenticationHeaderValue("Bearer", result.AccessToken);
            client.DefaultRequestHeaders.Authorization = authHeader;
            var response = await client.GetAsync(endpoint);
            string json = await response.Content.ReadAsStringAsync();
            Console.WriteLine(json);

            //Acquire silent token
            //var accounts = await app.GetAccountsAsync();
            //AuthenticationResult result2 = await app.AcquireTokenSilent(scopes, accounts.FirstOrDefault()).ExecuteAsync();
            //Console.WriteLine($"Silent Token:\t{result2.AccessToken}");
        }
    }
}
