namespace CSharpEssentials.RequestResponseLogging.LogWriters;

internal class LoggerFactoryLogWriter(ILoggerFactory loggerFactory,
                              LoggingOptions options) : ILogWriter
{
    private readonly ILogger _logger = loggerFactory.CreateLogger(options.LoggerCategoryName);

    public IMessageCreator MessageCreator { get; } = options.UseSeparateContext
                            ? new LogMessageWithContextCreator(options)
                            : new LogMessageCreator(options);

    public Task Write(RequestResponseContext requestResponseContext)
    {
        var (logString, values) = MessageCreator.Create(requestResponseContext);
        string?[]? parameters = null;

        if (values is not null)
            parameters = [.. values.AsReadOnly()];

        _logger.Log(options.LoggingLevel, logString, parameters ?? []);

        return Task.CompletedTask;
    }
}