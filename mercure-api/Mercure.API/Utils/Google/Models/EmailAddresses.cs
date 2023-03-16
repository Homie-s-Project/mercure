using Newtonsoft.Json;

namespace Mercure.API.Utils.Google.Models;

public class EmailAddresses : Json
{
    [JsonProperty("value")] public string Value { get; set; }
}