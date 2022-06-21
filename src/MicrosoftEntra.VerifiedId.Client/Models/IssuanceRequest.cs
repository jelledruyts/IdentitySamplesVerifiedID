namespace MicrosoftEntra.VerifiedId.Client
{
    // TODO: Inherit from RequestBase which has common properties between Issuance and Presentation request
    public class IssuanceRequest
    {
        public bool IncludeQRCode { get; set; }
        public Callback? Callback { get; set; } = new Callback();
        public string? Authority { get; set; }
        public Registration? Registration { get; set; } = new Registration();
        public Issuance? Issuance { get; set; } = new Issuance();
    } 
}