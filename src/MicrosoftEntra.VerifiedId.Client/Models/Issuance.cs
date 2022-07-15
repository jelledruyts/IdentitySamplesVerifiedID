using System.Collections.Generic;

namespace MicrosoftEntra.VerifiedId.Client.Models;

public class Issuance
{
    public string? Type { get; set; }
    public string? Manifest { get; set; }
    public Pin? Pin { get; set; }
    public IDictionary<string, string>? Claims { get; set; } = new Dictionary<string, string>();
}