using Microsoft.AspNetCore.Http;

namespace PatientFront.Services
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
            await _httpResponse(context);

            // errors management: 401 (unauthorized) // 403 (forbidden) // 404 (notfound) // 415 (unsupportedmediatype)
            if (context.Response.StatusCode == StatusCodes.Status415UnsupportedMediaType || context.Response.StatusCode == StatusCodes.Status404NotFound || context.Response.StatusCode == StatusCodes.Status403Forbidden || context.Response.StatusCode == StatusCodes.Status401Unauthorized)
            {                             
                //context.Response.Redirect("/Home/Error/404");
                switch (context.Response.StatusCode) {
                    case StatusCodes.Status415UnsupportedMediaType:
                        context.Response.Redirect("/Home/Error/415");
                        break;
                    case StatusCodes.Status404NotFound:
                        context.Response.Redirect("/Home/Error/404");
                        break;
                    case StatusCodes.Status403Forbidden:
                        context.Response.Redirect("/Home/Error/403");
                        break;
                    case StatusCodes.Status401Unauthorized:
                        context.Response.Redirect("/Home/Error/401");
                        break;
                }
            }
        }
    }
}
