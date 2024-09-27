using Ocelot.DependencyInjection;
using Ocelot.Middleware;

/*var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
*/

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Ocelot service.
builder.Services.AddOcelot(builder.Configuration);

// Ocelot configuration.
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

WebApplication app = builder.Build();

app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(_ => { });

await app.UseOcelot();
await app.RunAsync();
