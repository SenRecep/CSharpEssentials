namespace CSharpEssentials.RequestResponseLogging.Infrastructure.MessageCreators;

internal sealed class LogMessageCreator(LoggingOptions loggingOptions) : BaseLogMessageCreator, ILogMessageCreator
{
    public (string logString, List<string?>? values) Create(RequestResponseContext requestResponseContext)
    {
        var sb = new StringBuilder();

        foreach (var logField in loggingOptions.LoggingFields)
        {
            var generatedStr = GenerateLogStringByField(requestResponseContext,loggingOptions, logField);
            sb.AppendFormat("{0}: {1}{2}", logField, generatedStr, Environment.NewLine);
        }

        return (sb.ToString(), null);
    }
}