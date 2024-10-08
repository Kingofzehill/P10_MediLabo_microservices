﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PatientBackAPI.Services;
using PatientBackAPI.Models;
using Serilog;
using Microsoft.AspNetCore.Authorization;

namespace PatientBackAPI.Controllers
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
        /// <remarks>Authentication controller Login method. 
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
            Log.Information("[PatientBackAPI][HttpPost] Authentication/Login. User {UserName} authentification.", loginModel.Username);
            try
            {
                var token = await _loginService.Login(loginModel.Username, loginModel.Password);
                if (token != "")
                {
                    Log.Information("[PatientBackAPI] User authentified.");
                    return Ok(new { value = token });                    
                }
                Log.Warning("Not found (404).", loginModel.Username);
                return NotFound();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "[PatientBackAPI] Internal error (500) occurs on {username} authentication.", loginModel.Username);
                Log.Error($"{ex.StackTrace} : {ex.Message}");
                return StatusCode(500, "Internal error occurs.");
            }            
        }
    }
}
