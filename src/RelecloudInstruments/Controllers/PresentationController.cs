using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using MicrosoftEntra.VerifiedId.Client;
using MicrosoftEntra.VerifiedId.Client.Models;
using RelecloudInstruments.Models;

namespace RelecloudInstruments.Controllers;

[ApiController]
public class PresentationController : ControllerBase
{
    private readonly ILogger<PresentationController> logger;
    private readonly PresentationRequestClient requestClient;
    private readonly IDistributedCache distributedCache;
    private readonly DistributedCacheEntryOptions distributedCacheOptions;

    public PresentationController(ILogger<PresentationController> logger, PresentationRequestClient requestClient, IDistributedCache distributedCache)
    {
        this.logger = logger;
        this.requestClient = requestClient;
        this.distributedCache = distributedCache;
        this.distributedCacheOptions = new DistributedCacheEntryOptions
        {
            // Keep callback events in cache for 5 minutes maximum, which should be
            // enough for the browser application to retrieve it.
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        };
    }

    [HttpPost("api/presentation/request")]
    public async Task<PresentationApiResponse> PresentationRequest()
    {
        // Get an absolute URL to the Callback action.
        var absoluteCallbackUrl = Url.Action(nameof(PresentationCallback), null, null, "https")!;

        // Send a presentation request to the Verifiable Credentials Service.
        // TODO: Button per presentation type (?type=student/staff).
        // TODO: Don't use RequestedCredentials in config but simplify with just issuer-did, student and staff credential type.
        var response = await this.requestClient.RequestPresentationAsync(absoluteCallbackUrl, includeQRCode: true);

        return new PresentationApiResponse(response);
    }

    [HttpPost("api/presentation/callback")]
    public async Task<IActionResult> PresentationCallback(PresentationCallbackEventMessage message)
    {
        this.logger.LogInformation($"Presentation callback received for request \"{message.RequestId}\": {message.Code}");

        // Validate the callback request (e.g. if an API key was configured).
        if (!this.requestClient.ValidateCallbackRequest(this.Request))
        {
            return Unauthorized();
        }

        // Store the message in the distributed cache so it can be retrieved by the browser app.
        await this.distributedCache.SetStringAsync(message.RequestId, JsonSerializer.Serialize(message), this.distributedCacheOptions);

        return Ok();
    }

    [HttpGet("api/presentation/status")]
    public async Task<PresentationStatus> PresentationStatus(string requestId)
    {
        // See if any callback message has arrived in the background, in which case it would
        // have been added to the distributed cache.
        var cachedMessageString = await this.distributedCache.GetStringAsync(requestId);
        if (cachedMessageString != null)
        {
            // Note: we could remove the message from the cache at this point, but it will disappear
            // automatically as it has been given an expiration time.
            var cachedMessage = JsonSerializer.Deserialize<PresentationCallbackEventMessage>(cachedMessageString);
            if (cachedMessage != null)
            {
                // If the presentation was successfully verified, return the credential details to the client.
                // Only consider the first issuer for simplicity as this sample doesn't request multiple issuers.
                // TODO: API sets discount depending on (configured) type, not browser app.
                var issuer = cachedMessage.Issuers.FirstOrDefault();
                return new PresentationStatus
                {
                    Status = cachedMessage.Code,
                    CredentialTypes = issuer?.Type,
                };
            }
        }

        // No callback was received, which means the request is still pending.
        return new PresentationStatus
        {
            Status = "pending"
        };
    }
}