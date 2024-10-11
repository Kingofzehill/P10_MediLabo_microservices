using PatientBackAPI.Models.InputModels;
using PatientBackAPI.Models.OutputModels;
using Serilog;
using System.Net.Http.Headers;

namespace PatientFront.Services
{
    /// <summary>PatientService class provide access to PatienBack API for Patient API methods.</summary> 
    /// <remarks>Use HttpClient defined for accessing Patient API methods:
    /// - List.
    /// - Get.
    /// - Create.
    /// - Update.
    /// - Delete.</remarks>
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

        /// <summary>Front Patient Service. List method.
        /// Use PatientBack API for Patient List.</summary>         
        /// <returns>Output model Patients List.</returns> 
        /// <remarks> URI: /Patient/List.</remarks>
        public async Task<List<PatientOutputModel>> List()
        {
            try
            {               
                // Call Patient List method from PatientBackAPI.
                var patients = await _httpClient.GetFromJsonAsync<List<PatientOutputModel>>("/Patient/List");                
                return patients;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"[PatientFront][PatientService][List] Error on Patient List.");
                Log.Error($"{ex.StackTrace} : {ex.Message}");                
                return null;
            }
        }

        /// <summary>Front Patient Service. Get method.
        /// Use PatientBack API for Patient Get.</summary>     
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
                    Log.Error($"[PatientFront][PatientService][Get] Unauthorized(401) or Forbidden(403)  on Patient Get, id: {id}, statusCode: {response.StatusCode}.");
                    return null;
                }

                response.EnsureSuccessStatusCode();
                var patient = await response.Content.ReadFromJsonAsync<PatientOutputModel>();
                return patient;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"[PatientFront][PatientService][Get] Error on Patient Get, id: {id}");
                Log.Error($"{ex.StackTrace} : {ex.Message}");
                return null;
            }
        }

        /// <summary>Front Patient Service. Create method.
        /// Use PatientBack API for Patient Create.</summary>     
        /// <param name="input">Patient Input Model.</param>    
        /// <returns>Patient Output Model.</returns> 
        /// <remarks> URI: /Patient/Create.</remarks>
        public async Task<PatientOutputModel> Create(PatientInputModel input)
        {
            try
            {
                // Call Patient Create method from PatientBackAPI.
                var response = await _httpClient.PostAsJsonAsync("/Patient/Create", input);
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized || response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    Log.Warning($"[PatientFront][PatientService][Create] Unauthorized(401) or Forbidden(403) error code on Patient Create, statusCode: {response.StatusCode}.");
                    return null;
                }
                response.EnsureSuccessStatusCode();

                var patient = await response.Content.ReadFromJsonAsync<PatientOutputModel>();
                return patient;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"[PatientFront][PatientService][Create] Error on Patient Create.");
                Log.Error($"{ex.StackTrace} : {ex.Message}");
                return null;
            }
        }


        /// <summary>Front Patient Service. Update method.
        /// Use PatientBack API for Patient Update.</summary>     
        /// <param name="id">Patient Id.</param> 
        /// <param name="input">Patient Input Model.</param>    
        /// <returns>Patient Output Model.</returns> 
        /// <remarks> URI: /Patient/Update{id}.</remarks>
        public async Task<PatientOutputModel> Update(int id, PatientInputModel input)

        {
            try
            {
                // Call Patient Update method from PatientBackAPI.
                var response = await _httpClient.PutAsJsonAsync($"/Patient/update?id={id}", input);
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized || response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    Log.Warning($"[PatientFront][PatientService][Update] Unauthorized(401) or Forbidden(403) on Patient Update, id:{id}, statusCode: {response.StatusCode}.");
                    return null;
                }

                response.EnsureSuccessStatusCode();
                var patient = await response.Content.ReadFromJsonAsync<PatientOutputModel>();
                return patient;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"[PatientFront][PatientService][Update] Error on Patient Update id: {id}.");
                Log.Error($"{ex.StackTrace} : {ex.Message}");
                return null;
            }
        }

        /// <summary>Front Patient Service. Delete method.
        /// Use PatientBack API for Patient Delete.</summary>     
        /// <param name="id">Patient Id.</param>         
        /// <returns>Patient Output Model.</returns> 
        /// <remarks> URI: /Patient/Delete{id}.</remarks>
        /*public async Task<List<PatientOutputModel>> Delete(int id)*/
        public async Task<PatientOutputModel> Delete(int id)
        {
            try
            {
                // Call Patient Delete method from PatientBackAPI.
                var response = await _httpClient.DeleteAsync($"/Patient/Delete?id={id}");
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized || response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    Log.Warning($"[PatientFront][PatientService][Delete] Unauthorized(401) or Forbidden(403) on Patient Delete, id: {id}, statusCode: {response.StatusCode}.");
                    return null;
                }

                response.EnsureSuccessStatusCode();                
                var patient = await response.Content.ReadFromJsonAsync<PatientOutputModel>();
                return patient;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"[PatientFront][PatientService][Delete] Error on Patient Delete id: {id}.");
                Log.Error($"{ex.StackTrace} : {ex.Message}");
                //return new List<PatientOutputModel>();
                return null;
            }
        }
    }
}
