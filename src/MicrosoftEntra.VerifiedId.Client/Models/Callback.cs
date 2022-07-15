using System.Collections.Generic;

namespace MicrosoftEntra.VerifiedId.Client.Models;

/// <summary>
/// Allows the developer to asynchronously get information on the flow during
/// the verifiable credential issuance process. For example, the developer might want a
/// call when the user has scanned the QR code or if the issuance request succeeds or fails.
/// </summary>
public class Callback
{
    /// <summary>
    /// URI to the callback endpoint of your application. The URI must point to a reachable
    /// endpoint on the internet otherwise the service will throw callback URL unreadable
    /// error. Accepted formats IPv4, IPv6 or DNS resolvable hostname.
    /// </summary>
    public string? Url { get; set; }

    /// <summary>
    /// Associates with the state passed in the original payload.
    /// </summary>
    public string? State { get; set; }

    /// <summary>
    /// Optional. You can include a collection of HTTP headers required by the receiving end
    /// of the POST message. The current supported header values are the api-key or the
    /// Authorization headers. Any other header will throw an invalid callback header error.
    /// </summary>
    public IDictionary<string, string>? Headers { get; set; } = new Dictionary<string, string>();
}