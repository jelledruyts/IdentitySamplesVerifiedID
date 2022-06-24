using Microsoft.AspNetCore.Mvc;
using MicrosoftEntra.VerifiedId.Client;

namespace ContosoMusiversity.Controllers;

[ApiController]
public class IssuerController : ControllerBase
{
    private readonly ILogger<IssuerController> logger;

    public IssuerController(ILogger<IssuerController> logger, RequestClient requestClient)
    {
        this.logger = logger;
    }

    // [Authorize] // TODO: Ensure user is logged in
    [HttpPost("api/issuer/issuance-request")]
    public string IssuanceRequest()
    {
        return "ok";
    }

    [HttpPost("api/issuer/issuance-callback")]
    public string IssuanceCallback()
    {
        return "ok";
    }

    [HttpPost("api/issuer/issuance-response")]
    public string IssuanceResponse()
    {
        return "ok";
    }
}