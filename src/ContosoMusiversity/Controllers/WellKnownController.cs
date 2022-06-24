using Microsoft.AspNetCore.Mvc;
using MicrosoftEntra.VerifiedId;

namespace ContosoMusiversity.Controllers;

public class WellKnownController : ControllerBase
{
    private readonly RequestClientOptions options;

    public WellKnownController(RequestClientOptions options)
    {
        this.options = options;
    }

    [Route(".well-known/did.json")]
    public string? GetDid()
    {
        return this.options.WellKnownDidJsonContents;
    }

    [Route(".well-known/did-configuration.json")]
    public string? GetDidConfiguration()
    {
        return this.options.WellKnownDidConfigurationJsonContents;
    }
}