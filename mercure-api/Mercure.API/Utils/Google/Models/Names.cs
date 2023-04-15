using Newtonsoft.Json;

namespace Mercure.API.Utils.Google.Models;

/// <summary>
/// Represents a response from the Google OAuth2 API.
/// </summary>
public class Names : Json
{
    /// <summary>
    /// Gets or sets the display name.
    /// </summary>
    [JsonProperty("displayName")] public string DisplayName { get; set; }
    /// <summary>
    /// Gets or sets the family name.
    /// </summary>
    [JsonProperty("familyName")] public string FamilyName { get; set; }
    /// <summary>
    /// Gets or sets the given name.
    /// </summary>
    [JsonProperty("givenName")] public string GivenName { get; set; }
    /// <summary>
    /// Gets or sets the display name last first.
    /// </summary>
    [JsonProperty("displayNameLastFirst")] public string DisplayNameLastFirst { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("unstructuredName")] public string UnstructuredName { get; set; }
}