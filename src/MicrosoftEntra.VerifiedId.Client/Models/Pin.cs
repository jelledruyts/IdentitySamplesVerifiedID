namespace MicrosoftEntra.VerifiedId.Client.Models;

/// <summary>
/// Defines a PIN code that can be displayed as part of the issuance.
/// </summary>
public class Pin
{
    /// <summary>
    /// Contains the PIN value in plain text. When you're using a hashed PIN, the value property contains the salted hash, base64 encoded.
    /// </summary>
    public string? Value { get; set; }

    /// <summary>
    /// The type of the PIN code. Possible value: numeric (default).
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// The length of the PIN code. The default length is 6, the minimum is 4, and the maximum is 16.
    /// </summary>
    public int? Length { get; set; }

    /// <summary>
    /// The salt of the hashed PIN code. The salt is prepended during hash computation. Encoding: UTF-8.
    /// </summary>
    public string? Salt { get; set; }

    /// <summary>
    /// The hashing algorithm for the hashed PIN. Supported algorithm: sha256.
    /// </summary>
    public string? Alg { get; set; }

    /// <summary>
    /// The number of hashing iterations. Possible value: 1.
    /// </summary>
    public int? Iterations { get; set; }
}