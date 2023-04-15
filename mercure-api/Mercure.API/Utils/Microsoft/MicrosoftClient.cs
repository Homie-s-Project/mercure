using System.Threading.Tasks;
using Flurl.Http;
using Mercure.API.Utils.Microsoft.Models;

namespace Mercure.API.Utils.Microsoft;

/// <summary>
/// Classe utilitaire pour la connexion à Microsoft
/// </summary>
public class MicrosoftClient
{
    private const string OAuthUser = "https://graph.microsoft.com/v1.0/me";

    private readonly string _userToken;

    /// <summary>
    /// Constructeur de la classe
    /// </summary>
    /// <param name="encryptedUserToken"></param>
    public MicrosoftClient(string encryptedUserToken)
    {
        _userToken = CryptoUtils.Decrypt(encryptedUserToken);
    }

    /// <summary>
    /// On fait une requête à l'API Microsoft pour récupérer les informations de l'utilisateur
    /// </summary>
    /// <returns>L'utilisateur Microsoft conneté</returns>
    public Task<UserMicrosoft> GetUserAsync()
    {
        return OAuthUser
            .WithOAuthBearerToken(_userToken)
            .GetJsonAsync<UserMicrosoft>();
    }
}