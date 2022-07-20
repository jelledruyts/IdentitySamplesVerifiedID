using ContosoMusiversity.Models;
using Microsoft.AspNetCore.Mvc;
using MicrosoftEntra.VerifiedId.Client;
using MicrosoftEntra.VerifiedId.Client.Models;

namespace ContosoMusiversity.Controllers;

[ApiController]
public class IssuanceController : ControllerBase
{
    private readonly ILogger<IssuanceController> logger;
    private readonly IssuanceRequestClient requestClient;

    public IssuanceController(ILogger<IssuanceController> logger, IssuanceRequestClient requestClient)
    {
        this.logger = logger;
        this.requestClient = requestClient;
    }

    // [Authorize] // TODO: Ensure user is logged in
    [HttpPost("api/issuance/request")]
    public async Task<IssuanceApiResponse> IssuanceRequest([FromBody] IssuanceApiRequest request)
    {
        // Get an absolute URL to the Callback action.
        var absoluteCallbackUrl = Url.Action(nameof(IssuanceCallback), null, null, "https")!;
        var claims = new Dictionary<string, string>();
        var credentialType = "Verified Student";
        claims.Add("user_name", "john@doe.com");
        claims.Add("given_name", "John");
        claims.Add("family_name", "Doe");
        var pinLength = request.UsePinCode ? (int?)4 : null;
        var context = await this.requestClient.RequestIssuanceAsync(credentialType, claims, absoluteCallbackUrl, includeQRCode: true, pinLength: pinLength);
        return new IssuanceApiResponse(context);
    }

    [HttpPost("api/issuance/callback")]
    public IActionResult IssuanceCallback(IssuanceCallbackEventMessage message)
    {
        this.logger.LogInformation($"Issuance callback received for request \"{message.RequestId}\": {message.Code}");
        if (!this.requestClient.ValidateCallbackRequest(this.Request))
        {
            return Unauthorized();
        }
        if (message.Error != null)
        {
            this.logger.LogError(message.Error.GetErrorMessage());
        }
        return Ok();
    }

    [HttpPost("api/issuance/response")] // TODO: Rename to poll?
    public IActionResult IssuanceResponse()
    {
        return Ok();
    }
}