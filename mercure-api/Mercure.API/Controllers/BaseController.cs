using Microsoft.AspNetCore.Mvc;

namespace Mercure.API.Controllers;

/// <summary>
/// Base controller which supports application/json type responses
/// </summary>
[Produces("application/json")]
[ApiController]
[Route("api/[controller]")]
public class BaseController : ControllerBase
{
}