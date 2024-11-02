using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
using PatientBackAPI.Data;
using PatientBackAPI.Repositories;
using PatientBackAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// (UPD007) DbContext configuration with "Patient-back". 
builder.Services.AddDbContext<LocalDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("Patient-back")));

// (UPD008) Identity configuration.
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
       .AddEntityFrameworkStores<LocalDbContext>()
       .AddDefaultTokenProviders();

// (UPD009) Swagger : Open API Bearer http security scheme configuration.
// https://medium.com/@rahman3593/implementing-jwt-authentication-with-swagger-ca991b7aca08
// JWT configuration from appsettings.json.
var jwt = builder.Configuration.GetSection("Jwt");
// Get SecretKey for token generation.
var key = Encoding.ASCII.GetBytes(jwt["SecretKey"]);
builder.Services.AddSwaggerGen(options =>
{
    options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    options.SwaggerDoc("v1", new()
    {
        Title = "MediLabo PatientBackAPI",
        Version = "v1",
        Description = "An ASP.NET Core Web API for managing Patients."
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

// (UPD011) JWT Bearer Authentication configuration with Secret Key to use for token generation.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // (FIX001) solve sharing authentication between microservices.
        //options.Authority = "https://localhost:7244"; // PatientBackAPI microservice.
        //options.Audience = "https://localhost:7244"; // PatientBackAPI microservice.
        
        // https not required.
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        // Token validation settings.
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            // Mitigates forwarding attacks (ValidateIssuer / ValidateAudience).
            ValidateIssuer = false,//ValidateIssuer = true,
            ValidateAudience = false,//ValidateAudience = true,
            ValidateLifetime = true,
            // Valid issuer and audience for token check.
            //ValidIssuer = jwt["Issuer"],
            //ValidAudience = jwt["Audience"],
            // Set security key to use.
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

// (UPD012) Authorization policies for Organizer and Practitioner.
//      https://learn.microsoft.com/en-us/aspnet/core/security/authorization/policies?view=aspnetcore-8.0
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("Organizer", policy =>
    {        
        policy.RequireAuthenticatedUser();
        policy.RequireRole("Organizer");        
        policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
    })
    .AddPolicy("Practitioner", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireRole("Practitioner");        
        policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
    })

    .AddPolicy("OrganizerOrPractitioner", policy =>
    {
        policy.RequireAuthenticatedUser();
        // Policy which check roles Organizer OR Practitioner for authorization (could also be done through Claims).
        //      https://stackoverflow.com/questions/35609632/asp-net-5-authorize-against-two-or-more-policies-or-combined-policy
        //      https://github.com/dotnet/AspNetCore.Docs/issues/27761
        policy.RequireAssertion(context =>
            context.User.IsInRole("Organizer") || context.User.IsInRole("Practitioner"));
        policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
    });

// (UPD013) application logs configuration (Serilog).
// https://serilog.net/ 
// https://www.nuget.org/packages/Serilog.Sinks.File 
// https://www.nuget.org/packages/Serilog.Sinks.Console 
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/MediLabo_PatientBackAPI_log.txt", rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
    .CreateLogger();

// (UPD014) HttpContextAccessor implementation ==> encapsulates all information of HTTP request and response.
// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-context?view=aspnetcore-8.0 
builder.Services.AddHttpContextAccessor();

// (UPD016) Scoped services for Patient, Address, Login..
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IPatientService, PatientBackAPI.Services.PatientService>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<ILoginService, PatientBackAPI.Services.LoginService>();

//FIXRUN02 force https.
/* not working
builder.Services.AddHttpsRedirection(options =>
{    
    options.HttpsPort = 7244;
});
builder.WebHost.UseUrls("https://localhost:7244");//builder.WebHost.UseUrls("http://localhost:5033", "https://localhost:7244");*/

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{    
    // (UPD017) Service provider for DbContext, Login, Identity users and roles.
    var dbcontext = scope.ServiceProvider.GetRequiredService<LocalDbContext>();
    var authService = scope.ServiceProvider.GetService<PatientBackAPI.Services.LoginService>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    // Check if DB exists.
    var succeed = dbcontext.Database.EnsureCreated();

    // (UPD018) Admin, Organizer and Practitioner users. 
    if (!succeed)
    {
        var usersAndRoles = new DBUsersAndRoles(userManager, roleManager);
        await usersAndRoles.Admin();
        await usersAndRoles.Organizer();
        await usersAndRoles.Practitioner();
    }
}


/*// FIXRUN01 app.UseCors(builder =>
{
    builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
});*/

app.UseHttpsRedirection();
//app.UseStaticFiles();
//app.UseRouting();
app.UseAuthorization();
app.UseAuthentication();
//app.UseEndpoints(_ => { });

app.MapControllers();
app.Run();
