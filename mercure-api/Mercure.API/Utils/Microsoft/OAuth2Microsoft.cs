using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Mercure.API.Utils.Microsoft.Models;

namespace Mercure.API.Utils.Microsoft;

/// <summary>
/// Classe utilitaire pour la connexion à Microsoft
/// </summary>
public abstract class OAuth2Microsoft
{
    /// <summary>
    /// Options pour la connexion à Microsoft
    /// </summary>
    public class AuthorizeOptions
    {
        /// <summary>
        /// Le client id de notre application
        /// </summary>
        public string ClientId { get; set; }
        /// <summary>
        /// Le client secret de notre application
        /// </summary>
        public string TenantId { get; set; }
        /// <summary>
        /// L'url de redirection après la connexion
        /// </summary>
        public string RedirectUri { get; set; }
        /// <summary>
        /// Le state de la connexion (pour éviter les attaques CSRF)
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// Le scope de la connexion
        /// </summary>
        public string Scope { get; set; }
    }

    /// <summary>
    /// Options pour la récupération du token d'accès
    /// </summary>
    public static class OAuthHelper
    {
        private const string OAuthEndpoint =
            "https://login.microsoftonline.com/{tenant}/oauth2/v2.0/token";

        private const string AuthorizeEndpoint = "https://login.microsoftonline.com/{tenant}/oauth2/v2.0/authorize";

        /// <summary>
        /// On crée une url pour rediriger l'utilisateur vers la page de connexion de Microsoft
        /// </summary>
        /// <param name="opts">Les options de </param>
        public static string GetAuthorizeUrl(AuthorizeOptions opts)
        {
            var url = AuthorizeEndpoint.Replace("{tenant}", opts.TenantId)
                .SetQueryParam("client_id", opts.ClientId)
                .SetQueryParam("response_type", "code")
                .SetQueryParam("redirect_uri", opts.RedirectUri)
                .SetQueryParam("response_mode", "query")
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
        /// <param name="tenantId">L'id de notre application micrcosoft</param>
        /// <param name="redirectUri">Où souhaite le rediriger</param>
        public static Task<OAuthResponse> GetAccessTokenAsync(string code, string clientId, string clientSecret,
            string tenantId,
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

            // On remplace le tenant dans l'url par l'id de notre application
            return OAuthEndpoint.Replace("{tenant}", tenantId)
                .PostUrlEncodedAsync(form)
                .ReceiveJson<OAuthResponse>();
        }
    }
}