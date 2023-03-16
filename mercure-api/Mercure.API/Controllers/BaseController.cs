using Microsoft.AspNetCore.Mvc;

namespace Mercure.API.Controllers;

[Produces("application/json")]
[ApiController]
[Route("api/[controller]")]
public class BaseController : ControllerBase
{
}