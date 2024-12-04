using System;
using System.Text.RegularExpressions;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CSharpEssentials.AspNetCore.Swagger.Filters;

public sealed partial class ReApplyOptionalRouteParameterOperationFilter : IOperationFilter
{
    const string _captureName = "routeParameter";

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var httpMethodAttributes = context.MethodInfo
            .GetCustomAttributes(true)
            .OfType<Microsoft.AspNetCore.Mvc.Routing.HttpMethodAttribute>();

        var httpMethodWithOptional = httpMethodAttributes?.FirstOrDefault(m => m.Template?.Contains('?') ?? false);
        if (httpMethodWithOptional?.Template == null) return;

        var matches = RouteRegex().Matches(httpMethodWithOptional.Template);

        foreach (Match match in matches.Cast<Match>())
        {
            var name = match.Groups[_captureName].Value;

            var parameter = operation.Parameters.FirstOrDefault(p => p.In == ParameterLocation.Path && p.Name == name);
            if (parameter == null) continue;
            parameter.AllowEmptyValue = true;
            parameter.Required = false;
            parameter.Schema.Default = new OpenApiString(null);
            parameter.Schema.Nullable = true;
        }
    }

    [GeneratedRegex(@"{(?<routeParameter>\w+)\?}")]
    private static partial Regex RouteRegex();
}