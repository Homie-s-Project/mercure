using Newtonsoft.Json;

namespace Mercure.API.Utils.Google.Models;

/// <summary>
/// Classe utilitaire pour gérer l'authentification avec Google
/// </summary>
public class EmailAddresses : Json
{
    /// <summary>
    /// The email address.
    /// </summary>
    [JsonProperty("value")] public string Value { get; set; }
}