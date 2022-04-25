using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using MvcWebApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MvcWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        IConfiguration _config;
        readonly ITokenAcquisition _tokenAcquisition;
        public HomeController(ILogger<HomeController> logger, IConfiguration config, ITokenAcquisition tokenAcquisition)
        {
            _configuration = config;
            _tokenAcquisition = tokenAcquisition;
        }


        public IActionResult Index()
        {
            return View();
        }

        [AuthorizeForScopes(Scopes = new[] { "https://prasantibogyarihotmail.onmicrosoft.com/MyWebAPI/user_impersonation" })]
        public async Task<IActionResult> WeatherForecast()
        {
            HttpClient httpClient = new HttpClient();
            string[] scopes = new string[] { "https://prasantibogyarihotmail.onmicrosoft.com/MyWebAPI/user_impersonation" };
            string token = await _tokenAcquisition.GetAccessTokenForUserAsync(scopes);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            string url = _configuration["ApiUrl"] + "weatherforecast";
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(url)
            };
            IEnumerable<WeatherForecast> cols = null;
            using (var response = httpClient.SendAsync(request).Result)
            {
                cols = JsonConvert.DeserializeObject<IEnumerable<WeatherForecast>>(response.Content.ReadAsStringAsync().Result);
            };
            return View(cols);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize]
        public IActionResult About()
        {
            ViewBag.Name = User.Claims.FirstOrDefault(claim => claim.Type == "name")?.Value;
            ViewBag.Username = User.Claims.FirstOrDefault(claim => claim.Type == "preferred_username")?.Value;
            ViewBag.TenantId = User.Claims.FirstOrDefault(claim => claim.Type == "http://schemas.microsoft.com/identity/claims/tenantid")?.Value;
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
