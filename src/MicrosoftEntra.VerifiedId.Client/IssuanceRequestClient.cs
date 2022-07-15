using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using MicrosoftEntra.VerifiedId.Client.Models;

namespace MicrosoftEntra.VerifiedId.Client;

// TODO: Create IIssuanceRequestClient for cleaner DI?
public class IssuanceRequestClient
{
    private static readonly JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
    private static string[] VerifiableCredentialsServiceRequestScopes = new[] { "3db474b9-6a0c-4840-96ac-1fceb342124f/.default" }; // This is the fixed scope which is required for the "Verifiable Credentials Service Request" app permission.

    private ILogger logger;
    private readonly IssuanceRequestClientOptions options;
    private readonly HttpClient httpClient;
    private readonly IConfidentialClientApplication confidentialClientApplication;

    public IssuanceRequestClient(ILogger<IssuanceRequestClient> logger, IssuanceRequestClientOptions options, HttpClient httpClient, IConfidentialClientApplication confidentialClientApplication)
    {
        this.logger = logger;
        this.options = options;
        this.httpClient = httpClient;
        this.confidentialClientApplication = confidentialClientApplication;
    }

    public async Task<IssuanceRequestContext> RequestIssuanceAsync(string credentialType, IDictionary<string, string> claims, string callbackUrl, string callbackState)
    {
        var context = GetIssuanceRequest(credentialType, claims, callbackUrl, callbackState);
        context.Response = await RequestIssuanceAsync(context.Request);
        return context;
    }

    public IssuanceRequestContext GetIssuanceRequest(string credentialType, IDictionary<string, string> claims, string callbackUrl, string callbackState)
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
            Authority = this.options.DidAuthority,
            Registration = new Registration
            {
                ClientName = this.options.ClientName
            },
            Issuance = new Issuance
            {
                Type = credentialType,
                Manifest = GetManifestUrl(credentialType),
                Claims = claims // TODO: This is only used in id_token_hint flows?
            }
        };
        var context = new IssuanceRequestContext(request);
        if (this.options.PinLength.HasValue && this.options.PinLength.Value > 0)
        {
            var pinValue = RandomNumberGenerator.GetInt32(1, (int)Math.Pow(10, this.options.PinLength.Value));
            var pinValueString = string.Format("{0:D" + this.options.PinLength + "}", pinValue);
            context.PinValue = pinValueString;
            request.Issuance.Pin = new Pin
            {
                Length = this.options.PinLength.Value,
                Value = pinValueString
            };
        }
        return context;
    }

    public async Task<IssuanceResponse> RequestIssuanceAsync(IssuanceRequest request)
    {
        var url = GetRequestIssuanceUrl();
        this.httpClient.DefaultRequestHeaders.Authorization = await GetAuthorizationHeader();
        var responseMessage = await this.httpClient.PostAsJsonAsync(url, request, jsonSerializerOptions);
        using var responseStream = await responseMessage.Content.ReadAsStreamAsync();
        if (responseMessage.IsSuccessStatusCode)
        {
            var response = await JsonSerializer.DeserializeAsync<IssuanceResponse>(responseStream, jsonSerializerOptions);
            if (response != null)
            {
                return response;
            }
        }

        var errorMessage = "There was an error calling the Verifiable Credentials Service.";
        var errorContent = await responseMessage.Content.ReadAsStringAsync();
        var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(errorContent, jsonSerializerOptions);
        if (errorResponse?.Error != null)
        {
            // Throw a specific exception in case there was a service-level error.
            errorMessage += $" Request ID: \"{errorResponse.RequestId}\". {errorResponse.Error.GetErrorMessage()}";
        }
        else
        {
            // Throw default exception in case of other failures.
            errorMessage += " " + errorContent;
        }
        this.logger.LogError(errorMessage);
        throw new HttpRequestException(errorMessage);
    }

    public string GetManifestUrl(string credentialType)
    {
        return GetApiUrl("verifiableCredential/contracts/" + HttpUtility.UrlPathEncode(credentialType));
    }

    public string GetRequestIssuanceUrl()
    {
        return GetApiUrl("verifiablecredentials/request");
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