using System.Text.Json.Serialization;

namespace Demo.Core.Models
{
    public class Configuration
    {
        [JsonPropertyName("issuer")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Issuer { get; set; }

        [JsonPropertyName("jwks_uri")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string JwksUri { get; set; }

        [JsonPropertyName("authorization_endpoint")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string AuthorizationEndpoint { get; set; }

        [JsonPropertyName("token_endpoint")]
        public string TokenEndpoint { get; set; }

        [JsonPropertyName("token_endpoint_auth_methods_supported")]
        public string[] TokenEndpointAuthMethodsSupported { get; set; }

        [JsonPropertyName("grant_types_supported")]
        public string[] GrantTypesSupported { get; set; }

        [JsonPropertyName("registration_endpoint")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string RegistrationEndpoint { get; set; }

        [JsonPropertyName("scopes_supported")]
        public string[] ScopesSupported { get; set; }

        [JsonPropertyName("response_types_supported")]
        public string[] ResponseTypesSupported { get; set; }

        [JsonPropertyName("management_endpoint")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string ManagementEndpoint { get; set; }

        [JsonPropertyName("introspection_endpoint")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string IntrospectionEndpoint { get; set; }

        [JsonPropertyName("revocation_endpoint")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string RevocationEndpoint { get; set; }

        [JsonPropertyName("code_challenge_methods_supported")]
        public string[] CodeChallengeMethodsSupported { get; set; }

        [JsonPropertyName("capabilities")]
        public string[] Capabilities { get; set; }
    }
}