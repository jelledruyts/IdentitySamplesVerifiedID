using ContosoMusiversity.Models;
using Microsoft.AspNetCore.Mvc;
using MicrosoftEntra.VerifiedId.Client;
using MicrosoftEntra.VerifiedId.Client.Models;

namespace ContosoMusiversity.Controllers;

[ApiController]
public class IssuerController : ControllerBase
{
    private readonly ILogger<IssuerController> logger;
    private readonly IssuanceRequestClient requestClient;

    public IssuerController(ILogger<IssuerController> logger, IssuanceRequestClient requestClient)
    {
        this.logger = logger;
        this.requestClient = requestClient;
    }

    // [Authorize] // TODO: Ensure user is logged in
    [HttpPost("api/issuer/issuance-request")]
    public async Task<IssuanceApiResponse> IssuanceRequest([FromBody] IssuanceApiRequest request)
    {
        var absoluteCallbackUrl = Url.Action(nameof(IssuanceCallback), null, null, "https")!;
        var claims = new Dictionary<string, string>();
        var credentialType = "Verified Student";
        claims.Add("user_name", "john@doe.com");
        claims.Add("given_name", "John");
        claims.Add("family_name", "Doe");
        var pinLength = request.UsePinCode ? (int?)4 : null;
        var context = await this.requestClient.RequestIssuanceAsync(credentialType, claims, absoluteCallbackUrl, "TODO", pinLength: pinLength, includeQRCode: true);
        return new IssuanceApiResponse(context);
    }

    [HttpPost("api/issuer/issuance-callback")]
    public IActionResult IssuanceCallback(CallbackEventMessage message)
    {
        this.logger.LogInformation($"Issuance callback received for request \"{message.RequestId}\": {message.Code}");
        if (message.Error != null)
        {
            this.logger.LogError(message.Error.GetErrorMessage());
        }
        return Ok();
    }

    [HttpPost("api/issuer/issuance-response")]
    public string IssuanceResponse()
    {
        return "ok";
    }
}