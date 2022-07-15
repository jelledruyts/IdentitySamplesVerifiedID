namespace MicrosoftEntra.VerifiedId;

public class RequestClientOptions
{
    public string? DidInstance { get; set; } = "https://beta.did.msidentity.com/v1.0/"; // For Europe, use "https://beta.eu.did.msidentity.com/v1.0/".
    public string? DidAuthority { get; set; } // The longform Decentralized Identifier (DID) of the issuer.
    public string? WellKnownDidJsonContents { get; set; }
    public string? WellKnownDidConfigurationJsonContents { get; set; }
    public string? TenantId { get; set; }
    public string? ClientName { get; set; }
    public int? MinimumPinLength { get; set; }
}