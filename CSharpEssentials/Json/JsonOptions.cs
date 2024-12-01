using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CSharpEssentials.Json;

public static class JsonOptions
{
    /// <summary>
    /// The default JSON serializer options.
    /// </summary>
    public static readonly JsonSerializerOptions DefaultOptions = new(JsonSerializerDefaults.Web)
    {
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        WriteIndented = false,
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters =
        {
            new ConditionalStringEnumConverter(),
            new MultiFormatDateTimeConverterFactory(),
        }
    };

    /// <summary>
    /// The default JSON serializer options without converters.
    /// </summary>
    public static readonly JsonSerializerOptions DefaultOptionsWithoutConverters = new(JsonSerializerDefaults.Web)
    {
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        WriteIndented = false,
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    /// <summary>
    /// The default JSON serializer options with a date time converter.
    /// </summary>
    public static readonly JsonSerializerOptions DefaultOptionsWithDateTimeConverter = new(JsonSerializerDefaults.Web)
    {
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        WriteIndented = false,
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters = { new MultiFormatDateTimeConverterFactory() }
    };

    /// <summary>
    /// Creates JSON serializer options with the specified converters.
    /// </summary>
    /// <param name="converters"></param>
    /// <returns></returns>
    public static JsonSerializerOptions CreateOptionsWithConverters(params JsonConverter[] converters)
    {
        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            WriteIndented = false,
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };

        foreach (var converter in converters)
            options.Converters.Add(converter);

        return options;
    }

    /// <summary>
    /// Creates JSON serializer options with the specified converters.
    /// </summary>
    /// <param name="options"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static JsonSerializerOptions Create(this JsonSerializerOptions options,Action<JsonSerializerOptions> configure)
    {
        var jsonSerializerOptions=new JsonSerializerOptions(options);
        configure(jsonSerializerOptions);
        return jsonSerializerOptions;

    }
}
