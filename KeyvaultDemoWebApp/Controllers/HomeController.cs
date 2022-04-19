using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Azure.Storage.Blob;
using System;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace KeyvaultDemoWebApp.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var kv = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(Utils.GetToken));
            var sec1 = await kv.GetSecretAsync(ConfigurationManager.AppSettings["Secreturi"]);
            var key1 = await kv.GetKeyAsync(ConfigurationManager.AppSettings["MyFirstkeyUri"]);
            Console.WriteLine(sec1.Value);
            Console.WriteLine(key1);
            ViewBag.KeyvaultSecretValue = sec1.Value;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            StorageCredentials creds = new StorageCredentials(ConfigurationManager.AppSettings["accountName"], ConfigurationManager.AppSettings["accountKey"]);
            CloudStorageAccount account = new CloudStorageAccount(creds, useHttps: true);
            CloudBlobClient client = account.CreateCloudBlobClient();
            CloudBlobContainer container = client.GetContainerReference(ConfigurationManager.AppSettings["container"]);
            container.CreateIfNotExists();

            // The Resolver object is used to interact with Key Vault for Azure Storage.
            // This is where the GetToken method from above is used.
            KeyVaultKeyResolver cloudResolver = new KeyVaultKeyResolver(Utils.GetToken);
            // Retrieve the key that you created previously.
            // The IKey that is returned here is an RsaKey.
            var rsa = cloudResolver.ResolveKeyAsync("https://kgsapps-kv-development.vault.azure.net/keys/MyFirstKey/", CancellationToken.None).GetAwaiter().GetResult();


            // Now you simply use the RSA key to encrypt by setting it in the BlobEncryptionPolicy.
            BlobEncryptionPolicy policy = new BlobEncryptionPolicy(rsa, null);
            BlobRequestOptions options = new BlobRequestOptions() { EncryptionPolicy = policy };

            // Reference a block blob.
            CloudBlockBlob blob = container.GetBlockBlobReference("MyFile.txt");
            // Upload using the UploadFromStream method.
            using (var stream = System.IO.File.OpenRead(@"F:\Demo.txt"))
                blob.UploadFromStream(stream, stream.Length, null, options, null);

            //In this case, we will not pass a key and only pass the resolver because this policy will only be used for            downloading / decrypting.
            policy = new BlobEncryptionPolicy(null, cloudResolver);
            options = new BlobRequestOptions() { EncryptionPolicy = policy };

            using (var np = System.IO.File.Open(@"F:\MyFileDecrypted.txt", FileMode.Create))
                blob.DownloadToStream(np, null, options, null);

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}