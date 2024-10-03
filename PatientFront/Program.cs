using PatientFront.Services;
using PatientBackAPI.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Serilog;
using Microsoft.Extensions.DependencyInjection;

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

// Creates a new HttpClient instance as a singleton
//builder.Services.AddSingleton((serviceProvider) => new HttpClient());
/*builder.Services.AddSingleton((serviceProvider) => new HttpClient(new SocketsHttpHandler
{
    // Update the DNS every 60 seconds.
    PooledConnectionLifetime = TimeSpan.FromSeconds(60)//FromMinutes(5)
}));*/

// (UPD024) Add http client to PatientBackAPI app Services for PatientBack API access.
builder.Services.AddHttpClient<PatientBackAPIService>(serviceProvider =>
{
    serviceProvider.BaseAddress = new Uri("https://localhost:7243"); // URL from PatientBackAPIlaunchSettings.json.
});

// (UPD023) Add http client to PatientBackAPI app Services for login authentication.
builder.Services.AddHttpClient<AuthenticationService>(serviceProvider =>
{
    serviceProvider.BaseAddress = new Uri("https://localhost:7243"); // URL from PatientBackAPI launchSettings.json.
});

/*// (UPD023) Add http client to PatientBackAPI app Services for login authentication.
builder.Services.AddHttpClient<AuthenticationService>(serviceProvider =>
{
    serviceProvider.BaseAddress = new Uri("http://localhost:5033"); // URL from PatientBackAPI launchSettings.json.
});*/

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
