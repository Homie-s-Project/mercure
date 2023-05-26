using Microsoft.AspNetCore.Mvc;

namespace Mercure.API.Controllers;

/// <summary>
/// All the routes for the api that don't need to be secured
/// </summary>
[Route("api/[controller]")]
public class ApiNoSecurityController : BaseController
{
    
}