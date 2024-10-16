using PatientBackAPI.Models.OutputModels;
using Serilog;
using System.Net.Http.Headers;

namespace PatientDiabeteRiskBackAPI.Services
{
    public class PatientService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<PatientService> _logger;

        public PatientService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, ILogger<PatientService> logger)
        {
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

        /// <summary>PatientDiabeteRiskBackAPI. Patient Service. Get method.
        /// Use PatientBackAPI for Patient Get.</summary>     
        /// <param name="id">Patient id.</param>    
        /// <returns>Output model Patient.</returns> 
        /// <remarks> URI: /Patient/Get{id}.</remarks>
        public async Task<PatientOutputModel> Get(int id)
        {
            try
            {
                // Call Patient Get method from PatientBackAPI.
                var response = await _httpClient.GetAsync($"/Patient/Get?id={id}");
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized || response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    Log.Error($"[PatientDiabeteRiskBackAPI][PatientService][Get] Unauthorized(401) or Forbidden(403)  on Patient Get, id: {id}, statusCode: {response.StatusCode}.");
                    return null;
                }

                response.EnsureSuccessStatusCode();
                var patient = await response.Content.ReadFromJsonAsync<PatientOutputModel>();
                return patient;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"[PatientDiabeteRiskBackAPI][PatientService][Get] Error on Patient Get, id: {id}");
                Log.Error($"{ex.StackTrace} : {ex.Message}");
                return null;
            }
        }
    }
}
