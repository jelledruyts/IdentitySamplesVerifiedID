using System.Collections.Generic;

namespace MicrosoftEntra.VerifiedId.Client.Models;

public class Callback
{
    public string? Url { get; set; }
    public string? State { get; set; }
    public IDictionary<string, string>? Headers { get; set; } = new Dictionary<string, string>();
}