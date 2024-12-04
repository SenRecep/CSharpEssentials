
namespace CSharpEssentials.RequestResponseLogging.LogWriters;

internal class NullLogWriter : ILogWriter
{
    public IMessageCreator? MessageCreator { get; }

    public Task Write(RequestResponseContext requestResponseContext) => Task.CompletedTask;
}
