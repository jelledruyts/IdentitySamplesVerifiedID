namespace MicrosoftEntra.VerifiedId;

public static class VerifiedIdConstants
{
    public static class Scopes
    {
        /// <summary>
        /// The fixed scope which is required for the "Verifiable Credentials Service Request" app permission.
        /// </summary>
        public const string VerifiableCredentialsServiceRequestScope = "3db474b9-6a0c-4840-96ac-1fceb342124f/.default";
    }

    public static class PinTypes
    {
        /// <summary>
        /// Indicates a numeric PIN type.
        /// </summary>
        public const string Numeric = "numeric";
    }

    public static class PinAlgorithms
    {
        /// <summary>
        /// Indicates a SHA256 hash algorithm.
        /// </summary>
        public const string Sha256 = "sha256";
    }

    public static class CallbackHeaderNames
    {
        /// <summary>
        /// Indicates an API key header.
        /// </summary>
        public const string ApiKey = "api-key";

        /// <summary>
        /// Indicates an authorization header.
        /// </summary>
        public const string Authorization = "Authorization";
    }

    public static class CallbackCodes
    {
        /// <summary>
        /// The user scanned the QR code or selected the link that starts the issuance flow.
        /// </summary>
        public const string RequestRetrieved = "request_retrieved";

        /// <summary>
        /// The issuance of the verifiable credentials was successful.
        /// </summary>
        public const string IssuanceSuccessful = "issuance_successful";

        /// <summary>
        /// There was an error during issuance.
        /// </summary>
        public const string IssuanceError = "Issuance_error";
    }
}