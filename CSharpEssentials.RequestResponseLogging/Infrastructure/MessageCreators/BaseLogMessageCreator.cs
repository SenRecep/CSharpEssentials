﻿using System.Globalization;

namespace CSharpEssentials.RequestResponseLogging.Infrastructure.MessageCreators;

internal abstract class BaseLogMessageCreator
{
    protected static string? GenerateLogStringByField(RequestResponseContext requestResponseContext, LoggingOptions loggingOptions, LogFields field)
    {
        return field switch
        {
            LogFields.Request => requestResponseContext.RequestBody,
            LogFields.Response => requestResponseContext.ResponseBody,
            LogFields.QueryString => requestResponseContext.Context.Request.QueryString.Value,
            LogFields.Path => requestResponseContext.Context.Request.Path.Value,
            LogFields.HostName => requestResponseContext.Context.Request.Host.Value,
            LogFields.ResponseTiming => requestResponseContext.ResponseTime,
            LogFields.RequestLength => requestResponseContext.RequestLength?.ToString(CultureInfo.InvariantCulture),
            LogFields.ResponseLength => requestResponseContext.ResponseLength?.ToString(CultureInfo.InvariantCulture),
            LogFields.Method => requestResponseContext.Context.Request.Method,
            LogFields.Headers => GetHeaders(requestResponseContext, loggingOptions.HeaderKeys),
            _ => string.Empty
        };
    }

    private static string GetHeaders(RequestResponseContext requestResponseContext, HashSet<string> headerKeys)
    {
        if (requestResponseContext?.Context?.Request?.Headers == null || headerKeys.Count == 0)
            return string.Empty;

        IEnumerable<string> filteredHeaders = requestResponseContext.Context.Request.Headers
            .Where(header => headerKeys.Contains(header.Key))
            .Select(header => $"{{\"{header.Key}\":\"{header.Value}\"}}");

        return $"[{string.Join(",", filteredHeaders)}]";
    }

}