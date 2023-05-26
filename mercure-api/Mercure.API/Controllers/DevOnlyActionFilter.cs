using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Mercure.API.Controllers;

/// <summary>
/// Filter checking that it is a dev environment. Used for test routes
/// </summary>
public class DevOnlyActionFilter : ActionFilterAttribute
{
    private IHostingEnvironment HostingEnv { get; }
    public DevOnlyActionFilter(IHostingEnvironment hostingEnv)
    {
        HostingEnv = hostingEnv;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if(!HostingEnv.IsDevelopment())
        {
            context.Result = new NotFoundResult();
            return;
        }    

        base.OnActionExecuting(context);
    }
}