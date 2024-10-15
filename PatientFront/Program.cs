using PatientFront.Services;
using PatientBackAPI.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Serilog;
using Microsoft.Extensions.DependencyInjection;
using static System.Net.WebRequestMethods;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddViewOptions(option =>
    {
        option.HtmlHelperOptions.ClientValidationEnabled = true; // client validation.
    });

// (UPD020) Session service.
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    // Should force cookie expiration for avoiding user access to stay alive.
    // options.Cookie.Expiration = TimeSpan.FromMinutes(30);
});

// (UPD028) Change cookie expiration from "until browser close" to 30 minutes
//      https://serverless.industries/2022/03/08/net-core-session-cookie-lifetime.en.html
builder.Services.AddCookiePolicy(opts => {
    opts.CheckConsentNeeded = ctx => false;
    opts.OnAppendCookie = ctx => {
        ctx.CookieOptions.Expires = DateTimeOffset.UtcNow.AddMinutes(30);
    };
});

builder.Services.AddHttpContextAccessor();

// (UPD024) Add http client to PatientBackAPI microservice for API methods access.
builder.Services.AddHttpClient<PatientFront.Services.PatientService>(serviceProvider =>
{
    serviceProvider.BaseAddress = new Uri("https://localhost:7243"); // URL from PatientBackAPIlaunchSettings.json.
});

// (UPD023) Add http client to PatientBackAPI microservice for login authentication method access.
builder.Services.AddHttpClient<PatientFront.Services.AuthenticationService>(serviceProvider =>
{
    serviceProvider.BaseAddress = new Uri("https://localhost:7243"); // URL from PatientBackAPI launchSettings.json.
});

// (UPD028)Add http client to PatientNoteBackAPI microservice for API methods access.
builder.Services.AddHttpClient<PatientFront.Services.PatientNoteService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7079"); // URL from PatientNoteBackAPI launchSettings.json.
});

// (UPD028)Add http client to PatientDiabeteRiskBackAPI microservice for API methods access.
builder.Services.AddHttpClient<PatientFront.Services.PatientDiabeteService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7088"); // URL from PatientDiabeteRiskBackAPI launchSettings.json.
});

// (UPD026) Add dependency to service ILoginService (interface) from PatientBackAPI with AuthenticationService type.
builder.Services.AddScoped<ILoginService, AuthenticationService>();

// (UPD019.Front) Add authentication / httpContextAccessor services (example 18).
// (UPD021) Cookie for Microsoft Asp.Net authentication. 
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
       .AddCookie(options =>
       {
           //options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
           options.LoginPath = "/Authentication/Index";
           //options.AccessDeniedPath = "/error/403";
       });

// (UPD013) application logs configuration (Serilog).
// https://serilog.net/ 
// https://www.nuget.org/packages/Serilog.Sinks.File 
// https://www.nuget.org/packages/Serilog.Sinks.Console 
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/MediLabo_PatientFront_log.txt", rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
    .CreateLogger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // (UPD027) Add middleware for dealing with error statut code (401, 403, 404).
    app.UseStatusCodePagesWithReExecute("/Home/Error{0}");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
// Session and cookie authentication.
app.UseSession();
app.UseAuthentication();

app.UseAuthorization();

// (UPD022) Midlleware Service.
app.UseMiddleware<PatientFront.Services.MiddlewareService>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
