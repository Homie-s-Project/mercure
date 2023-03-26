using NUnit.Framework;

namespace Mercure.API.Tests.Models;

[TestFixture]
public class OrderTest : AssemblyLoader
{
    private const string TYPE_NAME = "order";
    
    [Test]
    public void TypeExist()
    {
        Assert.IsNotNull(GetType(TYPE_NAME));
    }
    
    [Test]
    public void TypeIsPublic()
    {
        Assert.IsTrue(GetType(TYPE_NAME).IsPublic);
    }
    
    [Test]
    public void TypeIsEnum()
    {
        Assert.IsTrue(base.GetType(TYPE_NAME)?.IsClass ?? false);
    }
}