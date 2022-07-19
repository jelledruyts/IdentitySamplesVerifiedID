using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MicrosoftEntra.VerifiedId.Client.Models;

/// <summary>
/// The information posted to the callback endpoint for a presentation request.
/// </summary>
public class PresentationCallbackEventMessage : BaseCallbackEventMessage
{
    /// <summary>
    /// The code returned when the request has an error. Possible values:
    /// "request_retrieved": The user scanned the QR code or selected the link that starts the presentation flow.
    /// "presentation_verified": The verifiable credential validation completed successfully.
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// The verifiable credential user DID.
    /// </summary>
    public string? Subject { get; set; }

    /// <summary>
    /// Returns an array of verifiable credentials requested.
    /// </summary>
    public IList<IssuedCredential>? Issuers { get; set; } = new List<IssuedCredential>();

    /// <summary>
    /// Optional. The receipt contains the original payload sent from the wallet to the Verifiable Credentials service.
    /// The receipt should be used for troubleshooting/debugging only. The format in the receipt is not fix and can
    /// change based on the wallet and version used.
    /// </summary>
    public JsonElement? Receipt { get; set; }
}