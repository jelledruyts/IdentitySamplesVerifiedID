using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace MicrosoftEntra.VerifiedId.Client;

public static class VerifiedIdApplicationBuilderExtensions
{
    /// <summary>
    /// Listen for requests on the ".well-known" endopint for "did.json" and "did-configuration.json".
    /// </summary>
    /// <param name="app">The application builder.</param>
    /// <returns>The application builder.</returns>
    public static IApplicationBuilder UseVerifiedIdWellKnownEndpoints(this IApplicationBuilder app)
    {
        var options = app.ApplicationServices.GetRequiredService<WellKnownEndpointOptions>();
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            if (!string.IsNullOrWhiteSpace(options.WellKnownDidJsonContents))
            {
                endpoints.MapGet("/.well-known/did.json", async context =>
                {
                    await context.Response.WriteAsync(options.WellKnownDidJsonContents);
                });
            }
            if (!string.IsNullOrWhiteSpace(options.WellKnownDidConfigurationJsonContents))
            {
                endpoints.MapGet("/.well-known/did-configuration.json", async context =>
                {
                    await context.Response.WriteAsync(options.WellKnownDidConfigurationJsonContents);
                });
            }
        });
        return app;
    }
}