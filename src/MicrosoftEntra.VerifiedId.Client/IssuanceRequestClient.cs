using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using MicrosoftEntra.VerifiedId.Client.Models;

namespace MicrosoftEntra.VerifiedId.Client;

public class IssuanceRequestClient : BaseRequestClient
{
    private readonly IssuanceRequestClientOptions options;

    public IssuanceRequestClient(ILogger<IssuanceRequestClient> logger, IssuanceRequestClientOptions options, HttpClient httpClient, IConfidentialClientApplication confidentialClientApplication)
    : base(logger, options, httpClient, confidentialClientApplication)
    {
        this.options = options;
    }

    public IssuanceRequestContext GetIssuanceRequest(string credentialType, IDictionary<string, string> claims, string callbackUrl, string? callbackState = null, bool? includeQRCode = null, int? pinLength = null)
    {
        var request = base.GetRequest<IssuanceRequest>(callbackUrl, callbackState, includeQRCode);
        request.Issuance = new Issuance
        {
            Type = credentialType,
            Manifest = GetManifestUrl(credentialType),
            Claims = claims // TODO: This is only used in id_token_hint flows?
        };
        var context = new IssuanceRequestContext(request);

        // Check if a PIN was either explicitly requested or otherwise statically configured.
        var requestedPinLength = pinLength.HasValue ? pinLength.Value : (this.options.PinLength.HasValue ? this.options.PinLength.Value : 0);
        if (requestedPinLength > 0)
        {
            var pinValue = RandomNumberGenerator.GetInt32(1, (int)Math.Pow(10, requestedPinLength));
            var pinValueString = string.Format("{0:D" + requestedPinLength + "}", pinValue);
            context.PinValue = pinValueString;
            request.Issuance.Pin = new Pin
            {
                Length = requestedPinLength,
                Value = pinValueString
            };
        }

        return context;
    }

    public async Task<IssuanceRequestContext> RequestIssuanceAsync(string credentialType, IDictionary<string, string> claims, string callbackUrl, string? callbackState = null, bool? includeQRCode = null, int? pinLength = null)
    {
        var context = GetIssuanceRequest(credentialType, claims, callbackUrl, callbackState, includeQRCode, pinLength);
        context.Response = await RequestIssuanceAsync(context.Request);
        return context;
    }
    
    public Task<IssuanceResponse> RequestIssuanceAsync(IssuanceRequest request)
    {
        return base.SendRequestAsync<IssuanceRequest, IssuanceResponse>(request);
    }
}