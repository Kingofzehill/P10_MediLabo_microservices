using PatientNoteBackAPI.Models.InputModels;
using PatientNoteBackAPI.Models.OutputModels;
using Serilog;
using System.Net;
using System.Net.Http.Headers;

namespace PatientFront.Services
{
    /// <summary>PatientNoteService class provide access to PatientNoteBackAPI for Patient Notes API methods.</summary> 
    /// <remarks>Use HttpClient defined for accessing Patient Notes API methods:
    /// - List.    
    /// - Create.</remarks>
    public class PatientNoteService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<PatientNoteService> _logger;

        public PatientNoteService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, ILogger<PatientNoteService> logger)
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

        /// <summary>Front PatientNote Service. List method.
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
                    Log.Error($"[PatientFront][PatientNoteService][List] Unauthorized(401) or Forbidden(403)  on Patient Notes List, Patient id: {id}, statusCode: {response.StatusCode}.");
                    return null;
                }

                response.EnsureSuccessStatusCode();
                var notes = await response.Content.ReadFromJsonAsync<List<NoteOutputModel>>();
                return notes;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"[PatientFront][PatientNoteService][List] Error on Patient Notes List.");
                Log.Error($"{ex.StackTrace} : {ex.Message}");                
                return null;
            }
        }

        /// <summary>Front PatientNote Service. Create method.
        /// Use PatientNoteNoteAPI API for Patient Note Create.</summary>             
        /// <param name="input">Patient Note Input Model.</param>    
        /// <returns>Patient Note Output Model.</returns> 
        /// <remarks> URI: /Note/Create.</remarks>
        public async Task<NoteOutputModel> Create(NoteInputModel input)
        {
            try
            {
                // Call Patient Note Create method from PatientNoteBackAPI.
                var response = await _httpClient.PostAsJsonAsync("/Note/Create", input);

                if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
                {
                    Log.Warning($"[PatientFront][PatientNoteService][Create] Unauthorized(401) or Forbidden(403) error code on Patient Note Create, statusCode: {response.StatusCode}.");
                    return null;
                }
                response.EnsureSuccessStatusCode();

                var note = await response.Content.ReadFromJsonAsync<NoteOutputModel>();
                return note;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"[PatientFront][PatientNoteService][Create] Error on Patient Note Create.");
                Log.Error($"{ex.StackTrace} : {ex.Message}");
                return null;
            }
        }
    }
}
