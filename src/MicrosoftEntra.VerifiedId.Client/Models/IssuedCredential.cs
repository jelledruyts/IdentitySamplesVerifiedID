using System.Collections.Generic;

namespace MicrosoftEntra.VerifiedId.Client.Models;

/// <summary>
/// A requested verifiable credential.
/// </summary>
public class IssuedCredential
{
    /// <summary>
    /// The verifiable credential type(s).
    /// </summary>
    public IList<string> Type { get; set; } = new List<string>();

    /// <summary>
    /// The verifiable credential issuer's domain.
    /// </summary>
    public string? Domain { get; set; }

    /// <summary>
    /// The verifiable credential issuer's domain validation status.
    /// </summary>
    public string? Verified { get; set; }

    /// <summary>
    /// The issuer's DID.
    /// </summary>
    public string? Issuer { get; set; }

    /// <summary>
    /// The issuer's DID.
    /// </summary>
    public string? Authority { get; set; }

    /// <summary>
    /// The claims retrieved.
    /// </summary>
    public IDictionary<string, string>? Claims { get; set; } = new Dictionary<string, string>();
}