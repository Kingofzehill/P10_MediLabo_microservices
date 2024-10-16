using Azure;
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
                if (context.Response.StatusCode != StatusCodes.Status200OK)
                {
                    Log.Error($"[PatientDiabeteRiskBackAPI][MiddlewareService] Http request response error, statusCode: {context.Response.StatusCode}.");
                }
            }
            catch (HttpRequestException ex)
            {                
                Log.Error($"[PatientDiabeteRiskBackAPI][MiddlewareService] HTTP request failed: {ex.Message}");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "[PatientDiabeteRiskBackAPI][MiddlewareService] An error occurs.");
                Log.Error($"{ex.StackTrace} : {ex.Message}");
            }
        }
    }
}
