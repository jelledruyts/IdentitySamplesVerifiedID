using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MicrosoftEntra.VerifiedId.Client
{
    // TODO: Create IRequestClient for cleaner DI?
    public class RequestClient
    {
        private readonly RequestClientOptions options;
        private readonly HttpClient httpClient;

        public RequestClient(RequestClientOptions options, HttpClient httpClient)
        {
            this.options = options;
            this.httpClient = httpClient;
        }

        public Task RequestIssuance(string callbackUrl, string callbackState)
        {
            if (this.options.Authority == null) throw new ArgumentNullException(nameof(this.options.Authority));
            var request = new IssuanceRequest
            {
                IncludeQRCode = false,
                Callback = new Callback
                {
                    Url = callbackUrl,
                    State = callbackState,
                    Headers = null // TODO
                },
                Authority = this.options.Authority,
                Registration = new Registration
                {
                    ClientName = this.options.ClientName
                },
                Issuance = new Issuance
                {
                    Type = "", // TODO
                    Manifest = "", // TODO
                    Claims = null // TODO
                }
            };
            // if (this.options.MinimumPinLength > 0) // TODO
            // {
            //     request.Issuance.Pin = new Pin
            //     {
            //         Length = this.options.MinimumPinLength,
            //         Value = "" // TODO
            //     };
            // }
            return RequestIssuanceAsync(request);
        }

        public async Task RequestIssuanceAsync(IssuanceRequest request)
        {
            var url = GetApiUrl("verifiablecredentials/request");
        }

        private string GetApiUrl(string api)
        {
            if (this.options.Instance == null) throw new ArgumentNullException(nameof(this.options.Instance));
            if (this.options.TenantId == null) throw new ArgumentNullException(nameof(this.options.TenantId));
            return $"{this.options.Instance.TrimEnd('/')}/{this.options.TenantId}/{api}";
        }
    }
}