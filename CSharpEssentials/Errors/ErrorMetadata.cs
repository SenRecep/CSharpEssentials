using System;
using System.Text;

namespace CSharpEssentials;

public sealed class ErrorMetadata : Dictionary<string, object?>
{
    public ErrorMetadata() { }
    public ErrorMetadata(IDictionary<string, object?> dictionary) : base(dictionary) { }
    public ErrorMetadata(params IEnumerable<KeyValuePair<string, object?>> collection) : base(collection) { }

    public ErrorMetadata(KeyValuePair<string, object?> keyValuePair) : this([keyValuePair]) { }
    public ErrorMetadata(string key, object? value) : this(new KeyValuePair<string, object?>(key, value)) { }


    public static ErrorMetadata CreateEmpty() => [];
    public static ErrorMetadata CreateWithStackTrace() => new("StackTrace", Environment.StackTrace);
    public static ErrorMetadata CreateWithException(Exception ex) => new("Exception", ex);
    public static ErrorMetadata CreateWithExceptionDetailed(Exception exception) => new()
    {
            { "Exception.Type", exception.GetType().Name},
            { "Exception.StackTrace", exception.StackTrace },
            { "Exception.Message", exception.Message },
    };

    public ErrorMetadata AddStackTrace()
    {
        TryAdd("StackTrace", Environment.StackTrace);
        return this;
    }

    public ErrorMetadata AddException(Exception ex)
    {
        TryAdd("Exception", ex);
        return this;
    }

    public ErrorMetadata AddExceptionDetailed(Exception exception)
    {
        TryAdd("Exception.Type", exception.GetType().Name);
        TryAdd("Exception.StackTrace", exception.StackTrace ?? Environment.StackTrace);
        TryAdd("Exception.Message", exception.Message);
        return this;
    }

    public ErrorMetadata Combine(ErrorMetadata? metadata)
    {
        if (metadata is null) return this;
        foreach (var (key, value) in metadata)
            TryAdd(key, value);
        return this;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        foreach (var (key, value) in this)
            sb.AppendLine($"{key}: {value}");
        return sb.ToString();
    }
}
