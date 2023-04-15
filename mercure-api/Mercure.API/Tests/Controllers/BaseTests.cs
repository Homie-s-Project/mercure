using System;
using System.Reflection;
using Mercure.API.Controllers;
using Mercure.API.Models;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace Mercure.API.Tests.Controllers;

/// <summary>
/// Tests for the <see cref="BaseController"/> class.
/// </summary>
[TestFixture]
public class BaseTests
{
    
    /// <summary>
    /// Test the <see cref="BaseController"/> controller.
    /// </summary>
    [Test]
    [Category("BaseController")]
    public void TestAnnotation()
    {
        var baseController = typeof(BaseController);

        var productAttribute = baseController.GetCustomAttribute<ProducesAttribute>();
        Assert.IsNotNull(productAttribute);
        
        var apiControllerAttribute = baseController.GetCustomAttribute<ApiControllerAttribute>();
        Assert.IsNotNull(apiControllerAttribute);
        
        var routeAttribute = baseController.GetCustomAttribute<RouteAttribute>();
        Assert.IsNotNull(routeAttribute);
    }
}