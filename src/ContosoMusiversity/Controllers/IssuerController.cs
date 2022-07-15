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
    public async Task<IssuanceApiResponse> IssuanceRequest()
    {
        var absoluteCallbackUrl = Url.Action(nameof(IssuanceCallback), null, null, "https")!;
        var claims = new Dictionary<string, string>();
        claims.Add("user_name", "john@doe.com");
        claims.Add("given_name", "John");
        claims.Add("family_name", "Doe");
        var context = await this.requestClient.RequestIssuanceAsync("Verified Student", claims, absoluteCallbackUrl, "TODO");
        return new IssuanceApiResponse(context);
    }

    [HttpPost("api/issuer/issuance-callback")]
    public IActionResult IssuanceCallback(CallbackEventMessage message)
    {
        this.logger.LogInformation($"Issuance callback received for request \"{message.RequestId}\": {message.Code}");
        if (message.Error != null)
        {
            // "badOrMissingField": unspecified_error
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