using System.Reflection;
using Mercure.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace Mercure.API.Tests.Controllers;

/// <summary>
/// Tests for the <see cref="ApiNoSecurityController"/> class.
/// </summary>
[TestFixture]
public class ApiNoSecurityTests
{
    
    /// <summary>
    /// Test the <see cref="ApiNoSecurityController"/> controller.
    /// </summary>
    [Test]
    [Category("ApiNoSecurityController")]
    public void TestAnnotation()
    {
        var apiNoSecurity = typeof(ApiNoSecurityController);
        
        var routeAttribute = apiNoSecurity.GetCustomAttribute<RouteAttribute>();
        Assert.IsNotNull(routeAttribute);
    }
}