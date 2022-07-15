namespace MicrosoftEntra.VerifiedId.Client.Models;

// TODO: Inherit from RequestBase which has common properties between Issuance and Presentation request

/// <summary>
/// A request to issue a new verifiable credential.
/// </summary>
public class IssuanceRequest
{
    /// <summary>
    /// Determines whether a QR code is included in the response of this request. Present the
    /// QR code and ask the user to scan it. Scanning the QR code launches the authenticator
    /// app with this issuance request. Possible values are true (default) or false.
    /// When you set the value to false, use the return url property to render a deep link.
    /// </summary>
    public bool IncludeQRCode { get; set; }

    /// <summary>
    /// Mandatory. Allows the developer to asynchronously get information on the flow during
    /// the verifiable credential issuance process. For example, the developer might want a
    /// call when the user has scanned the QR code or if the issuance request succeeds or fails.
    /// </summary>
    public Callback? Callback { get; set; } = new Callback();

    /// <summary>
    /// The issuer's decentralized identifier (DID).
    /// </summary>
    public string? Authority { get; set; }

    /// <summary>
    /// Provides information about the issuer that can be displayed in the authenticator app.
    /// </summary>
    public Registration? Registration { get; set; } = new Registration();

    /// <summary>
    /// Provides information about the issuance request.
    /// </summary>
    public Issuance? Issuance { get; set; } = new Issuance();
}