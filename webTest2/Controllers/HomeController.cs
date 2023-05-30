using AuthenticationLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace webTest2.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            var scope = "https://graph.microsoft.com/user.read";
            var redirectUri = Request.Scheme + "://" + Request.Host + "/Home/ProcessToken";
            var clientId = _configuration["AzureAd:ClientId"];
            var clientSecret = _configuration["AzureAd:ClientSecret"];

            var authUrl = AzureADAuthenticator.GetAuthorizationUrl(scope, redirectUri, clientId, clientSecret);
            Console.WriteLine(authUrl);

            return Redirect(authUrl);
        }

        public ActionResult Success()
        {
            return View();
        }

        public ActionResult ProcessToken(string code, string error=null)
        {
            Console.WriteLine("Authentication code");
            if (error != null)
            {
                // Handle the error
                Console.WriteLine($"Authentication error: {error}");
                return RedirectToAction("Index");
            }
            Console.WriteLine($"Authentication code: {code}");
            var scope = "https://graph.microsoft.com";
            var redirectUri = "https://localhost:7056/Home/ProcessToken";
            var clientId = _configuration["AzureAd:ClientId"];
            var clientSecret = _configuration["AzureAd:ClientSecret"];

            var accessToken = AzureADAuthenticator.ProcessAuthorizationCode(code, scope, redirectUri, clientId, clientSecret);

            // Use the access token in 'accessToken' to call the Microsoft Graph API

            return RedirectToAction("Success");
        }
    }
}
