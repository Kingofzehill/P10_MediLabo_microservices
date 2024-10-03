using Ocelot.DependencyInjection;
using Ocelot.Middleware;

/*var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
*/

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Ocelot configuration.
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
// Ocelot service.
builder.Services.AddOcelot(builder.Configuration);

//WebApplication app = builder.Build();
var app = builder.Build();

/*app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(_ => { });*/

await app.UseOcelot();
app.Run();
