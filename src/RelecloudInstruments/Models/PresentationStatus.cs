namespace RelecloudInstruments.Models;

public class PresentationStatus
{
    public string? Status { get; set; }
    public IList<string>? CredentialTypes { get; set; } = new List<string>();
}