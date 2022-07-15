using System.Text;
using System.Text.Json.Serialization;

namespace MicrosoftEntra.VerifiedId.Client.Models;

/// <summary>
/// Error details containing more information about the reason of the failure.
/// </summary>
public class ErrorDetail
{
    /// <summary>
    /// The return error code.
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// The error message.
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// The target of the error.
    /// </summary>
    public string? Target { get; set; }

    /// <summary>
    /// The inner error.
    /// </summary>
    [JsonPropertyName("innererror")]
    public ErrorDetail? InnerError { get; set; }

    public string GetErrorMessage()
    {
        var errorMessage = new StringBuilder();
        var detail = this;
        while (detail != null)
        {
            errorMessage.Append("Error \"" + detail.Code + "\"");
            if (!string.IsNullOrWhiteSpace(detail.Message))
            {
                errorMessage.Append(": " + detail.Message);
                if (!detail.Message.EndsWith('.'))
                {
                    errorMessage.Append('.');
                }
            }
            else
            {
                errorMessage.Append('.');
            }
            if (!string.IsNullOrWhiteSpace(detail.Target))
            {
                errorMessage.Append(" Target: \"" + detail.Target + "\".");
            }
            if (detail.InnerError != null)
            {
                errorMessage.Append(" Inner ");
            }
            detail = detail.InnerError;
        }
        return errorMessage.ToString();
    }
}