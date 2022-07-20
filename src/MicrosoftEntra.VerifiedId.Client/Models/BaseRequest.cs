namespace MicrosoftEntra.VerifiedId.Client.Models;

/// <summary>
/// A base request for the Verifiable Credentials Service.
/// </summary>
public abstract class BaseRequest
{
    /// <summary>
    /// Determines whether a QR code is included in the response of this request. Present the
    /// QR code and ask the user to scan it. Scanning the QR code launches the authenticator
    /// app with this issuance or presentation request. Possible values are true (default) or false.
    /// When you set the value to false, use the return url property to render a deep link.
    /// </summary>
    public bool IncludeQRCode { get; set; }

    /// <summary>
    /// Mandatory. Allows the developer to asynchronously get information on the flow during
    /// the verifiable credential issuance or presentation process. For example, the developer
    /// might want a call when the user has scanned the QR code or if the issuance or
    /// presentation request succeeds or fails.
    /// </summary>
    public Callback Callback { get; set; } = new Callback();

    /// <summary>
    /// The issuer's or verifier's decentralized identifier (DID).
    /// </summary>
    public string? Authority { get; set; }

    /// <summary>
    /// Provides information about the issuer or verifier that can be displayed in the authenticator app.
    /// </summary>
    public Registration Registration { get; set; } = new Registration();
}