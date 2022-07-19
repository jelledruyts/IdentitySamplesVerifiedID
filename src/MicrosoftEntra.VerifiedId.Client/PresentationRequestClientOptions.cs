using System.Collections.Generic;
using MicrosoftEntra.VerifiedId.Client.Models;

namespace MicrosoftEntra.VerifiedId.Client;

public class PresentationRequestClientOptions : BaseRequestClientOptions
{
    public bool IncludeReceipt { get; set; }

    public IList<RequestedCredential> RequestedCredentials { get; set; } = new List<RequestedCredential>();
}