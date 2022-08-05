using System.Text;

namespace MicrosoftEntra.VerifiedId.Client.Models;

/// <summary>
/// The inner error object contains error specific details useful to the developer to help investigate the current failure.
/// </summary>
public class InnerError
{
    /// <summary>
    /// The internal error code. Contains a standardized code, based on the type of the error.
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// The internal error message. Contains a detailed message of the error.
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Optional. Target contains the field in the request that is causing this error.
    /// This field is optional and may not be present, depending on the error type.
    /// </summary>
    public string? Target { get; set; }

    public string GetErrorMessage()
    {
        var errorMessage = new StringBuilder();
        errorMessage.Append("Inner error \"" + this.Code + "\"");
        if (!string.IsNullOrWhiteSpace(this.Message))
        {
            errorMessage.Append(": " + this.Message);
            if (!this.Message.EndsWith('.'))
            {
                errorMessage.Append('.');
            }
        }
        else
        {
            errorMessage.Append('.');
        }
        if (!string.IsNullOrWhiteSpace(this.Target))
        {
            errorMessage.Append(" Target: \"" + this.Target + "\".");
        }
        return errorMessage.ToString();
    }
}