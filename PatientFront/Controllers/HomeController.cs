using Microsoft.AspNetCore.Mvc;
using PatientFront.Models;
using System.Diagnostics;

namespace PatientFront.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>Front Home Controller. Error method.
        /// Displays errors. Particularly for error codes
        /// 401 (unauthorized), 403 (forbidden), 404 (not found), 415</summary> 
        /// <param name="statusCode">Error code.</param>
        /// <returns>Error View</returns> URI: "Home/Error/{statusCode}"</remarks>
        [Route("Home/Error/{statusCode}")]
        public async Task<IActionResult> Error(int statusCode)
        {
            if (statusCode == 404)
            {                
                TempData["ErrorTitle"] = "404 - Page Not Found";
                TempData["ErrorMessage"] = "Les identifiants saisis ne sont pas trouv�s. Veuillez v�rifier utilisateur et mot de passe.";   
                return View("Views/Shared/404.cshtml");
            }
            else if (statusCode == 401)
            {
                TempData["ErrorTitle"] = "401 - Unauthorized";
                TempData["ErrorMessage"] = "Vous n'�tes pas authentifi�. Vous ne pouvez pas � acc�der � cette page.";
                return await Task.FromResult(View("404"));
            } else if (statusCode == 403)
            {
                TempData["ErrorTitle"] = "403 - Forbidden";
                TempData["ErrorMessage"] = "Vous n'�tes pas autoris� (token). Vous ne pouvez pas � acc�der � cette page.";
                return await Task.FromResult(View("404"));
            } else if (statusCode == 415)
            {
                TempData["ErrorTitle"] = "415 - Unsupported media type";
                TempData["ErrorMessage"] = "Le serveur refuse d'accepter la requ�te de votre navigateur.";
                return await Task.FromResult(View("404"));
            }
            
            return await Task.FromResult(View("Error"));
            
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
