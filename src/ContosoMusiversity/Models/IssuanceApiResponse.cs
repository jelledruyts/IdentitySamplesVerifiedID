using MicrosoftEntra.VerifiedId.Client.Models;

namespace ContosoMusiversity.Models;

public class IssuanceApiResponse
{
    public string? PinValue { get; set; }
    public string? RequestId { get; set; }
    public string? Url { get; set; }
    public string? QRCode { get; set; }

    public IssuanceApiResponse()
    {
    }

    public IssuanceApiResponse(IssuanceRequestContext context)
    {
        this.PinValue = context.PinValue;
        if (context.Response != null)
        {
            this.RequestId = context.Response.RequestId;
            this.Url = context.Response.Url;
            this.QRCode = context.Response.QRCode;
        }
    }
}