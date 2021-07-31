using Microsoft.Azure.KeyVault;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Threading.Tasks;

namespace AzureKeyvaultdemoapp
{
    class Program
    {
        //AIM: 
        //Key Vault is using Azure AD authentication that requires Azure AD security principal to grant access
        //We cannot access the secret from Azure key vault directly! Then how can we access the secrets in our application?
        async static Task Main(string[] args)
        {
            //step1: Create app in app registration and get applicationID or client ID
            string client_ID = "02f7b0be-eb43-4f7c-9b5b-ec7cd117ca31";

            //Step 2: Add client secret in the app created in prevous step and copu secret value
            string client_Secret = "Vp_r2Kr.hSyZG6PXB7~P1p3F94Q8BxRom.";

            //step 3: Create Key vault
            string keyvault_baseURI = "https://testkeyvaultkpg-demo.vault.azure.net/";

            //Step 4:  select Access policies and add a new Access policy select Secret Management from configure
            
            //Step 5: Add secret in keyvault secrets, secret key name: testsecretkey

            //Step 6: Add .net code to read secrets

            var client = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(
                async (string auth, string rest, string scope) =>
                {
                    var authContext = new AuthenticationContext(auth);
                    var credentials = new ClientCredential(client_ID, client_Secret);
                    AuthenticationResult result = await authContext.AcquireTokenAsync(rest, credentials);
                    if (result == null)
                    {
                        throw new InvalidOperationException("Failed to retrive token");

                    }
                    return result.AccessToken;
                }
                ));

            var secretData = await client.GetSecretAsync(keyvault_baseURI, "testsecretkey");
            Console.WriteLine("Secret :"+secretData.Value);
            Console.WriteLine("Hello World!");
        }
    }
}
