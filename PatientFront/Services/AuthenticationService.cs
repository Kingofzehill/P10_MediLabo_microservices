using PatientBackAPI.Domain;
using PatientBackAPI.Services;
using Serilog;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PatientFront.Services
{
    // Use interface of PatientBackAPI/Services.
    public class AuthenticationService : ILoginService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AuthenticationService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationService(HttpClient httpClient, ILogger<AuthenticationService> logger, IHttpContextAccessor httpContextAccessor)
        {
            // PatientBackAPI.
            // For local development.            
            //httpClient.BaseAddress = new Uri("https://localhost:7244");
            
            // Use API DNS for containerized app. Port provides by configuration.
            var endPoint = "https://patientbackapi"; 

            // FIXRUN06 dev-cert https certificate are not correctly handled by docker containers.
            // In order to avoid generating an official certificate with letsencrypt (for example) we force to TRUE the certificate validation.
            // Recommanded instructions from Microsoft in Enforce HTTPS in ASP.NET Core page was already made ==> https://learn.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?view=aspnetcore-5.0&tabs=visual-studio
            //      https://www.conradakunga.com/blog/disable-ssl-certificate-validation-in-net/
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
            {
                return true;
            };
            httpClient = new HttpClient(httpClientHandler) { BaseAddress = new Uri(endPoint) };
            // END: Use API DNS for containerized app. Port provides by configuration.

            httpClient.DefaultRequestHeaders.Accept.Clear();
            // Set Content-Type header for an HttpClient request : application/json
            //      https://www.dofactory.com/code-examples/csharp/content-type-header
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient = httpClient;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;            
        }

        /// <summary>Front Authentication Service. Connection method.
        /// Use PatientBack API for authentication using http client defined.</summary> 
        /// <param name="username">Username for authentication.</param>
        /// <param name="password">Password for authentication.</param>
        /// <returns>Generated token.</returns> 
        /// <remarks>Autentication controller Login method from PatientBackAPI. 
        /// URI: /Authentication/Login.</remarks>
        public async Task<string> Login(string username, string password)
        {
            try
            {
                //ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                
                // Use HttpClient defined to PatientBack API authentication method, route "/Authentication/Login".
                // Transmits username and password for login and check code 200 (success).
                var connection = await _httpClient.PostAsJsonAsync("/Authentication/Login",
                    new { Username = username, Password = password });
                connection.EnsureSuccessStatusCode();

                // Get token from the response.
                var responseContent = await connection.Content.ReadAsStringAsync();
                var tokenObject = JsonSerializer.Deserialize<Dictionary<string, string>>(responseContent);
                var token = tokenObject["value"];

                // Configure authorization.
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                return token;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[PatientFront][AuthenticationService] Error on Login.");
                Log.Error($"{ex.StackTrace} : {ex.Message}");
            }

            return string.Empty;
        }

        /// <summary>Front Authentication Service. ReadToken method.
        /// Get username from token for user logout.</summary> 
        /// <param name="token">Token returned by Login method.</param>
        /// <returns>Username claim.</returns> 
        public async Task<string> ReadToken(string token)
        {
            return await Task.Run(() =>
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var userNameClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "unique_name");

                return userNameClaim?.Value ?? "Utilisateur";
            });
        }
    }

}
