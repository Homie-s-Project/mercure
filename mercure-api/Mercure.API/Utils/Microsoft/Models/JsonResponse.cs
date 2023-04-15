using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mercure.API.Utils.Microsoft.Models
{
    /// <summary>
    /// Base class for JSON objects that need to be serialized/deserialized.
    /// </summary>
    public class Json
    {
        /// <summary>
        /// Extra data for/from the JSON serializer/deserializer to included with the object model.
        /// </summary>
        [JsonExtensionData]
        public IDictionary<string, JToken> ExtraJson { get; internal set; } = new Dictionary<string, JToken>();
    }

    /// <summary>
    /// User information from Microsoft Graph.
    /// </summary>
    public class OAuthResponse : Json
    {
        /// <summary>
        /// Gets or sets the access token.
        /// </summary>
        [JsonProperty("access_token")] public string AccessToken { get; set; }

        /// <summary>
        /// Gets or sets the refresh token.
        /// </summary>
        [JsonProperty("scope")] public string Scope { get; set; }

        /// <summary>
        /// Gets or sets the refresh token.
        /// </summary>
        [JsonProperty("token_type")] public string TokenType { get; set; }

        /// <summary>
        /// Gets or sets the refresh token.
        /// </summary>
        [JsonProperty("expires_in")] protected internal int ExpiresInSeconds { get; set; }
        public TimeSpan Expires { get; private set; }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext ctx)
        {
            this.Expires = TimeSpan.FromSeconds(this.ExpiresInSeconds);
        }
    }
}