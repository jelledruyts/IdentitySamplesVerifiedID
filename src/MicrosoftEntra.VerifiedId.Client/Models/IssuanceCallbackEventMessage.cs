namespace MicrosoftEntra.VerifiedId.Client.Models;

/// <summary>
/// The information posted to the callback endpoint for an issuance request.
/// </summary>
public class IssuanceCallbackEventMessage : BaseCallbackEventMessage
{
    /// <summary>
    /// The code returned when the request has an error. Possible values:
    /// "request_retrieved": The user scanned the QR code or selected the link that starts the issuance flow.
    /// "issuance_successful": The issuance of the verifiable credentials was successful.
    /// "Issuance_error": There was an error during issuance. For details, see the error property.
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// When the code property value is "Issuance_error", this property contains information about the error.
    /// </summary>
    public ErrorDetail? Error { get; set; }
}