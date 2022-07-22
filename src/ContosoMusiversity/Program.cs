using System.IdentityModel.Tokens.Jwt;
using ContosoMusiversity;
using Microsoft.Identity.Web;
using MicrosoftEntra.VerifiedId.Client;

var builder = WebApplication.CreateBuilder(args);

// Don't map any standard OpenID Connect claims to Microsoft-specific claims.
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

// Add app configuration.
builder.Services.Configure<AppConfiguration>(builder.Configuration.GetSection("ContosoMusiversity"));

// Add Verified ID issuance services.
builder.Services.AddVerifiedIdIssuance(builder.Configuration.GetSection("EntraVerifiedId"));
builder.Services.AddVerifiedIdWellKnownEndpoints(builder.Configuration.GetSection("EntraVerifiedId"));

// Add a distributed cache for correlating user requests with callback events.
// This sample uses an in-memory cache to keep the dependencies to a minimum,
// but in a production environment you would need an actual distributed cache
// as this will not work if there are multiple instances of the app, and the
// cache won't survive a restart of the app.
// See https://docs.microsoft.com/aspnet/core/performance/caching/distributed.
builder.Services.AddDistributedMemoryCache();

// Add authentication services for the APIs which rely on a user context.
// Reuse the Client ID as the valid Audience as that's what the browser app will request for its access token scope.
builder.Configuration["EntraVerifiedId:Audience"] = builder.Configuration.GetValue<string>("EntraVerifiedId:ClientId");
builder.Services.AddMicrosoftIdentityWebApiAuthentication(builder.Configuration, "EntraVerifiedId");

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
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

// Expose the "/.well-known/did.json" and "/.well-known/did-configuration.json" endpoints.
app.UseVerifiedIdWellKnownEndpoints();

app.Run();
