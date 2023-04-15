using Newtonsoft.Json;

namespace Mercure.API.Utils.Google.Models;

/// <summary>
/// Classe utilitaire pour gérer l'authentification avec Google
/// </summary>
public class Birthdays : Json
{
    [JsonProperty("date")] public DateGoogle Date { get; set; }
}

/// <summary>
/// Classe utilitaire pour gérer l'authentification avec Google
/// </summary>
public class DateGoogle
{
    [JsonProperty("year")] public int Year { get; set; }
    [JsonProperty("month")] public int Month { get; set; }
    [JsonProperty("day")] public int Day { get; set; }
}