using MicrosoftEntra.VerifiedId.Client;
using RelecloudInstruments;

var builder = WebApplication.CreateBuilder(args);

// Add app configuration.
builder.Services.Configure<AppConfiguration>(builder.Configuration.GetSection("RelecloudInstruments"));

// Add Verified ID issuance services.
builder.Services.AddVerifiedIdPresentation(builder.Configuration.GetSection("EntraVerifiedId"));
builder.Services.AddVerifiedIdWellKnownEndpoints(builder.Configuration.GetSection("EntraVerifiedId"));

// Add a distributed cache for correlating user requests with callback events.
// This sample uses an in-memory cache to keep the dependencies to a minimum,
// but in a production environment you would need an actual distributed cache
// as this will not work if there are multiple instances of the app, and the
// cache won't survive a restart of the app.
// See https://docs.microsoft.com/aspnet/core/performance/caching/distributed.
builder.Services.AddDistributedMemoryCache();

// Add services to the container.
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error-development");
}
else
{
    app.UseExceptionHandler("/error");
}

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

// Expose the "/.well-known/did.json" and "/.well-known/did-configuration.json" endpoints.
app.UseVerifiedIdWellKnownEndpoints();

app.Run();
