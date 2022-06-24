namespace MicrosoftEntra.VerifiedId
{
    public class RequestClientOptions
    {
        public string? Instance { get; set; } = "https://beta.did.msidentity.com/v1.0/"; // For Europe, use "https://beta.eu.did.msidentity.com/v1.0/".
        public string? Authority { get; set; } // The longform Decentralized Identifier (DID) of the issuer.
        public string? WellKnownDidJsonContents { get; set; }
        public string? WellKnownDidJsonConfigurationContents { get; set; }
        public string? TenantId { get; set; }
        public string? ClientName { get; set; }
    }
}