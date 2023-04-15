using System.Reflection;
using Mercure.API.Controllers;
using Mercure.API.Middleware;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace Mercure.API.Tests.Controllers;

/// <summary>
/// Tests for the <see cref="SecurityController"/> class.
/// </summary>
[TestFixture]
public class SecurityTests
{
    
    /// <summary>
    /// Test the <see cref="SecurityController"/> controller.
    /// </summary>
    [Test]
    [Category("SecurityController")]
    public void TestAnnotation()
    {
        var securityController = typeof(SecurityController);

        var authorizeAttribute = securityController.GetCustomAttribute<AuthorizeAttribute>();
        Assert.IsNotNull(authorizeAttribute);
        
        var apiControllerAttribute = securityController.GetCustomAttribute<ApiControllerAttribute>();
        Assert.IsNotNull(apiControllerAttribute);
        
        var routeAttribute = securityController.GetCustomAttribute<RouteAttribute>();
        Assert.IsNotNull(routeAttribute);
    }
}