using System.Threading.Tasks;
using Mercure.API.Utils.Logger;
using Microsoft.AspNetCore.Http;

namespace Mercure.API.Middleware;

public class HttpLoggerSystem
{
    private RequestDelegate _next;
    
    public HttpLoggerSystem(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var request = context.Request;
        var requestLog = $"[{request.Method}] {request.Path} {request.QueryString}";
        
        Logger.Log(LogLevel.Info, LogTarget.EventLog, requestLog);
        
        
        await _next(context);
    }
}