using System.Collections.Generic;
using MicrosoftEntra.VerifiedId.Client.Models;

namespace MicrosoftEntra.VerifiedId.Client;

public class PresentationRequestClientOptions : BaseRequestClientOptions
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
    /// A collection of requested credentials.
    /// </summary>
    public IList<RequestedCredential> RequestedCredentials { get; set; } = new List<RequestedCredential>();
}