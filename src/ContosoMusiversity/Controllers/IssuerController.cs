using Microsoft.AspNetCore.Mvc;
using MicrosoftEntra.VerifiedId.Client;

namespace ContosoMusiversity.Controllers;

[ApiController]
public class IssuerController : ControllerBase
{
    private readonly ILogger<IssuerController> logger;
    private readonly RequestClient requestClient;

    public IssuerController(ILogger<IssuerController> logger, RequestClient requestClient)
    {
        this.logger = logger;
        this.requestClient = requestClient;
    }

    // [Authorize] // TODO: Ensure user is logged in
    [HttpPost("api/issuer/issuance-request")]
    public async Task<string> IssuanceRequest()
    {
        var absoluteCallbackUrl = Url.Action(nameof(IssuanceCallback), null, null, "https")!;
        await this.requestClient.RequestIssuanceAsync("Verified Student", absoluteCallbackUrl, "TODO");
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