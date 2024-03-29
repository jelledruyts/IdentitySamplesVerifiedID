namespace MicrosoftEntra.VerifiedId.Client;

public abstract class BaseRequestClientOptions
{
    /// <summary>
    /// The instance or API endpoint for the Verifiable Credentials Service.
    /// </summary>
    public string? DidInstance { get; set; } = "https://verifiedid.did.msidentity.com/v1.0";

    /// <summary>
    /// The Decentralized Identifier (DID) of the issuer.
    /// </summary>
    public string? DidAuthority { get; set; }

    /// <summary>
    /// The Tenant ID of the Azure AD tenant hosting the Verifiable Credentials Service.
    /// </summary>
    public string? TenantId { get; set; }

    /// <summary>
    /// The client name for the Verifiable Credentials Service.
    /// </summary>
    public string? ClientName { get; set; }

    /// <summary>
    /// Optional. The API key that the Verifiable Credentials Service needs to send
    /// back on callback events.
    /// </summary>
    public string? ApiKey { get; set; }
}