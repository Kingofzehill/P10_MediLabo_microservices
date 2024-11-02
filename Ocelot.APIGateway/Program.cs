using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Activate multiple origins requests (cors) for allowing cross origins requests between microservices apps.
//      https://learn.microsoft.com/fr-fr/aspnet/core/security/cors?view=aspnetcore-8.0
/*//FIXRUN01 builder.Services.AddCors(options =>
{
    //Get microservices url from docker-compose for allowing CORS requests.
    var PatientFrontUrl = builder.Configuration.GetValue<string>("PatientFrontUrl");
    var PatientServiceUrl = builder.Configuration.GetValue<string>("PatientBackAPIUrl");
    var PatientNoteUrl = builder.Configuration.GetValue<string>("PatientNoteBackAPIUrl");
    var PatientRapportDiabeteUrl = builder.Configuration.GetValue<string>("PatientDiabeteRiskBackAPIUrl");
    options.AddPolicy("CorsMediLabo",
        policy =>
        {
            if (!string.IsNullOrEmpty(PatientFrontUrl))
            {
                policy.WithOrigins(PatientFrontUrl);
            }
            if (!string.IsNullOrEmpty(PatientServiceUrl))
            {
                policy.WithOrigins(PatientServiceUrl);
            }
            if (!string.IsNullOrEmpty(PatientNoteUrl))
            {
                policy.WithOrigins(PatientNoteUrl);
            }
            if (!string.IsNullOrEmpty(PatientRapportDiabeteUrl))
            {
                policy.WithOrigins(PatientRapportDiabeteUrl);
            }
            policy.AllowAnyMethod();
            policy.AllowAnyHeader();
            policy.AllowCredentials();
        });
});*/

// Health checks: libraries and integrity controls of http endpoints.
//      https://learn.microsoft.com/fr-fr/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-8.0
builder.Services
    .AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);

// Ocelot service.
builder.Services.AddOcelot(builder.Configuration);
// Ocelot configuration.
builder.Configuration.AddJsonFile("ocelot.json");

//FIXRUN02 (force https).
/*builder.Services.AddHttpsRedirection(options =>
{
    options.HttpsPort = 7196;
});
builder.WebHost.UseUrls("https://localhost:7196"); //builder.WebHost.UseUrls("http://localhost:5236", "https://localhost:7196");*/

WebApplication app = builder.Build();

app.UseHttpsRedirection(); //FIXRUN01 
//app.UseStaticFiles();
app.UseRouting();
//app.UseAuthentication();
//FIXRUN01 app.UseCors("CorsMediLabo");
app.UseAuthorization();
app.UseEndpoints(_ => { });

// Create health checks endpoints (/hc // /liveness) of the middleware.
//      https://stackoverflow.com/questions/72384646/usehealthchecks-vs-maphealthchecks
app.MapHealthChecks("/hc", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
});

app.MapHealthChecks("/liveness", new HealthCheckOptions
{
    Predicate = r => r.Name.Contains("self")
});

await app.UseOcelot();
await app.RunAsync();