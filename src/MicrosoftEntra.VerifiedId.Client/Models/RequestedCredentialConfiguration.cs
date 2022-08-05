namespace MicrosoftEntra.VerifiedId.Client.Models;

/// <summary>
/// Settings for presentation validation.
/// </summary>
public class RequestedCredentialConfiguration
{
    /// <summary>
    /// Provides information about how the presented credentials should be validated.
    /// </summary>
    public RequestedCredentialValidation Validation { get; set; } = new RequestedCredentialValidation();
}