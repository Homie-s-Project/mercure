using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mercure.API.Utils.Google.Models;

/// <summary>
/// Represents a response from the Google OAuth2 API.
/// </summary>
public class UserGoogle : Json
{
    /// <summary>
    /// Gets or sets the display name.
    /// </summary>
    [JsonProperty("names")] public List<Names> Names { get; set; }
    /// <summary>
    /// Gets or sets the display name.
    /// </summary>
    [JsonProperty("birthdays")] public List<Birthdays> Birthdays { get; set; }
    /// <summary>
    /// Gets or sets the display name.
    /// </summary>
    [JsonProperty("emailAddresses")] public List<EmailAddresses> EmailAddresses { get; set; }
    /// <summary>
    /// Gets or sets the display name.
    /// </summary>
    [JsonProperty("resourceName")] public string ResourceName { get; set; }
}