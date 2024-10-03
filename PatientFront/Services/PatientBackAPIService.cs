using System.Net.Http.Headers;

namespace PatientFront.Services
{
    /// <summary>PatientBackAPIService class to PatienBack API for Patient API methods.</summary> 
    /// <remarks>Use HttpClient defined for accessing Patient API methods:
    /// - List.
    /// - Get.
    /// - Create.
    /// - Update.
    /// - Delete.</remarks>
    public class PatientBackAPIService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<PatientBackAPIService> _logger;

        public PatientBackAPIService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, ILogger<PatientBackAPIService> logger)
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;

            // Récupérer le token JWT depuis le cookie
            var token = _httpContextAccessor.HttpContext.Request.Cookies["Jwt"];
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }
    }
}
