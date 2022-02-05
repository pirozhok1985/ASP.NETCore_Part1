using System.Configuration;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace WebStore.Logging;

public static class Log4NetLoggerFactoryExtensions
{
    private static string CheckFilePath(string filePath)
    {
        if (filePath == string.Empty)
            throw new ArgumentException("Не указан путь к файлу конфигурации");
        return Path.IsPathRooted(filePath) 
            ? filePath 
            : Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!, filePath);
    }

    public static ILoggingBuilder AddLog4Net(this ILoggingBuilder builder, string configurationFile = "log4net.config")
    {
        return builder.AddProvider(new Log4NetLoggerProvider(CheckFilePath(configurationFile)));
    }
}