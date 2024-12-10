namespace CSharpEssentials.Exceptions;

public sealed class DomainException(Error error) : Exception(error.Description)
{
    public Error Error => error;
}
