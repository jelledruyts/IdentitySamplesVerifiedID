using System.Collections.Generic;

namespace MicrosoftEntra.VerifiedId.Client.Models;

/// <summary>
/// A requested verifiable credential.
/// </summary>
public class IssuedCredential
{
    /// <summary>
    /// The issuer's DID.
    /// </summary>
    public string? Issuer { get; set; }

    /// <summary>
    /// The verifiable credential type(s).
    /// </summary>
    public IList<string> Type { get; set; } = new List<string>();

    /// <summary>
    /// The claims retrieved.
    /// </summary>
    public IDictionary<string, string> Claims { get; set; } = new Dictionary<string, string>();

    /// <summary>
    /// The state of the credential.
    /// </summary>
    public CredentialState? CredentialState { get; set; }
}