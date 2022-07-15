namespace MicrosoftEntra.VerifiedId.Client.Models;

/// <summary>
/// The information posted to the callback endpoint.
/// </summary>
public class CallbackEventMessage
{
    /// <summary>
    /// Mapped to the original request when the payload was posted to the Verifiable Credentials service.
    /// </summary>
    public string? RequestId { get; set; }

    /// <summary>
    /// The code returned when the request has an error. Possible values:
    /// "request_retrieved": The user scanned the QR code or selected the link that starts the issuance flow.
    /// "issuance_successful": The issuance of the verifiable credentials was successful.
    /// "Issuance_error": There was an error during issuance. For details, see the error property.
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// Returns the state value that you passed in the original payload.
    /// </summary>
    public string? State { get; set; }

    /// <summary>
    /// When the code property value is "Issuance_error", this property contains information about the error.
    /// </summary>
    public ErrorDetail? Error { get; set; }
}