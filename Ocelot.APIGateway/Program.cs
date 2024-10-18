using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

/*var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
*/

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// (TD007) To activate for docker-compose.
/*
// Activate multiple origins requests (cors) for allowing cross origins requests between microservices apps.
//      https://learn.microsoft.com/fr-fr/aspnet/core/security/cors?view=aspnetcore-8.0
builder.Services.AddCors(options =>
{
    var patientFrontUrl = builder.Configuration.GetValue<string>("PatientFrontUrl");
    var patientBackAPIUrl = builder.Configuration.GetValue<string>("PatientBackAPIUrl");
    var PatientNoteAPIUrl = builder.Configuration.GetValue<string>("PatientNoteAPIUrl");
    options.AddPolicy("CorsPolicy",
        policy =>
        {
            if (!string.IsNullOrEmpty(patientFrontUrl))
            {
                policy.WithOrigins(patientFrontUrl);
            }
            if (!string.IsNullOrEmpty(patientBackAPIUrl))
            {
                policy.WithOrigins(patientBackAPIUrl);
            }
            if (!string.IsNullOrEmpty(PatientNoteAPIUrl))
            {
                policy.WithOrigins(PatientNoteAPIUrl);
            }
            policy.AllowAnyMethod();
            policy.AllowAnyHeader();
            policy.AllowCredentials();
        });
});
*/

// (TD007) To activate for docker-compose.
// Health checks: libraries and integrity controls of http endpoints.
//      https://learn.microsoft.com/fr-fr/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-8.0
builder.Services
    .AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);

// Ocelot configuration.
//builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Configuration.AddJsonFile("ocelot.json");
// Ocelot service.
builder.Services.AddOcelot(builder.Configuration);

WebApplication app = builder.Build();
//var app = builder.Build();

//app.UseCors("CorsPolicy");
// (FIX001) solve sharing authentication between microservices.
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(_ => { });

// (TD007) To activate for docker-compose.
// Create health checks endpoints (/hc // /liveness) of the middleware.
//      https://stackoverflow.com/questions/72384646/usehealthchecks-vs-maphealthchecks
// (TD006) using RequireAuthorization??? for avoiding unauthorize client to usurp port.
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
app.Run();
