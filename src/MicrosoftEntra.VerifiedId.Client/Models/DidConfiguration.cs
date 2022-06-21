using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MicrosoftEntra.VerifiedId.Models;

// See https://identity.foundation/.well-known/resources/did-configuration/.
public class DidConfiguration
{
    [JsonPropertyName("@context")]
    public string Context { get; } = "https://identity.foundation/.well-known/contexts/did-configuration-v0.0.jsonld";
    
    [JsonPropertyName("linked_dids")]
    public IList<string>? LinkedDids { get; set; } = new List<string>(); // TODO: Also support Linked Data Proof Format.
}