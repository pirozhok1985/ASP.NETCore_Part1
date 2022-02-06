using System.Reflection;
using System.Xml;
using log4net;
using log4net.Core;
using Microsoft.Extensions.Logging;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace WebStore.Logging;

public class Log4NetLogger : ILogger
{
    private readonly ILog _log;
    public Log4NetLogger(string category, XmlElement configuration)
    {
        var logger_repo = LoggerManager.CreateRepository(Assembly.GetEntryAssembly(),
            typeof(log4net.Repository.Hierarchy.Hierarchy));
        _log = LogManager.GetLogger(logger_repo.Name, category);
        log4net.Config.XmlConfigurator.Configure(configuration);
    }

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state, Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        if (formatter is null) throw new ArgumentNullException(nameof(formatter));
        if(!IsEnabled(logLevel)) return;

        var logString = formatter(state, exception);
        if(string.IsNullOrWhiteSpace(logString) && exception == null) return;

        switch (logLevel)
        {
            default: throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, String.Empty);
            case LogLevel.None: 
                break;
            case LogLevel.Debug:
            case LogLevel.Trace:
                _log.Debug(logString);
                break;
            case LogLevel.Critical:
                _log.Fatal(logString, exception);
                break;
            case LogLevel.Error:
                _log.Error(logString, exception);
                break;
            case LogLevel.Information:
                _log.Info(logString);
                break;
            case LogLevel.Warning:
                _log.Warn(logString);
                break;
        }
    }

    public bool IsEnabled(LogLevel logLevel) => logLevel switch
    {
        LogLevel.Trace => _log.IsDebugEnabled,
        LogLevel.Debug => _log.IsDebugEnabled,
        LogLevel.Information => _log.IsInfoEnabled,
        LogLevel.Warning => _log.IsWarnEnabled,
        LogLevel.Error => _log.IsErrorEnabled,
        LogLevel.Critical => _log.IsFatalEnabled,
        LogLevel.None => false,
        _ => throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null)
    };

    public IDisposable BeginScope<TState>(TState state) => null!;
}