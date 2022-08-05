namespace MicrosoftEntra.VerifiedId.Client.Models;

/// <summary>
/// The state of a verifiable credential.
/// </summary>
public class CredentialState
{
    /// <summary>
    /// The revocation status of the credential.
    /// </summary>
    public string? RevocationStatus { get; set; }
}