using System;
using System.Reflection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Extensions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CSharpEssentials.AspNetCore.Swagger.Filters;

public class EnumSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        var isStringEnum = context.Type.IsEnum && context.Type.GetCustomAttribute<StringEnumAttribute>() != null;
        if (isStringEnum.IsFalse()) return;
        var values = Enum.GetNames(context.Type).Select(x => x.ToSnakeCase()).ToArray();

        var enumValues = values
            .Select(name => new OpenApiString(name))
            .Cast<IOpenApiAny>()
            .ToList();

        schema.Type = "string";
        schema.Enum = enumValues;
        schema.Description = $"Possible values: {string.Join(", ", values)}";
    }
}