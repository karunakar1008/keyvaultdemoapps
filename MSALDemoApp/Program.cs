using Microsoft.Identity.Client;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MSALDemoApp
{
    class Program
    {
        static string _clientId = "dedb6748-772b-4679-998c-3a39a77dec60";
        static string _tenantId = "0c44cc27-a1cd-4b20-9c98-607c791fc563";
        public static async Task Main(string[] args)
        {
            var app = PublicClientApplicationBuilder
                        .Create(_clientId)
                        .WithAuthority(AzureCloudInstance.AzurePublic, _tenantId)
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
            var accounts = await app.GetAccountsAsync();
            AuthenticationResult result2 = await app.AcquireTokenSilent(scopes, accounts.FirstOrDefault()).ExecuteAsync();
            Console.WriteLine($"Silent Token:\t{result2.AccessToken}");
        }
    }
}
