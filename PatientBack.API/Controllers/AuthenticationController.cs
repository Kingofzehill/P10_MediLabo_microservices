using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PatientBack.API.Services;
using PatientBack.API.Models;
using Serilog;
using Microsoft.AspNetCore.Authorization;

namespace PatientBack.API.Controllers
{    
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public AuthenticationController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        /// <summary>User login which generates a token if successfull.</summary> 
        /// <param name="loginModel">User input model object.</param>
        /// <returns>Generated token.</returns> 
        /// <remarks>Login controller Login method. 
        /// URI: /Authentication/Login.</remarks>
        /// <response code ="200">OK.</response>
        /// <response code ="401">Unauthorized.</response>
        /// <response code ="404">NotFoundResult.</response>                
        /// <response code ="500">Internal error (exception).</response>
        [HttpPost]
        [AllowAnonymous]
        [Route("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]        
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            Log.Information("[HttpPost] Authentication/Login. User {UserName} authentification.", loginModel.Username);
            try
            {
                var token = await _loginService.Login(loginModel.Username, loginModel.Password);
                if (token != "")
                {
                    Log.Information("User authentified.");
                    return Ok(new { value = token });                    
                }
                Log.Warning("Not found (404).", loginModel.Username);
                return NotFound();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Internal error (500) occurs.", loginModel.Username);
                Log.Error($"{ex.StackTrace} : {ex.Message}");
                return StatusCode(500, "Internal error occurs.");
            }            
        }
    }
}
