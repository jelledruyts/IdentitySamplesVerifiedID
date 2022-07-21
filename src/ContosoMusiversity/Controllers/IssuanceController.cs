using System.Text.Json;
using ContosoMusiversity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using MicrosoftEntra.VerifiedId.Client;
using MicrosoftEntra.VerifiedId.Client.Models;

namespace ContosoMusiversity.Controllers;

[ApiController]
public class IssuanceController : ControllerBase
{
    private readonly ILogger<IssuanceController> logger;
    private readonly IssuanceRequestClient requestClient;
    private readonly IDistributedCache distributedCache;
    private readonly DistributedCacheEntryOptions distributedCacheOptions;

    public IssuanceController(ILogger<IssuanceController> logger, IssuanceRequestClient requestClient, IDistributedCache distributedCache)
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

    // [Authorize] // TODO: Ensure user is logged in
    [HttpPost("api/issuance/request")]
    public async Task<IssuanceApiResponse> IssuanceRequest([FromBody] IssuanceApiRequest request)
    {
        // Get an absolute URL to the Callback action.
        var absoluteCallbackUrl = Url.Action(nameof(IssuanceCallback), null, null, "https")!;

        // Define the claims that will be part of the issued credential.
        var claims = new Dictionary<string, string>();
        var credentialType = "Verified Student";
        claims.Add("user_name", "john@doe.com");
        claims.Add("given_name", "John");
        claims.Add("family_name", "Doe");

        // If a PIN code was requested, use 4 digits.
        var pinLength = request.UsePinCode ? (int?)4 : null;

        // Send an issuance request to the Verifiable Credentials Service.
        var context = await this.requestClient.RequestIssuanceAsync(credentialType, absoluteCallbackUrl, claims, includeQRCode: true, pinLength: pinLength);

        return new IssuanceApiResponse(context);
    }

    [HttpPost("api/issuance/callback")]
    public async Task<IActionResult> IssuanceCallback(IssuanceCallbackEventMessage message)
    {
        this.logger.LogInformation($"Issuance callback received for request \"{message.RequestId}\": {message.Code}");

        // Validate the callback request (e.g. if an API key was configured).
        if (!this.requestClient.ValidateCallbackRequest(this.Request))
        {
            return Unauthorized();
        }

        // Store the message in the distributed cache so it can be retrieved by the browser app.
        await this.distributedCache.SetStringAsync(message.RequestId, JsonSerializer.Serialize(message), this.distributedCacheOptions);

        if (message.Error != null)
        {
            this.logger.LogError(message.Error.GetErrorMessage());
        }

        return Ok();
    }

    // [Authorize] // TODO: Ensure user is logged in
    [HttpGet("api/issuance/status")]
    public async Task<IssuanceStatus> IssuanceResponse(string requestId)
    {
        // See if any callback message has arrived in the background, in which case it would
        // have been added to the distributed cache.
        var cachedMessageString = await this.distributedCache.GetStringAsync(requestId);
        if (cachedMessageString != null)
        {
            // Note: we could remove the message from the cache at this point, but it will disappear
            // automatically as it has been given an expiration time.
            var cachedMessage = JsonSerializer.Deserialize<IssuanceCallbackEventMessage>(cachedMessageString);
            if (cachedMessage != null)
            {
                // TODO: Check that the currently logged in user requested the issuance for this requestId.
                // Likely this needs the user's oid as the callback state to correlate.
                // If the credential was successfully issued, return the credential details to the client.
                return new IssuanceStatus
                {
                    Status = cachedMessage.Code
                };
            }
        }

        // No callback was received, which means the request is still pending.
        return new IssuanceStatus
        {
            Status = "pending"
        };
    }
}