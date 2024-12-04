
using Microsoft.AspNetCore.Http.Features;

namespace CSharpEssentials.RequestResponseLogging.Infrastructure.Middlewares;

internal abstract class BaseMiddleware(ILogWriter logWriter, string[] ignoredPaths)
{
    private readonly ILogWriter? _logWriter = logWriter is NullLogWriter ? null : logWriter;
    private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager = new();

    private static (bool IsSkipRequestLogging, bool IsSkipResponseLogging) IsSkipRequestResponseLogging(HttpContext context)
    {
        var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
        var hasSkipRequestResponseLoggingAttribute = endpoint?.Metadata.GetMetadata<SkipRequestResponseLoggingAttribute>();
        var hasSkipRequestLoggingAttribute = endpoint?.Metadata.GetMetadata<SkipRequestLoggingAttribute>();
        var hasSkipResponseLoggingAttribute = endpoint?.Metadata.GetMetadata<SkipResponseLoggingAttribute>();

        var isSkipRequestLogging = hasSkipRequestResponseLoggingAttribute is not null || hasSkipRequestLoggingAttribute is not null;
        var isSkipResponseLogging = hasSkipRequestResponseLoggingAttribute is not null || hasSkipResponseLoggingAttribute is not null;

        return (isSkipRequestLogging, isSkipResponseLogging);
    }

    protected bool IsIgnoredPath(HttpContext context) => ignoredPaths.Contains(context.Request.Path.Value);

    protected async Task<RequestResponseContext> InvokeMiddleware(RequestDelegate next, HttpContext httpContext)
    {
        (bool isSkipRequestLogging, bool isSkipResponseLogging) = IsSkipRequestResponseLogging(httpContext);

        var requestText = "Skipped logging request body";
        var responseText = "Skipped logging response body";

        if (isSkipRequestLogging is false)
            requestText = await GetRequestBody(httpContext);

        await using var responseBody = _recyclableMemoryStreamManager.GetStream();
        var originalBodyStream = httpContext.Response.Body;

        if (isSkipResponseLogging is false)
            httpContext.Response.Body = responseBody;

        var sw = Stopwatch.StartNew();
        await next.Invoke(httpContext);
        sw.Stop();


        if (isSkipResponseLogging is false)
        {
            httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            responseText = await new StreamReader(httpContext.Response.Body).ReadToEndAsync();
            httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalBodyStream);
        }

        var reqResContext = new RequestResponseContext(httpContext)
        {
            RequestBody = requestText,
            ResponseBody = responseText,
            ResponseCreationTime = TimeSpan.FromTicks(sw.ElapsedTicks)
        };

        _logWriter?.Write(reqResContext);

        return reqResContext;
    }

    private static async Task<string> GetRequestBody(HttpContext context)
    {
        context.Request.EnableBuffering();

        using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
        string reqBody = await reader.ReadToEndAsync();

        context.Request.Body.Seek(0, SeekOrigin.Begin);

        return reqBody;
    }
}
