using System.Threading.Tasks;
using Flurl.Http;
using Mercure.API.Utils.Google.Models;

namespace Mercure.API.Utils.Google;

public class GoogleClient
{
    public const string OAuthUser =
        "https://people.googleapis.com/v1/people/me?personFields=birthdays,emailAddresses,names";

    private readonly string _userToken;

    public GoogleClient(string encryptedUserToken)
    {
        _userToken = CryptoUtils.Decrypt(encryptedUserToken);
    }

    /// <summary>
    /// On fait la requête à l'API Google pour récupérer les informations de l'utilisateur
    /// </summary>
    /// <returns>L'utilisateur Google connecté</returns>
    public Task<UserGoogle> GetUserAsync()
    {
        return OAuthUser
            .WithOAuthBearerToken(_userToken)
            .GetJsonAsync<UserGoogle>();
    }
}