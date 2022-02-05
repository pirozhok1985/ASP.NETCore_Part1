using System.Collections.Concurrent;
using System.Xml;
using Microsoft.Extensions.Logging;

namespace WebStore.Logging;

public class Log4NetLoggerProvider : ILoggerProvider
{
    private readonly string _configurationFile;
    private readonly ConcurrentDictionary<string, Log4NetLogger> _loggers = new();

    public Log4NetLoggerProvider(string configurationFile)
    {
        _configurationFile = configurationFile;
    }
    public ILogger CreateLogger(string category)
    {
        return _loggers.GetOrAdd(category, (categoryName, configFile) =>
        {
            var xml = new XmlDocument();
            xml.LoadXml(configFile);
            return new Log4NetLogger(categoryName, xml["log4net"]!);
        }, _configurationFile);
        
    }

    public void Dispose() => _loggers.Clear();
}