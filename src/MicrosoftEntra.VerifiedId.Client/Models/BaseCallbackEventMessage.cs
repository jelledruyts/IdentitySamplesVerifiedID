namespace MicrosoftEntra.VerifiedId.Client.Models;

/// <summary>
/// The information posted to the callback endpoint.
/// </summary>
public class BaseCallbackEventMessage
{
    /// <summary>
    /// Mapped to the original request when the payload was posted to the Verifiable Credentials service.
    /// </summary>
    public string? RequestId { get; set; }

    /// <summary>
    /// Returns the state value that you passed in the original payload.
    /// </summary>
    public string? State { get; set; }
}