namespace MicrosoftEntra.VerifiedId.Client.Models;

/// <summary>
/// A request to issue a new verifiable credential.
/// </summary>
public class IssuanceRequest : BaseRequest
{
    /// <summary>
    /// Provides information about the issuance request.
    /// </summary>
    public Issuance Issuance { get; set; } = new Issuance();
}