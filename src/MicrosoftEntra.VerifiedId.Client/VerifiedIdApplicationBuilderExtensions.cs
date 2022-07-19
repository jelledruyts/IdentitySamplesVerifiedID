using Microsoft.AspNetCore.Builder;

namespace MicrosoftEntra.VerifiedId.Client;

public static class VerifiedIdApplicationBuilderExtensions
{
    public static IApplicationBuilder UseVerifiedId(this IApplicationBuilder app)
    {
        // TODO: Listen for did.json and did-configuration.json requests?
        // var options = app.ApplicationServices.GetRequiredService<WellKnownOptions>();
        return app;
    }
}