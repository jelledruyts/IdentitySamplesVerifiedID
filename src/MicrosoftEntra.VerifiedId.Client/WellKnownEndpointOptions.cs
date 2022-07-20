namespace MicrosoftEntra.VerifiedId.Client;

public class WellKnownEndpointOptions
{
    /// <summary>
    /// The contents of the "did.json" document to host.
    /// </summary>
    public string? WellKnownDidJsonContents { get; set; }

    /// <summary>
    /// The contents of the "did-configuration.json" document to host.
    /// </summary>
    public string? WellKnownDidConfigurationJsonContents { get; set; }
}