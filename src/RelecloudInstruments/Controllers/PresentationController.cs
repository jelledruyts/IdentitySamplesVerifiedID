using Microsoft.AspNetCore.Mvc;
using MicrosoftEntra.VerifiedId.Client;
using MicrosoftEntra.VerifiedId.Client.Models;
using RelecloudInstruments.Models;

namespace RelecloudInstruments.Controllers;

[ApiController]
public class PresentationController : ControllerBase
{
    private readonly ILogger<PresentationController> logger;
    private readonly PresentationRequestClient requestClient;

    public PresentationController(ILogger<PresentationController> logger, PresentationRequestClient requestClient)
    {
        this.logger = logger;
        this.requestClient = requestClient;
    }

    // [Authorize] // TODO: Ensure user is logged in
    [HttpPost("api/presentation/request")]
    public async Task<PresentationApiResponse> PresentationRequest()
    {
        // Get an absolute URL to the Callback action.
        var absoluteCallbackUrl = Url.Action(nameof(PresentationCallback), null, null, "https")!;
        var response = await this.requestClient.RequestPresentationAsync(absoluteCallbackUrl, includeQRCode: true);
        return new PresentationApiResponse(response);
    }

    [HttpPost("api/presentation/callback")]
    public IActionResult PresentationCallback(PresentationCallbackEventMessage message)
    {
        this.logger.LogInformation($"Presentation callback received for request \"{message.RequestId}\": {message.Code}");
        if (!this.requestClient.ValidateCallbackRequest(this.Request))
        {
            return Unauthorized();
        }
        return Ok();
    }

    [HttpPost("api/presentation/response")] // TODO: Rename to poll?
    public IActionResult PresentationResponse()
    {
        return Ok();
    }
}