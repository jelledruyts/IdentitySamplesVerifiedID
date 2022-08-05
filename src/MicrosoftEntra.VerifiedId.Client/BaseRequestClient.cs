using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
        ArgumentNullException.ThrowIfNull(this.options.TenantId);
        // The contract identifier is the Base64 encoding of the concatenated tenant ID and credential type.
        var contractId = Convert.ToBase64String(Encoding.UTF8.GetBytes(this.options.TenantId.ToLowerInvariant() + credentialType.ToLowerInvariant())).TrimEnd('=');
        return GetApiUrl($"tenants/{this.options.TenantId}/verifiableCredentials/contracts/{contractId}/manifest");
    }

    public abstract string GetRequestUrl();

    public bool ValidateCallbackRequest(HttpRequest request)
    {
        if (string.IsNullOrEmpty(this.options.ApiKey))
        {
            // No API key was configured, so no need to check it on the callback.
            return true;
        }

        if (request.Headers.TryGetValue(VerifiedIdConstants.CallbackHeaderNames.ApiKey, out var apiKeyHeaders))
        {
            // Validate that the callback request has the configured API key sent back.
            if (string.Equals(this.options.ApiKey, apiKeyHeaders, StringComparison.Ordinal))
            {
                return true;
            }
        }
        this.logger.LogDebug("Invalid callback request, required API key is missing");
        return false;
    }

    protected TRequest GetRequest<TRequest>(string callbackUrl, string? callbackState = null, bool? includeQRCode = null) where TRequest : BaseRequest, new()
    {
        // Callback state is required, but if the caller didn't require any then we just generate a new GUID.
        callbackState = string.IsNullOrWhiteSpace(callbackState) ? Guid.NewGuid().ToString() : callbackState;
        var request = new TRequest
        {
            IncludeQRCode = includeQRCode ?? false,
            Callback = new Callback
            {
                Url = callbackUrl,
                State = callbackState
            },
            Authority = this.options.DidAuthority,
            Registration = new Registration
            {
                ClientName = this.options.ClientName
            }
        };
        if (!string.IsNullOrEmpty(this.options.ApiKey))
        {
            request.Callback.Headers.Add(VerifiedIdConstants.CallbackHeaderNames.ApiKey, this.options.ApiKey);
        }
        return request;
    }

    protected async Task<TResponse> SendRequestAsync<TRequest, TResponse>(TRequest request) where TRequest : BaseRequest where TResponse : BaseResponse
    {
        var url = GetRequestUrl();
        this.httpClient.DefaultRequestHeaders.Authorization = await GetAuthorizationHeader();
        this.logger.LogDebug($"Calling Verifiable Credentials Service at \"{url}\" with callback URL \"{request.Callback?.Url}\"");
        var responseMessage = await this.httpClient.PostAsJsonAsync(url, request, jsonSerializerOptions);
        using var responseStream = await responseMessage.Content.ReadAsStreamAsync();
        if (responseMessage.IsSuccessStatusCode)
        {
            var response = await JsonSerializer.DeserializeAsync<TResponse>(responseStream, jsonSerializerOptions);
            if (response != null)
            {
                this.logger.LogDebug($"Request was accepted with Request ID \"{response.RequestId}\"");
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
            this.logger.LogError(errorMessage);
            throw new VerifiedIdException(errorResponse, errorMessage);
        }

        // Throw default exception in case of other failures.
        errorMessage += " " + errorContent;
        this.logger.LogError(errorMessage);
        throw new HttpRequestException(errorMessage);
    }

    private async Task<AuthenticationHeaderValue> GetAuthorizationHeader()
    {
        var token = await this.confidentialClientApplication.AcquireTokenForClient(VerifiableCredentialsServiceRequestScopes).ExecuteAsync();
        return new AuthenticationHeaderValue(token.TokenType, token.AccessToken);
    }

    protected string GetApiUrl(string api)
    {
        ArgumentNullException.ThrowIfNull(this.options.DidInstance);
        return $"{this.options.DidInstance.TrimEnd('/')}/{api}";
    }
}