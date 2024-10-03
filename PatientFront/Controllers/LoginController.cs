using Microsoft.AspNetCore.Mvc;
using PatientFront.Services;
using PatientBackAPI.Models; // For LoginModel use.
using Serilog;

namespace PatientFront.Controllers
{
    public class LoginController : Controller
    {
        private readonly AuthenticationService _authenticationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginController(AuthenticationService authenticationService, IHttpContextAccessor httpContextAccessor)
        {
            _authenticationService = authenticationService;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>PatientFront. Authentication controller. Index method.</summary>  
        /// <returns>LoginModel view.</returns> 
        /// <remarks>[HttpGet]</remarks>
        [HttpGet]
        public IActionResult Index()
        {
            return View(new LoginModel());
        }

        [HttpPost]        
        public async Task<IActionResult> Login([FromForm] LoginModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {                    
                    var token = await _authenticationService.Login(model.Username, model.Password);
                    if (token != null && token != "")
                    {
                        // JWT dans le header
                        var cookieOptions = new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true,
                            Expires = DateTime.UtcNow.AddDays(1) // Définir la durée de vie du cookie
                        };
                        _httpContextAccessor.HttpContext.Response.Cookies.Append("Jwt", token, cookieOptions);

                        // Message de connexion avec le nom de l'utilisateur
                        TempData["AuthenticationOK"] = $"Authentification réussie : {model.Username}.";

                        return RedirectToAction("Index", "Home");
                    }
                    ModelState.AddModelError(string.Empty, "Login failed.");
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "An error occurred during the login process.");
                    return StatusCode(500, "Internal server error");
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            try
            {
                // Récupérer le token JWT depuis le cookie
                var token = HttpContext.Request.Cookies["Jwt"];
                var userName = "Utilisateur";

                if (!string.IsNullOrEmpty(token))
                {
                    userName = await _authenticationService.ReadToken(token);
                }

                HttpContext.Session.Clear();

                // Supprimer le cookie JWT
                if (HttpContext.Request.Cookies["Jwt"] != null)
                {
                    var cookieOptions = new CookieOptions
                    {
                        Expires = DateTime.UtcNow.AddDays(-1)
                    };
                    HttpContext.Response.Cookies.Append("Jwt", "", cookieOptions);
                }

                TempData["AuthenticationOK"] = $"Déconnexion réussie : {userName}.";

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred during the logout process.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
