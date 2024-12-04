namespace CSharpEssentials.RequestResponseLogging.Infrastructure.MessageCreators;

internal sealed class LogMessageWithContextCreator(LoggingOptions loggingOptions) : BaseLogMessageCreator, ILogMessageCreator
{
    public (string logString, List<string?>? values) Create(RequestResponseContext requestResponseContext)
    {
        var valueList = loggingOptions.LoggingFields.Count > 0 ?
        new List<string?>(loggingOptions.LoggingFields.Count) : [];

        var sb = new StringBuilder();

        foreach (var logField in loggingOptions.LoggingFields)
        {
            var generatedStr = GenerateLogStringByField(requestResponseContext,loggingOptions, logField);
            sb.AppendFormat("{0}: {1}{2}", logField, "{" + logField + "}", Environment.NewLine);
            valueList.Add(generatedStr);
        }

        return (sb.ToString(), valueList);
    }
}
