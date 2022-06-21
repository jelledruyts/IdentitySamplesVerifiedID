using Microsoft.AspNetCore.Mvc;
using MicrosoftEntra.VerifiedId;
using MicrosoftEntra.VerifiedId.Models;

namespace ContosoMusiversity.Controllers;

public class WellKnownDidConfigurationController : ControllerBase
{
    private readonly DidConfiguration didConfiguration;

    public WellKnownDidConfigurationController(RequestClientOptions options)
    {
        this.didConfiguration = new DidConfiguration();
        if (options.DomainLinkageCredentials != null)
        {
            this.didConfiguration.LinkedDids = options.DomainLinkageCredentials;
        }
    }

    [Route(".well-known/did-configuration.json")]
    public DidConfiguration GetDidConfiguration()
    {
        return this.didConfiguration;
    }
}