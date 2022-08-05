using System.Collections.Generic;

namespace MicrosoftEntra.VerifiedId.Client.Models;

/// <summary>
/// Provides information about the requested credential the user needs to provide.
/// </summary>
public class RequestedCredential
{
    /// <summary>
    /// The verifiable credential type. The type must match the type as defined in the
    /// issuer verifiable credential manifest (for example, VerifiedCredentialExpert).
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// Provide information about the purpose of requesting this verifiable credential.
    /// </summary>
    public string? Purpose { get; set; }

    /// <summary>
    /// A collection of issuers' DIDs that could issue the type of verifiable credential
    /// that subjects can present.
    /// </summary>
    public IList<string> AcceptedIssuers { get; set; } = new List<string>();

    /// <summary>
    /// Optional settings for presentation validation.
    /// </summary>
    public RequestedCredentialConfiguration? Configuration { get; set; }
}