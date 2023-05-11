using System.Linq;
using System.Threading.Tasks;
using Mercure.API.Context;
using Mercure.API.Models;
using Mercure.API.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

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
        var hasToken = token != null && token.StartsWith(bearer);

        // Si le token est présent
        if (hasToken)
        {
            // On récupère le token sans le "Bearer "
            token = token.Remove(0, bearer.Length + 1);
        }

        // Récupère l'id de l'utilisateur
        var informationsJwt = JwtUtils.ValidateCurrentToken(token);

        // Si le token est valide
        if (informationsJwt != null)
        {
            // On ajoute dans le context de la requête l'utilisateur ainsi que son rôle
            var userId = informationsJwt.Value;
            var user = await mercureContext.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user != null)
            {
                context.Items["User"] = user;
            }
            else
            {
                // 498 = Token expired
                context.Response.StatusCode = 498;
                await context.Response.WriteAsync("Bad token");
            }
        }

        // Si le token est présent mais invalide
        if (hasToken && informationsJwt == null)
        {
            // 498 = Token expired
            context.Response.StatusCode = 498;
            await context.Response.WriteAsync("Bad token");
        }

        await _next(context);
    }
}