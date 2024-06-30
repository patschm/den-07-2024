using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace InfraStructure;

public class CustomLogger(CustomLoggerProvider loggerProvider) : ILogger
{
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return default;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
       return logLevel != LogLevel.None;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        loggerProvider.WriteEnry($"[{logLevel}] {formatter(state, exception)}");
    }
}

public class CustomLoggerProvider(string fileName) : ILoggerProvider
{
    private readonly StreamWriter _writer = new StreamWriter(fileName, true);

    public ILogger CreateLogger(string categoryName)
    {
        return new CustomLogger(this);
    }
    public void WriteEnry(string message)
    {
        _writer.WriteLine(message);
        _writer.Flush(); // Only for demo purposes
    }
    public void Dispose()
    {
        GC.SuppressFinalize(this);
       _writer?.Dispose();
    }
    ~CustomLoggerProvider()
    {
        // Geen garantie dat dit wordt uitgevoerd.
        _writer.Flush();
        _writer.Close();
    }
}

public static class CustomLoggerExtensions
{
    public static ILoggingBuilder AddCustomLogger(this ILoggingBuilder builder)
    {
        builder.Services.TryAddEnumerable(
            ServiceDescriptor.Singleton<ILoggerProvider, CustomLoggerProvider>(prov => 
            {
                return new CustomLoggerProvider("logfile.log");
            }));

        return builder;
    }
}
