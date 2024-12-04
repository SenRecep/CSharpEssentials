namespace CSharpEssentials.RequestResponseLogging.Infrastructure.Middlewares;

internal class DefaultRequestResponseWithHandlerMiddleware(RequestDelegate next,
                                                   Func<RequestResponseContext, Task> reqResHandler,
                                                   ILogWriter logWriter,
                                                   string[] ignoredPaths) : BaseMiddleware(logWriter, ignoredPaths)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        if (IsIgnoredPath(httpContext))
        {
            await next(httpContext);
            return;
        }

        var reqResContext = await InvokeMiddleware(next, httpContext);

        await reqResHandler.Invoke(reqResContext);
    }
}
