// using System.Collections.Generic;

// namespace MicrosoftEntra.VerifiedId.Client
// {

//     /// <summary>
//     /// VC Presentation
//     /// </summary>
//     public class PresentationRequest
//     {
//         public string authority { get; set; }
//         public bool includeQRCode { get; set; }
//         public Registration registration { get; set; }
//         public Callback callback { get; set; }
//         public Presentation presentation { get; set; }
//     }

//     /// <summary>
//     /// Presentation - the specific details when you do VC presentation
//     /// </summary>
//     public class Presentation
//     {
//         public bool includeReceipt { get; set; }
//         public List<RequestedCredential> requestedCredentials { get; set; }
//     }

//     /// <summary>
//     /// Presentation can involve asking for multiple VCs
//     /// </summary>
//     public class RequestedCredential
//     {
//         public string type { get; set; }
//         public string manifest { get; set; }
//         public string purpose { get; set; }
//         public List<string> acceptedIssuers { get; set; }
//     }

//     /// <summary>
//     /// VC Client API callback
//     /// </summary>
//     public class VCCallbackEvent
//     {
//         public string requestId { get; set; }
//         public string code { get; set; }
//         public Error error { get; set; }
//         public string state { get; set; }
//         public string subject { get; set; }
//         public ClaimsIssuer[] issuers { get; set; }
//         public Receipt receipt { get; set; }
//     }

//     /// <summary>
//     /// Receipt - returned when VC presentation is verified. The id_token contains the full VC id_token
//     /// the state is not to be confused with the VCCallbackEvent.state and is something internal to the VC Client API
//     /// </summary>
//     public class Receipt
//     {
//         public string id_token { get; set; }
//         public string state { get; set; }
//     }

//     /// <summary>
//     /// ClaimsIssuer - details of each VC that was presented (usually just one)
//     /// authority gives you who issued the VC and the claims is a collection of the VC's claims, like givenName, etc
//     /// </summary>
//     public class ClaimsIssuer
//     {
//         public string authority { get; set; }
//         public string domain { get; set; }
//         public string verified { get; set; }
//         public string[] type { get; set; }
//         public IDictionary<string, string> claims { get; set; }
//     }

// }