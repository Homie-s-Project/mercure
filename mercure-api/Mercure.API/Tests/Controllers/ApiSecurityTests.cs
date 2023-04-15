using System.Reflection;
using Mercure.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace Mercure.API.Tests.Controllers;

/// <summary>
/// Tests for the <see cref="ApiSecurityController"/> class.
/// </summary>
[TestFixture]
public class ApiSecurityTests
{
    
    /// <summary>
    /// Test the <see cref="ApiSecurityController"/> controller.
    /// </summary>
    [Test]
    [Category("ApiSecurityController")]
    public void TestAnnotation()
    {
        var apiSecurity = typeof(ApiSecurityController);
        
        var routeAttribute = apiSecurity.GetCustomAttribute<RouteAttribute>();
        Assert.IsNotNull(routeAttribute);
    }
}