using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using MicrosoftEntra.VerifiedId.Client.Models;

namespace MicrosoftEntra.VerifiedId.Client;

public class PresentationRequestClient : BaseRequestClient
{
    private readonly PresentationRequestClientOptions options;

    public PresentationRequestClient(ILogger<PresentationRequestClient> logger, PresentationRequestClientOptions options, HttpClient httpClient, IConfidentialClientApplication confidentialClientApplication)
    : base(logger, options, httpClient, confidentialClientApplication)
    {
        this.options = options;
    }

    public PresentationRequest GetPresentationRequest(string callbackUrl, string? callbackState = null, bool? includeQRCode = null, IList<RequestedCredential>? requestedCredentials = null)
    {
        var request = base.GetRequest<PresentationRequest>(callbackUrl, callbackState, includeQRCode);
        request.Presentation = new Presentation
        {
            IncludeReceipt = this.options.IncludeReceipt,
        };

        // Set the requested credentials from configuration.
        request.Presentation.RequestedCredentials = this.options.RequestedCredentials;

        // Add explicitly requested credentials, if any.
        if (requestedCredentials != null)
        {
            foreach (var requestedCredential in requestedCredentials)
            {
                request.Presentation.RequestedCredentials.Add(requestedCredential);
            }
        }

        return request;
    }

    public Task<PresentationResponse> RequestPresentationAsync(string callbackUrl, string? callbackState = null, bool? includeQRCode = null, IList<RequestedCredential>? requestedCredentials = null)
    {
        var request = GetPresentationRequest(callbackUrl, callbackState, includeQRCode, requestedCredentials);
        return RequestPresentationAsync(request);
    }

    public Task<PresentationResponse> RequestPresentationAsync(PresentationRequest request)
    {
        return base.SendRequestAsync<PresentationRequest, PresentationResponse>(request);
    }
}