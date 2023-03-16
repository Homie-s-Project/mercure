using Mercure.API.Middleware;
using Microsoft.AspNetCore.Mvc;

namespace Mercure.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class SecurityController : BaseController
{
}