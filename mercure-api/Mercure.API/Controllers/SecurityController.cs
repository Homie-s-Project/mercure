using Mercure.API.Middleware;
using Microsoft.AspNetCore.Mvc;

namespace Mercure.API.Controllers;

/// <summary>
/// All the routes that need to be secured
/// </summary>
[ApiController]
[Route("[controller]")]
[Authorize]
public class SecurityController : BaseController
{
}