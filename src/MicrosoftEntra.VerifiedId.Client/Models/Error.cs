using System.Text;
using System.Text.Json.Serialization;

namespace MicrosoftEntra.VerifiedId.Client.Models;

/// <summary>
/// Error details containing more information about the reason of the failure.
/// </summary>
public class Error
{
    /// <summary>
    /// The return error code matching the HTTP Status Code.
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// A standardized error message that is also dependent on the HTTP status code returned.
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// The inner error.
    /// </summary>
    [JsonPropertyName("innererror")]
    public InnerError? InnerError { get; set; }

    public string GetErrorMessage()
    {
        var errorMessage = new StringBuilder();
        errorMessage.Append("Error \"" + this.Code + "\"");
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
        if (this.InnerError != null)
        {
            errorMessage.Append(" " + this.InnerError.GetErrorMessage());
        }
        return errorMessage.ToString();
    }
}