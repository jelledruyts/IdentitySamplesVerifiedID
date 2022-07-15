# Identity Sample for Microsoft Entra Verified ID

Demonstrates a scenario of a university web app issuing a Microsoft Entra Verified ID so that an students and teachers can get discounts in an e-commerce shop that trusts those Verified IDs.

## Setup

- Go through [documented tenant setup process](https://docs.microsoft.com/azure/active-directory/verifiable-credentials/verifiable-credentials-configure-tenant).
  - Create an Azure Key Vault instance
  - Set up the Verifiable Credentials service
  - Create the "Contoso Musiversity" app registration
    - Make sure to grant it the "VerifiableCredential.Create.All" permission on the "Verifiable Credentials Service Request" Service Principal (for Application ID "3db474b9-6a0c-4840-96ac-1fceb342124f") and grant admin consent
- For domain verification, set the `EntraVerifiedId:DomainLinkageCredentials:0` app setting to the JWT
