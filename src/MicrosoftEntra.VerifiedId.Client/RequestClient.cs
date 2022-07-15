using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using MicrosoftEntra.VerifiedId.Client.Models;

namespace MicrosoftEntra.VerifiedId.Client;

// TODO: Create IRequestClient for cleaner DI?
public class RequestClient
{
    private static readonly JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
    private static string[] VerifiableCredentialsServiceRequestScopes = new[] { "3db474b9-6a0c-4840-96ac-1fceb342124f/.default" }; // This is the fixed scope which is required for the "Verifiable Credentials Service Request" app permission.

    private ILogger logger;
    private readonly RequestClientOptions options;
    private readonly HttpClient httpClient;
    private readonly IConfidentialClientApplication confidentialClientApplication;

    public RequestClient(ILogger<RequestClient> logger, RequestClientOptions options, HttpClient httpClient, IConfidentialClientApplication confidentialClientApplication)
    {
        this.logger = logger;
        this.options = options;
        this.httpClient = httpClient;
        this.confidentialClientApplication = confidentialClientApplication;
    }

    public IssuanceRequest GetIssuanceRequest(string credentialType, string callbackUrl, string callbackState)
    {
        if (this.options.DidAuthority == null) throw new ArgumentNullException(nameof(this.options.DidAuthority));
        var request = new IssuanceRequest
        {
            IncludeQRCode = true,
            Callback = new Callback
            {
                Url = callbackUrl,
                State = callbackState,
                Headers = null // TODO
            },
            Authority = this.options.DidAuthority, // TODO: Config has long-form issuer; demo request body in Portal just says "did:web:kontoso.org" though
            Registration = new Registration
            {
                ClientName = this.options.ClientName
            },
            Issuance = new Issuance
            {
                Type = credentialType,
                Manifest = GetApiUrl("verifiableCredential/contracts/" + WebUtility.UrlEncode(credentialType)),
                Claims = null // TODO: This is used in the B2C sample when there are any self-asserted claims in the rules definition file.
            }
        };
        if (this.options.MinimumPinLength.HasValue && this.options.MinimumPinLength.Value > 0)
        {
            var pinValue = RandomNumberGenerator.GetInt32(1, (int)Math.Pow(10, this.options.MinimumPinLength.Value));
            var pinValueString = string.Format("{0:D" + this.options.MinimumPinLength + "}", pinValue);
            request.Issuance.Pin = new Pin
            {
                Length = this.options.MinimumPinLength.Value,
                Value = pinValueString
            };
        }
        return request;
    }

    public Task RequestIssuanceAsync(string credentialType, string callbackUrl, string callbackState)
    {
        var request = GetIssuanceRequest(credentialType, callbackUrl, callbackState);
        return RequestIssuanceAsync(request);
    }

    public async Task RequestIssuanceAsync(IssuanceRequest request)
    {
        var url = GetApiUrl("verifiablecredentials/request");
        this.httpClient.DefaultRequestHeaders.Authorization = await GetAuthorizationHeader();
        var responseMessage = await this.httpClient.PostAsJsonAsync(url, request, jsonSerializerOptions);
        using var responseStream = await responseMessage.Content.ReadAsStreamAsync();
        if (responseMessage.IsSuccessStatusCode)
        {
            var response = await JsonSerializer.DeserializeAsync<IssuanceResponse>(responseStream, jsonSerializerOptions);
        }
        else
        {
            var errorContent = await responseMessage.Content.ReadAsStringAsync();
            var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(errorContent, jsonSerializerOptions);
            if (errorResponse != null && errorResponse.Error != null)
            {
                // Throw a specific exception in case there was a service-level error.
                var errorMessage = errorResponse.Error.GetErrorMessage("There was an error calling the Verifiable Credentials Service.");
                this.logger.LogError($"{errorMessage} Request ID: {errorResponse.RequestId}.");
                throw new ApplicationException(errorMessage);
            }
            else
            {
                // Throw default exception in case of other failures.
                this.logger.LogError($"There was an error calling the Verifiable Credentials Service: {errorContent}");
                responseMessage.EnsureSuccessStatusCode();
            }
        }
    }

    private async Task<AuthenticationHeaderValue> GetAuthorizationHeader()
    {
        var token = await this.confidentialClientApplication.AcquireTokenForClient(VerifiableCredentialsServiceRequestScopes).ExecuteAsync();
        return new AuthenticationHeaderValue(token.TokenType, token.AccessToken);
    }

    private string GetApiUrl(string api)
    {
        if (this.options.DidInstance == null) throw new ArgumentNullException(nameof(this.options.DidInstance));
        if (this.options.TenantId == null) throw new ArgumentNullException(nameof(this.options.TenantId));
        return $"{this.options.DidInstance.TrimEnd('/')}/{this.options.TenantId}/{api}";
    }
}