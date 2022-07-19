namespace MicrosoftEntra.VerifiedId.Client.Models;

/// <summary>
/// A request to present and verify a verifiable credential.
/// </summary>
public class PresentationRequest : BaseRequest
{
    /// <summary>
    /// Provides information about the presentation request.
    /// </summary>
    public Presentation? Presentation { get; set; } = new Presentation();
}