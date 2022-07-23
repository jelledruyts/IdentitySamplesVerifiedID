using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using MicrosoftEntra.VerifiedId.Client;
using MicrosoftEntra.VerifiedId.Client.Models;
using RelecloudInstruments.Models;

namespace RelecloudInstruments.Controllers;

[ApiController]
public class PresentationController : ControllerBase
{
    private readonly ILogger<PresentationController> logger;
    private readonly PresentationRequestClient requestClient;
    private readonly AppConfiguration appConfiguration;
    private readonly IDistributedCache distributedCache;
    private readonly DistributedCacheEntryOptions distributedCacheOptions;

    public PresentationController(ILogger<PresentationController> logger, IOptions<AppConfiguration> appConfiguration, PresentationRequestClient requestClient, IDistributedCache distributedCache)
    {
        this.logger = logger;
        this.requestClient = requestClient;
        this.appConfiguration = appConfiguration.Value;
        this.distributedCache = distributedCache;
        this.distributedCacheOptions = new DistributedCacheEntryOptions
        {
            // Keep callback events in cache for 5 minutes maximum, which should be
            // enough for the browser application to retrieve it.
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        };
    }

    [HttpPost("api/presentation/request")]
    public async Task<PresentationApiResponse> PresentationRequest(string type)
    {
        ArgumentNullException.ThrowIfNull(this.appConfiguration.VerifiedCredentialIssuer);

        // Get an absolute URL to the Callback action.
        var absoluteCallbackUrl = Url.Action(nameof(PresentationCallback), null, null, "https")!;

        // Send a presentation request to the Verifiable Credentials Service.
        var credentialType = string.Equals(type, "staff", StringComparison.OrdinalIgnoreCase) ? this.appConfiguration.StaffCredentialType : this.appConfiguration.StudentCredentialType;
        var requestedCredential = new RequestedCredential
        {
            Type = credentialType,
            Purpose = $"Please prove that you have the \"{credentialType}\" credential.",
            AcceptedIssuers = new[] { this.appConfiguration.VerifiedCredentialIssuer }
        };
        var response = await this.requestClient.RequestPresentationAsync(absoluteCallbackUrl, includeQRCode: true, requestedCredentials: new[] { requestedCredential });

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
                var message = default(string);
                if (string.Equals(cachedMessage.Code, VerifiedIdConstants.CallbackCodes.PresentationVerified, StringComparison.OrdinalIgnoreCase))
                {
                    // If the presentation was successfully verified, determine the discount based on the credential type.
                    var credentialTypes = cachedMessage.Issuers.SelectMany(i => i.Type).ToArray();
                    if (credentialTypes.Any(t => string.Equals(t, this.appConfiguration.StaffCredentialType, StringComparison.OrdinalIgnoreCase)))
                    {
                        // Staff get 7% discount.
                        message = "Thank you for proving that you are verified staff, you get a 7% discount!";
                    }
                    else if (credentialTypes.Any(t => string.Equals(t, this.appConfiguration.StudentCredentialType, StringComparison.OrdinalIgnoreCase)))
                    {
                        // Students get 5% discount.
                        message = "Thank you for proving that you are a verified student, you get a 5% discount!";
                    }
                    else
                    {
                        message = "Thank you for proving that you care about verifiable credentials, you get a 3% discount!";
                    }
                }
                return new PresentationStatus
                {
                    Status = cachedMessage.Code,
                    Message = message
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