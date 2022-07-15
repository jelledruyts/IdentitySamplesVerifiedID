namespace MicrosoftEntra.VerifiedId.Client.Models;

public class IssuanceRequestContext
{
    /// <summary>
    /// The issuance request sent to the Verifiable Credentials Service.
    /// </summary>
    public IssuanceRequest Request { get; }
    
    /// <summary>
    /// The unhashed PIN value that the user must enter.
    /// </summary>
    public string? PinValue { get; set; }

    /// <summary>
    /// The issuance response received from the Verifiable Credentials Service.
    /// </summary>
    public IssuanceResponse? Response { get; set; }

    public IssuanceRequestContext(IssuanceRequest request)
    {
        this.Request = request;
    }
}