using System.Collections.Generic;

namespace MicrosoftEntra.VerifiedId
{
    public class RequestClientOptions
    {
        public string? Instance { get; set; } = "https://beta.did.msidentity.com/v1.0/"; // For Europe, use "https://beta.eu.did.msidentity.com/v1.0/".
        public string? Authority { get; set; } // The longform Decentralized Identifier (DID) of the issuer.
        public IList<string>? DomainLinkageCredentials { get; set; } // The "linked_dids" JWT entries to include in the ".well-known/did-configuration.json" file for domain verification.
        public string? TenantId { get; set; }
        public string? ClientName { get; set; }
    }
}