namespace MicrosoftEntra.VerifiedId.Client;

public class IssuanceRequestClientOptions : BaseRequestClientOptions
{
    /// <summary>
    /// The PIN length to require, or null if no PIN is required.
    /// </summary>
    public int? PinLength { get; set; }
}