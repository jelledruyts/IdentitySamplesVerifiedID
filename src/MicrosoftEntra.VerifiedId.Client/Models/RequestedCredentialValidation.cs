namespace MicrosoftEntra.VerifiedId.Client.Models;

/// <summary>
/// Provides information about how the presented credentials should be validated.
/// </summary>
public class RequestedCredentialValidation
{
    /// <summary>
    /// Determines if a revoked credential should be accepted. Default is false (it shouldn't be accepted).
    /// </summary>
    public bool AllowRevoked { get; set; }

    /// <summary>
    /// Determines if the linked domain should be validated. Default is true (it should be validated).
    /// Setting this flag to false means you'll accept credentials from an unverified linked domain.
    /// Setting this flag to true means the linked domain will be validated and only verified domains will be accepted.
    /// </summary>
    public bool ValidateLinkedDomain { get; set; } = true;
}