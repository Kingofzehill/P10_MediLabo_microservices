using Serilog;
using System.Net.Http.Headers;
using PatientDiabeteRiskBackAPI.Models;
using PatientBackAPI.Models.OutputModels;

namespace PatientFront.Services
{
    public class PatientDiabeteService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<PatientDiabeteService> _logger;
        public PatientDiabeteService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, ILogger<PatientDiabeteService> logger)
        {
            //(FIX3.3) Baseaddress of PatientDiabeteRiskBackAPI.
            // For development.
            //httpClient.BaseAddress = new Uri("https://localhost:7089");
            // For docker container.
            httpClient.BaseAddress = new Uri("https://patientdiabeteriskbackapi:8081");

            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;

            // Get Token from cookie.
            var token = _httpContextAccessor.HttpContext.Request.Cookies["Jwt"];
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        /// <summary>Front Patient Diabete Service. GetReport method.
        /// Use PatientDiabeteRiskBackAPI for Patient GetReport.</summary>     
        /// <param name="id">Patient id.</param>    
        /// <returns>Risk.</returns> 
        /// <remarks> URI: /Diabete/Get?id={id}.</remarks>
        public async Task<Risk?> GetReport(int id)
        {
            try
            {
                // Call Get method from PatientDiabeteRiskBackAPI.
                var response = await _httpClient.GetAsync($"/Diabete/Get?id={id}");
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized || response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    Log.Error($"[PatientFront][PatientDiabeteService][GetReport] Unauthorized(401) or Forbidden(403) on Get Diabete Report, Patient id: {id}, statusCode: {response.StatusCode}.");
                    return null;
                }

                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Risk?>();                
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"[PatientFront][PatientDiabeteService][GetReport] Error on Get Diabete Report, Patient id: {id}");
                Log.Error($"{ex.StackTrace} : {ex.Message}");
                return null;
            }            
        }
    }
}
