namespace MicrosoftEntra.VerifiedId.Client;

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
        /// The user scanned the QR code or selected the link that starts the issuance or presentation flow.
        /// </summary>
        public const string RequestRetrieved = "request_retrieved";

        /// <summary>
        /// The issuance of the verifiable credentials was successful.
        /// </summary>
        public const string IssuanceSuccessful = "issuance_successful";

        /// <summary>
        /// There was an error during issuance.
        /// </summary>
        public const string IssuanceError = "issuance_error";

        /// <summary>
        /// The verifiable credential validation completed successfully.
        /// </summary>
        public const string PresentationVerified = "presentation_verified";
    }

    public static class CredentialTypes
    {
        public static readonly string[] BaseTypes = new[] { VerifiableCredential, VerifiablePresentation };

        /// <summary>
        /// A type which specifies a verifiable credential.
        /// See https://www.w3.org/TR/vc-data-model/#types.
        /// </summary>
        public const string VerifiableCredential = "VerifiableCredential";

        /// <summary>
        /// A type which specifies a verifiable presentation.
        /// See https://www.w3.org/TR/vc-data-model/#types.
        /// </summary>
        public const string VerifiablePresentation = "VerifiablePresentation";
    }

    public static class InnerErrorCodes
    {
        /// <summary>
        /// Returned when validation issues on the request occur.
        /// The target field contains the field in the request that is causing the issue.
        /// </summary>
        public const string BadOrMissingField = "badOrMissingField";

        /// <summary>
        /// Returned when a resource the client is requesting isn't found.
        /// The target field contains the resource name/id that isn't found.
        /// </summary>
        public const string NotFound = "notFound";

        /// <summary>
        /// Returned for any validation issues on tokens like JWT and the likes.
        /// The target field contains the token name causing the issue, when applicable.
        /// </summary>
        public const string TokenError = "tokenError";

        /// <summary>
        /// Returned for all the cases where the client might be able to get a successful response
        /// if they retry the request at a later stage. A common example of when this code is
        /// returned is when an HTTP 429 code is returned back.
        /// </summary>
        public const string TransientError = "transientError";
    }
}