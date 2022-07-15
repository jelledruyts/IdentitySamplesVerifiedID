namespace MicrosoftEntra.VerifiedId.Client.Models;

/// <summary>
/// Provides information about the issuer that can be displayed in the authenticator app.
/// </summary>
public class Registration
{
    /// <summary>
    /// A display name of the issuer of the verifiable credential.
    /// </summary>
    public string? ClientName { get; set; }

    /// <summary>
    /// Optional. The URL for the issuer logo.
    /// </summary>
    public string? LogoUrl { get; set; }

    /// <summary>
    /// Optional. The URL for the terms of use of the verifiable credential that you are issuing.
    /// </summary>
    public string? TermsOfServiceUrl { get; set; }
}