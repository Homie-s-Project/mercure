using Newtonsoft.Json;

namespace Mercure.API.Utils.Microsoft.Models;

/// <summary>
/// User information from Microsoft Graph.
/// </summary>
public class UserMicrosoft : Json
{
    /// <summary>
    /// Gets or sets the user's display name.
    /// </summary>
    [JsonProperty("displayName")] public string DisplayUsername { get; set; }

    /// <summary>
    /// Gets or sets the user's first name.
    /// </summary>
    [JsonProperty("givenName")] public string GivenName { get; set; }

    /// <summary>
    /// Gets or sets the user's job title.
    /// </summary>
    [JsonProperty("jobTitle")] public string JobTitle { get; set; }

    /// <summary>
    /// Gets or sets the user's mail address.
    /// </summary>
    [JsonProperty("mail")] public string Mail { get; set; }

    /// <summary>
    /// Gets or sets the user's mobile phone number.
    /// </summary>
    [JsonProperty("mobilePhone")] public string MobilePhone { get; set; }

    /// <summary>
    /// Gets or sets the user's preferred language.
    /// </summary>
    [JsonProperty("preferredLanguage")] public string PreferedLanguage { get; set; }

    /// <summary>
    /// Sets or gets the user's surname.
    /// </summary>
    [JsonProperty("surname")] public string Surname { get; set; }

    /// <summary>
    /// Gets or sets the user's unique identifier.
    /// </summary>
    [JsonProperty("id")] public string Id { get; set; }
}