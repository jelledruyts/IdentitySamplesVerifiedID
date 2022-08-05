using System.Collections.Generic;

namespace MicrosoftEntra.VerifiedId.Client.Models;

/// <summary>
/// A request to present and verify a verifiable credential.
/// </summary>
public class PresentationRequest : BaseRequest
{
    /// <summary>
    /// Determines whether a receipt should be included in the response of this request.
    /// Possible values are true or false (default). The receipt contains the original
    /// payload sent from the authenticator to the Verifiable Credentials service. The
    /// receipt is useful for troubleshooting, and shouldn't be set by default. In the
    /// OpenId Connect SIOP request, the receipt contains the ID token from the original
    /// request.
    /// </summary>
    public bool IncludeReceipt { get; set; }

    /// <summary>
    /// Provides information about the requested credentials the user needs to provide.
    /// </summary>
    public IList<RequestedCredential> RequestedCredentials { get; set; } = new List<RequestedCredential>();
}