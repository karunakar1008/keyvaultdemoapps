using Microsoft.Identity.Client;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DeamonConsoleApp
{
    class Program
    {
        private const string _clientId = "8f7cc168-24b6-4305-a61e-61025d2b25be";
        private const string _tenantId = "0c44cc27-a1cd-4b20-9c98-607c791fc563";
        private const string _secret = "gqD8Q~1lRGK.a0svrb3t9fkTc3_kAaPWeEZ_vaej";

        public static async Task Main(string[] args)
        {
            IConfidentialClientApplication app = ConfidentialClientApplicationBuilder
                                                 .Create(_clientId)
                                                 .WithAuthority(AzureCloudInstance.AzurePublic, _tenantId)
                                                 .WithClientSecret(_secret)
                                                 .Build();

            //Invoking Microsoft Graph API
            string[] scopes = { "https://graph.microsoft.com/.default" };
            AuthenticationResult result = await app.AcquireTokenForClient(scopes).ExecuteAsync();
            Console.WriteLine($"Token:\t{result.AccessToken}");

            string endpoint = "https://graph.microsoft.com/v1.0/users";
            var client = new HttpClient();
            var authHeader = new AuthenticationHeaderValue("Bearer", result.AccessToken);
            client.DefaultRequestHeaders.Authorization = authHeader;
            var response = await client.GetAsync(endpoint);
            string json = await response.Content.ReadAsStringAsync();
            Console.WriteLine(json);


            //Invoding Custom Web API
            var client1 = new HttpClient();
            string[] scopes1 = { "https://prasantibogyarihotmail.onmicrosoft.com/DssDemoApiApp3456/.default" };
            var result1 = await app.AcquireTokenForClient(scopes1).ExecuteAsync();
            client1 = new HttpClient();
            var authHeader1 = new AuthenticationHeaderValue("Bearer", result1.AccessToken);
            client1.DefaultRequestHeaders.Authorization = authHeader1;
            string endpoint1 = "https://localhost:44384/weatherforecast"; //URL of WebAPI

            var response1 = await client1.GetAsync(endpoint1);
            string json1 = await response1.Content.ReadAsStringAsync();
            Console.WriteLine(json1);

        }
    }
}
