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

        /// <summary>PatientFront. Login controller. Index method.</summary>  
        /// <returns>LoginModel view.</returns> 
        /// <remarks>[HttpGet]</remarks>
        [HttpGet]
        public IActionResult Index()
        {
            return View(new LoginModel());
        }

        /// <summary>PatientFront. Login controller. Login method.</summary>  
        /// <returns>LoginModel view.</returns> 
        /// <remarks>[HttpPost]</remarks>
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
                        var cookieOptions = new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true,
                            // (UPD028.1) Change cookie expiration from "until browser close" to 30 minutes.
                            //Expires = DateTime.UtcNow.AddDays(1) // Cookie lifetime.
                            Expires = DateTimeOffset.UtcNow.AddMinutes(120)
                    };

                        _httpContextAccessor.HttpContext.Response.Cookies.Append("Jwt", token, cookieOptions);                        
                        TempData["AuthenticationOK"] = $"Authentification réussie : {model.Username}.";
                        return RedirectToAction("Index", "Home");
                    }
                    ModelState.AddModelError(string.Empty, "Login failed.");
                }
                catch (Exception ex)
                {
                    Log.Error(ex, $"[PatientFront] An error occurred during user login.");
                    return StatusCode(500, "Internal server error");
                }
            }
            return View(model);
        }

        /// <summary>PatientFront. Login controller. Logout method.</summary>  
        /// <returns>Home Index view.</returns> 
        /// <remarks>[HttpPost]</remarks>
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            try
            {                
                var token = HttpContext.Request.Cookies["Jwt"];
                var userName = "Utilisateur";

                if (!string.IsNullOrEmpty(token))
                {
                    userName = await _authenticationService.ReadToken(token);
                }

                HttpContext.Session.Clear();
                
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
                Log.Error(ex, $"[PatientFront] An error occurred during user logout.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
