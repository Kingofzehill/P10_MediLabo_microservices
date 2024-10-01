using PatientFront.Services;
using PatientBack.API.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Serilog;

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
});

builder.Services.AddHttpContextAccessor();


// (UPD023) Add http client to PatientBack.API app Services for login authentication.
builder.Services.AddHttpClient<AuthenticationService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7243"); // URL from PatientBack.API launchSettings.json.
});

// (UPD024) Add http client to PatientBack.API app Services for PatientBack API access.
builder.Services.AddHttpClient<PatientBackAPIService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7243"); // URL from PatientBack.APIlaunchSettings.json.
});

// (UPD026) Add dependency to service ILoginService (interface) from PatientBack.API with AuthenticationService type.
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

// (TODO05) Replace for using Serilog.
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
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

// (UPD022) Midlleware Service for authentication errors management.
app.UseMiddleware<MiddlewareService>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
