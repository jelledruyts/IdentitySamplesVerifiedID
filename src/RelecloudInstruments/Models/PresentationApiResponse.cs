using MicrosoftEntra.VerifiedId.Client.Models;

namespace RelecloudInstruments.Models;

public class PresentationApiResponse
{
    public string? RequestId { get; set; }
    public string? Url { get; set; }
    public string? QRCode { get; set; }

    public PresentationApiResponse()
    {
    }

    public PresentationApiResponse(PresentationResponse response)
    {
        this.RequestId = response.RequestId;
        this.Url = response.Url;
        this.QRCode = response.QRCode;
    }
}