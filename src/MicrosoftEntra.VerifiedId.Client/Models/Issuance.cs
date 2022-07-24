using System.Collections.Generic;

namespace MicrosoftEntra.VerifiedId.Client.Models;

/// <summary>
/// Provides information about the issuance request.
/// </summary>
public class Issuance
{
    /// <summary>
    /// The verifiable credential type. Should match the type as defined in the verifiable
    /// credential manifest.
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// The URL of the verifiable credential manifest document.
    /// </summary>
    public string? Manifest { get; set; }

    /// <summary>
    /// Optional. A PIN number to provide extra security during issuance. For PIN code flow,
    /// this property is required. You generate a PIN code, and present it to the user in your
    /// app. The user must provide the PIN code that you generated.
    /// </summary>
    public Pin? Pin { get; set; }

    /// <summary>
    /// Optional. Include a collection of assertions made about the subject in the verifiable
    /// credential. For PIN code flow, it's important that you provide the user's first name
    /// and last name.
    /// </summary>
    public IDictionary<string, string>? Claims { get; set; }
}