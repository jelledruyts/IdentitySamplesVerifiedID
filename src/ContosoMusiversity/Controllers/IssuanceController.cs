using System.Text.Json;
using ContosoMusiversity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using MicrosoftEntra.VerifiedId.Client;
using MicrosoftEntra.VerifiedId.Client.Models;

namespace ContosoMusiversity.Controllers;

[ApiController]
public class IssuanceController : ControllerBase
{
    private const string RolesClaimType = "roles";
    private const string ObjectIdClaimType = "oid";
    private readonly ILogger<IssuanceController> logger;
    private readonly IssuanceRequestClient requestClient;
    private readonly AppConfiguration appConfiguration;
    private readonly IDistributedCache distributedCache;
    private readonly DistributedCacheEntryOptions distributedCacheOptions;

    public IssuanceController(ILogger<IssuanceController> logger, IssuanceRequestClient requestClient, IOptions<AppConfiguration> appConfiguration, IDistributedCache distributedCache)
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

    [Authorize]
    [HttpPost("api/issuance/request")]
    public async Task<IssuanceApiResponse> IssuanceRequest([FromBody] IssuanceApiRequest request)
    {
        ArgumentNullException.ThrowIfNull(this.appConfiguration.StaffCredentialType);
        ArgumentNullException.ThrowIfNull(this.appConfiguration.StudentCredentialType);

        // Get an absolute URL to the Callback action.
        var absoluteCallbackUrl = Url.Action(nameof(IssuanceCallback), null, null, "https")!;

        // Determine the credential type to issue based on the user's role.
        // If no roles are present, assume the user is a student anyway so that configuration of
        // the sample can be kept simple without requiring app roles to be defined and assigned.
        var isStaff = this.appConfiguration.StaffAppRoleName != null && this.User.HasClaim(RolesClaimType, this.appConfiguration.StaffAppRoleName);
        var credentialType = isStaff ? this.appConfiguration.StaffCredentialType : this.appConfiguration.StudentCredentialType;

        // Define the claims that will be part of the issued credential.
        var claims = default(Dictionary<string, string>);
        if (isStaff)
        {
            // The Verified Staff credential uses the id_token flow, which means the application should not
            // send any claims but they will be taken from the id_token as part of the login which is triggered
            // when acquiring the verifiable credential.
            claims = null;
        }
        else
        {
            // The Verified Student credential uses the id_token_hint flow, which means the application
            // that requests the issuance must send the claims to include in the verifiable credential.
            claims = new Dictionary<string, string>();
            foreach (var verifiedCredentialInputClaim in this.appConfiguration.VerifiedCredentialInputClaims)
            {
                var userClaim = this.User.Claims.FirstOrDefault(c => string.Equals(c.Type, verifiedCredentialInputClaim, StringComparison.OrdinalIgnoreCase));
                if (userClaim != null)
                {
                    claims[verifiedCredentialInputClaim] = userClaim.Value;
                }
            }
        }

        // If a PIN code was requested, use 4 digits.
        var pinLength = request.UsePinCode ? (int?)4 : null;

        // Send an issuance request to the Verifiable Credentials Service.
        var callbackState = GetUserObjectId(); // Use the user's object id as the callback state to correlate back with the status polling requests.
        var context = await this.requestClient.RequestIssuanceAsync(credentialType, absoluteCallbackUrl, claims: claims, callbackState: callbackState, includeQRCode: true, pinLength: pinLength);

        return new IssuanceApiResponse(context);
    }

    private string GetUserObjectId()
    {
        var userObjectIdClaim = this.User.Claims.Single(c => string.Equals(c.Type, ObjectIdClaimType, StringComparison.OrdinalIgnoreCase));
        return userObjectIdClaim.Value;
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

    [Authorize]
    [HttpGet("api/issuance/status")]
    public async Task<IActionResult> IssuanceResponse(string requestId)
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
                // For added security, check that the currently logged in user is the same user that
                // requested the issuance for this requestId, based on the user's object id which was
                // set as the callback state.
                if (cachedMessage.State != GetUserObjectId())
                {
                    return Unauthorized();
                }

                // If the credential was successfully issued, return the credential details to the client.
                return Ok(new IssuanceStatus
                {
                    Status = cachedMessage.Code,
                    Message = cachedMessage.Error?.Message
                });
            }
        }

        // No callback was received, which means the request is still pending.
        return Ok(new IssuanceStatus
        {
            Status = "pending"
        });
    }
}