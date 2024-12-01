using CSharpEssentials.Enums;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace CSharpEssentials.Json;

/// <summary>
/// A conditional string enum converter.
/// </summary>
/// <param name="namingPolicy"></param>
/// <param name="allowIntegerValues"></param>
public class ConditionalStringEnumConverter(JsonNamingPolicy? namingPolicy = null, bool allowIntegerValues = true) : JsonConverterFactory
{
    private readonly JsonNamingPolicy? _namingPolicy = namingPolicy ?? JsonNamingPolicy.SnakeCaseLower;
    private readonly bool _allowIntegerValues = allowIntegerValues;
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsEnum && typeToConvert.GetCustomAttribute<StringEnumAttribute>() != null;
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        return new JsonStringEnumConverter(_namingPolicy, _allowIntegerValues)
            .CreateConverter(typeToConvert, options);
    }
}