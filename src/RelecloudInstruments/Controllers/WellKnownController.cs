using Microsoft.AspNetCore.Mvc;
using MicrosoftEntra.VerifiedId.Client;

namespace RelecloudInstruments.Controllers;

public class WellKnownController : ControllerBase
{
    private readonly IssuanceRequestClientOptions options;

    public WellKnownController(IssuanceRequestClientOptions options)
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