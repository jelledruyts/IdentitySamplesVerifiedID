using System.Text;
using System.Text.Json.Serialization;

namespace MicrosoftEntra.VerifiedId.Client.Models;

public class ErrorDetail
{
    public string? Code { get; set; }
    public string? Message { get; set; }
    public string? Target { get; set; }
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