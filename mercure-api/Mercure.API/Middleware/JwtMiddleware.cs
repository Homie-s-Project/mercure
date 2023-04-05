using System.Linq;
using System.Threading.Tasks;
using Mercure.API.Context;
using Mercure.API.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Mercure.API.Middleware;

// https://jasonwatmore.com/post/2021/06/02/net-5-create-and-validate-jwt-tokens-use-custom-jwt-middleware
public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, MercureContext mercureContext)
    {
        const string bearer = "Bearer";

        // On récupère le token dans le header de la requête
        string token = context.Request.Headers.Authorization;

        // Si le token n'est pas null et qu'il commence par "Bearer "
        if (token != null && token.StartsWith(bearer))
        {
            // On récupère le token sans le "Bearer "
            token = token.Remove(0, bearer.Length + 1);
        }

        // Récupère l'id de l'utilisateur
        var informationsJwt = JwtUtils.ValidateCurrentToken(token);

        // Si le token est valide
        if (informationsJwt != null)
        {
            // On récupère l'utilisateur dans le token.
            var userId = informationsJwt.Value;
            context.Items["User"] = mercureContext.Users
                .Include(u => u.Role)
                .First(u => u.UserId == userId);
        }

        await _next(context);
    }
}