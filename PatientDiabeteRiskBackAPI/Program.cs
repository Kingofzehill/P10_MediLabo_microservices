using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// Swagger : Open API Bearer http security scheme configuration.
// https://medium.com/@rahman3593/implementing-jwt-authentication-with-swagger-ca991b7aca08
builder.Services.AddSwaggerGen(options =>
{
    options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    options.SwaggerDoc("v1", new()
    {
        Title = "MediLabo PatientDiabeteRiskBackAPI",
        Version = "v1",
        Description = "An ASP.NET Core Web API for managing Patient Notes."
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Entrer le token communiqué lors de votre authentification (login) pour avoir l'autorisation.",
        Scheme = "Bearer",
        Name = "Authorization",
        BearerFormat = "JWT",
        Type = SecuritySchemeType.Http
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new List<string>()
        }
    });
    // Swagger API xml documentation.
    /*
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";    
    //options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    //FileStream fs = new FileStream(@"" + offlinePath, FileMode.Create);
    var repFic = "C:\\Users\\smour\\source\\repos\\OCR\\Prj10";
    var fichier = File.Create(repFic, 512, FileOptions.None);
    options.IncludeXmlComments(Path.Combine("C:\\Users\\smour\\source\\repos\\OCR\\Prj10", xmlFilename));
    */
});

builder.Services.AddHttpContextAccessor();

// Add http client to PatientBackAPI microservice for API methods access.
builder.Services.AddHttpClient<PatientDiabeteRiskBackAPI.Services.PatientService>(serviceProvider =>
{
    serviceProvider.BaseAddress = new Uri("https://localhost:7243"); // URL from PatientBackAPIlaunchSettings.json.
});

// Add http client to PatientNoteBackAPI microservice for API methods access.
builder.Services.AddHttpClient<PatientDiabeteRiskBackAPI.Services.PatientNoteService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7079"); // URL from PatientNoteBackAPI launchSettings.json.
});

// Authentication with secretKey for token generation.
var jwt = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwt["SecretKey"]!);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
        };
    });

// Authorization using Practitioner policy
//      https://learn.microsoft.com/en-us/aspnet/core/security/authorization/policies?view=aspnetcore-8.0
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("Practitioner", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireRole("Practitioner");
        policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
    });

// DB connection for Identity.
builder.Services.AddDbContext<IdentityDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Patient-Back")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
        .AddEntityFrameworkStores<IdentityDbContext>()
        .AddDefaultTokenProviders();

// Logs configuration (Serilog).
// https://serilog.net/ 
// https://www.nuget.org/packages/Serilog.Sinks.File 
// https://www.nuget.org/packages/Serilog.Sinks.Console 
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/MediLabo_PatientDiabeteRiskBackAPI_log.txt", rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
    .CreateLogger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// (UPD022) Midlleware Service.
app.UseMiddleware<PatientDiabeteRiskBackAPI.Services.MiddlewareService>();

app.MapControllers();

app.Run();
