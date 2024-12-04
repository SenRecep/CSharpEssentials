namespace CSharpEssentials;

/// <summary>
/// Error types.
/// </summary>
[StringEnum]
public enum ErrorType:byte
{
    Failure,
    Unexpected,
    Validation,
    Conflict,
    NotFound,
    Unauthorized,
    Forbidden,
    Unknown
}