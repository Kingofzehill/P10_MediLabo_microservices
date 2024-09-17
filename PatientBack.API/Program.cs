using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PatientBack.API.Data;
using Serilog;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// (UPD009) Swagger : Open API Bearer http security scheme configuration.
// https://medium.com/@rahman3593/implementing-jwt-authentication-with-swagger-ca991b7aca08
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "MediLabo Patient-back API",
        Version = "v1",
        Description = "An ASP.NET Core Web API for managing Patients."
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Entrer Bearer suivi de votre token pour avoir l'autorisation",
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
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

// JWT configuration from appsettings.json.
var jwt = builder.Configuration.GetSection("Jwt");
// Get SecretKey for token generation.
var key = Encoding.ASCII.GetBytes(jwt["SecretKey"]);
// (UPD011) JWT Bearer Authentication configuration with Secret Key to use for token generation.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // https not required.
        options.RequireHttpsMetadata = false; 
        options.SaveToken = true;
        // Token validation settings.
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            // Mitigates forwarding attacks (ValidateIssuer / ValidateAudience).
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            // Valid issuer and audience for token check.
            ValidIssuer = jwt["Issuer"],
            ValidAudience = jwt["Audience"],
            // Set security key to use.
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

// (UPD012) Authorization policies for admin and user.
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("Admin", policy =>
    {
        // should be authenticated.
        policy.RequireAuthenticatedUser();
        policy.RequireRole("Admin");
        policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
    })
    .AddPolicy("User", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireRole("User");
        policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
    })
    .AddPolicy("AdminOrUser", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireAssertion(context =>
            context.User.IsInRole("Admin") || context.User.IsInRole("User"));
        policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
    });

// (UPD007) DbContext configuration with "Patient-back". 
builder.Services.AddDbContext<LocalDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("Patient-back")));

// (UPD008) Identity configuration.
builder.Services.AddIdentity<IdentityUser, IdentityRole<int>>()
       .AddEntityFrameworkStores<LocalDbContext>()
       .AddDefaultTokenProviders();

// (UPD013) application logs configuration (Serilog).
// https://serilog.net/ 
// https://www.nuget.org/packages/Serilog.Sinks.File 
// https://www.nuget.org/packages/Serilog.Sinks.Console 
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/MediLabo_Patient-back_log.txt", rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
    .CreateLogger();

// (UPD014) HttpContextAccessor implementation ==> encapsulates all information
// about an individual HTTP request and response.
// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-context?view=aspnetcore-8.0 
builder.Services.AddHttpContextAccessor();

// (TD001) addscope for interfaces and repositories.

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// (TD002) RequiredService.


app.UseHttpsRedirection();

// (UPD015) Authentication and Authorization Application Pipeline.
app.UseAuthorization(); // User identity.
app.UseAuthentication(); // User authorized.

app.MapControllers();

app.Run();
