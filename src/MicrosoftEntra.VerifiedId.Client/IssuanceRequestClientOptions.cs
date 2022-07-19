namespace MicrosoftEntra.VerifiedId.Client;

public class IssuanceRequestClientOptions : BaseRequestClientOptions
{
    /// <summary>
    /// The contents of the "did.json" document to host.
    /// </summary>
    public string? WellKnownDidJsonContents { get; set; } // TODO: Split out from SDK

    /// <summary>
    /// The contents of the "did-configuration.json" document to host.
    /// </summary>
    public string? WellKnownDidConfigurationJsonContents { get; set; } // TODO: Split out from SDK

    /// <summary>
    /// The PIN length to require, or null if no PIN is required.
    /// </summary>
    public int? PinLength { get; set; }
}