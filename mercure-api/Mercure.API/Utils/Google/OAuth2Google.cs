using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Mercure.API.Utils.Google.Models;

namespace Mercure.API.Utils.Google;

/// <summary>
/// Classe utilitaire pour gérer l'authentification avec Google
/// </summary>
public abstract class OAuth2Google
{
    /// <summary>
    /// Options pour la connexion
    /// </summary>
    public class AuthorizeOptions
    {
        /// <summary>
        /// Le client id de notre application
        /// </summary>
        public string ClientId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string RedirectUri { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Scope { get; set; }
    }

    /// <summary>
    /// Classe utilitaire pour gérer l'authentification avec Google
    /// </summary>
    public static class OAuthHelper
    {
        private const string OAuthEndpoint =
            "https://oauth2.googleapis.com/token";

        private const string AuthorizeEndpoint = "https://accounts.google.com/o/oauth2/v2/auth";

        /// <summary>
        /// On crée une url pour rediriger l'utilisateur vers la page de connexion de Google
        /// </summary>
        /// <param name="opts">Les options de connexion</param>
        /// <returns>l'url</returns>
        public static string GetAuthorizeUrl(AuthorizeOptions opts)
        {
            var url = AuthorizeEndpoint
                .SetQueryParam("client_id", opts.ClientId)
                .SetQueryParam("response_type", "code")
                .SetQueryParam("redirect_uri", opts.RedirectUri)
                .SetQueryParam("state", opts.State)
                .SetQueryParam("scope", opts.Scope);

            return url;
        }

        /// <summary>
        /// On récupère le token d'accès à partir du code de connexion
        /// </summary>
        /// <param name="code">Code reçu après la connexion</param>
        /// <param name="clientId">Le clientId de notre application</param>
        /// <param name="clientSecret">Le clientSecret de notre application</param>
        /// <param name="redirectUri">Où souhaite le rediriger</param>
        public static Task<OAuthResponse> GetAccessTokenAsync(string code, string clientId, string clientSecret,
            string redirectUri)
        {
            var form = new
            {
                client_id = clientId,
                client_secret = clientSecret,
                redirect_uri = redirectUri,
                grant_type = "authorization_code",
                code,
            };

            return OAuthEndpoint
                .PostUrlEncodedAsync(form)
                .ReceiveJson<OAuthResponse>();
        }
    }
}