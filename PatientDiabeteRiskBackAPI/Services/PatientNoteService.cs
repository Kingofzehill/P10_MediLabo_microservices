using PatientNoteBackAPI.Models.OutputModels;
using Serilog;
using System.Net;
using System.Net.Http.Headers;

namespace PatientDiabeteRiskBackAPI.Services
{
    public class PatientNoteService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<PatientDiabeteRiskBackAPI.Services.PatientNoteService> _logger;

        public PatientNoteService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, ILogger<PatientDiabeteRiskBackAPI.Services.PatientNoteService> logger)
        {
            // Fix baseaddress defined in httpclient of program.cs for PatientNoteService which etrangly remains at null.
            httpClient.BaseAddress = new Uri("https://localhost:7079");
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
            else
            {
                // For standalone use of the microservice.
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "testtoken");
            }
        }

        /// <summary>PatientDiabeteRiskBackAPI. Patient Note Service. List method.
        /// Use PatientNoteNoteAPI for Patient Notes List.</summary>         
        /// <param name="id">Patient Id.</param>    
        /// <returns>Output model Notes List.</returns> 
        /// <remarks> URI: /Note/List?id={Id}.</remarks>
        public async Task<List<NoteOutputModel>> List(int id)
        {
            try
            {
                // Call Patient Notes List method from PatientNoteBackAPI.
                var response = await _httpClient.GetAsync($"/Note/List?id={id}");
                if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
                {
                    Log.Error($"[PatientDiabeteRiskBackAPI][PatientNoteService][List] Unauthorized(401) or Forbidden(403)  on Patient Notes List, Patient id: {id}, statusCode: {response.StatusCode}.");
                    return null;
                }

                response.EnsureSuccessStatusCode();
                var notes = await response.Content.ReadFromJsonAsync<List<NoteOutputModel>>();
                return notes;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"[PatientDiabeteRiskBackAPI][PatientNoteService][List] Error on Patient Notes List.");
                Log.Error($"{ex.StackTrace} : {ex.Message}");
                return null;
            }
        }

        /// <summary>PatientDiabeteRiskBackAPI. Patient Note Service. Get method.
        /// Use PatientNoteBackAPI for Patient Note Get.</summary>     
        /// <param name="id">Patient Note id (MongoDB ObjectId type).</param>    
        /// <returns>Output model Patient Note.</returns> 
        /// <remarks> URI: /Note/Get?id={Id}.</remarks>
        public async Task<NoteOutputModel> Get(string id)
        {
            try
            {
                // Call Patient Note Get method from PatientNoteBackAPI.
                var response = await _httpClient.GetAsync($"/Note/Get?id={id}");
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized || response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    Log.Error($"[PatientDiabeteRiskBackAPI][PatientNoteService][Get] Unauthorized(401) or Forbidden(403)  on Patient Note Get, ObjectId: {id}, statusCode: {response.StatusCode}.");
                    return null;
                }

                response.EnsureSuccessStatusCode();
                var note = await response.Content.ReadFromJsonAsync<NoteOutputModel>();
                return note;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"[PatientDiabeteRiskBackAPI][PatientNoteService][Get] Error on Patient Note Get, Objectid: {id}");
                Log.Error($"{ex.StackTrace} : {ex.Message}");
                return null;
            }
        }
    }
}
