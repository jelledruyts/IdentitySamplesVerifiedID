namespace MicrosoftEntra.VerifiedId.Client.Models;

public class IssuanceResponse
{
    public string? RequestId { get; set; }
    public string? Url { get; set; }
    public int Expiry { get; set; }
    public string? QRCode { get; set; }
}