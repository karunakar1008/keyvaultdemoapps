using Microsoft.Azure.KeyVault;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
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

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}