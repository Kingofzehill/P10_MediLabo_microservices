using Serilog;

namespace PatientDiabeteRiskBackAPI.Services
{
    public class MiddlewareService
    {
        private readonly RequestDelegate _httpResponse;

        public MiddlewareService(RequestDelegate httpResponse)
        {
            _httpResponse = httpResponse;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _httpResponse(context);

                // errors management: 401 (unauthorized) // 403 (forbidden) // 404 (notfound) // 415 (unsupportedmediatype)
                if (context.Response.StatusCode == StatusCodes.Status415UnsupportedMediaType || context.Response.StatusCode == StatusCodes.Status404NotFound || context.Response.StatusCode == StatusCodes.Status403Forbidden || context.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    Log.Error("[PatientDiabeteRiskBackAPI][MiddlewareService] Response error, StatusCode: {context.Response.StatusCode}", context.Response.StatusCode);                    
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "[PatientDiabeteRiskBackAPI][MiddlewareService] An error occurs.");
                Log.Error($"{ex.StackTrace} : {ex.Message}");
            }
        }
    }
}
