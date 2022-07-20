using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using MicrosoftEntra.VerifiedId.Client.Models;

namespace MicrosoftEntra.VerifiedId.Client;

public abstract class BaseRequestClient
{
    private static readonly JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
    private static string[] VerifiableCredentialsServiceRequestScopes = new[] { VerifiedIdConstants.Scopes.VerifiableCredentialsServiceRequestScope };

    private ILogger logger;
    private readonly BaseRequestClientOptions options;
    private readonly HttpClient httpClient;
    private readonly IConfidentialClientApplication confidentialClientApplication;

    public BaseRequestClient(ILogger logger, BaseRequestClientOptions options, HttpClient httpClient, IConfidentialClientApplication confidentialClientApplication)
    {
        this.logger = logger;
        this.options = options;
        this.httpClient = httpClient;
        this.confidentialClientApplication = confidentialClientApplication;
    }

    public string GetManifestUrl(string credentialType)
    {
        return GetApiUrl("verifiableCredential/contracts/" + HttpUtility.UrlPathEncode(credentialType));
    }

    public string GetRequestUrl()
    {
        return GetApiUrl("verifiablecredentials/request");
    }

    protected TRequest GetRequest<TRequest>(string callbackUrl, string? callbackState = null, bool? includeQRCode = null) where TRequest : BaseRequest, new()
    {
        return new TRequest
        {
            IncludeQRCode = includeQRCode ?? false,
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
            }
        };
    }

    protected async Task<TResponse> SendRequestAsync<TRequest, TResponse>(TRequest request) where TRequest : BaseRequest where TResponse : BaseResponse
    {
        var url = GetRequestUrl();
        this.httpClient.DefaultRequestHeaders.Authorization = await GetAuthorizationHeader();
        var responseMessage = await this.httpClient.PostAsJsonAsync(url, request, jsonSerializerOptions);
        using var responseStream = await responseMessage.Content.ReadAsStreamAsync();
        if (responseMessage.IsSuccessStatusCode)
        {
            var response = await JsonSerializer.DeserializeAsync<TResponse>(responseStream, jsonSerializerOptions);
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