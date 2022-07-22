using ContosoMusiversity.Models;
using Microsoft.AspNetCore.Mvc;

namespace ContosoMusiversity.Controllers;

[ApiController]
public class ConfigurationController : ControllerBase
{
    private readonly AuthenticationConfiguration authenticationConfiguration;

    public ConfigurationController(IConfiguration configuration)
    {
        // Create authentication configuration information for the browser app so
        // it doesn't have to be hardcoded and is taken from the same configuration
        // store as the server side.
        this.authenticationConfiguration = new AuthenticationConfiguration
        {
            ClientId = configuration.GetValue<string>("EntraVerifiedId:ClientId"),
            Authority = configuration.GetValue<string>("EntraVerifiedId:Instance") + configuration.GetValue<string>("EntraVerifiedId:TenantId")
        };
    }

    [HttpGet("api/configuration/authentication")]
    public AuthenticationConfiguration Authentication()
    {
        return this.authenticationConfiguration;
    }
}