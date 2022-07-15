namespace MicrosoftEntra.VerifiedId.Client.Models;

public class ErrorResponse
{
    public string? RequestId { get; set; }
    public string? Date { get; set; }
    public ErrorDetail? Error { get; set; }
}