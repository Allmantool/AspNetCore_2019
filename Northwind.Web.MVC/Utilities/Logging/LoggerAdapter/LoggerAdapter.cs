using System;
using Microsoft.Extensions.Logging;

namespace Northwind.Web.MVC.Utilities.Logging.LoggerAdapter
{
    public class LoggerAdapter<T> : ILoggerAdapter<T>
    {
        private readonly ILogger<T> _logger;

        public LoggerAdapter(ILogger<T> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IDisposable BeginScope<TState>(TState state) => _logger.BeginScope<TState>(state);
        

        public bool IsEnabled(LogLevel logLevel) =>_logger.IsEnabled(logLevel);


        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
            Func<TState, Exception, string> formatter)
            => _logger.Log(logLevel, eventId, state, exception, formatter);

        public void LogError(string message, params object[] args)
        {
            _logger.LogError(message, args);
        }

        public void LogError(EventId eventId, string message, params object[] args)
        {
            _logger.LogError(eventId, message, args);
        }

        public void LogError(Exception exception, string message, params object[] args)
        {
            _logger.LogError(exception, message, args);
        }

        public void LogError(EventId eventId, Exception exception, string message, params object[] args)
        {
            _logger.LogError(eventId, exception, message, args);
        }

        public void LogInformation(string message, params object[] args)
        {
            _logger.LogInformation(message, args);
        }

        public void LogInformation(EventId eventId, string message, params object[] args)
        {
            _logger.LogInformation(eventId, message, args);
        }

        public void LogInformation(Exception exception, string message, params object[] args)
        {
            _logger.LogInformation(exception, message, args);
        }

        public void LogInformation(EventId eventId, Exception exception, string message, params object[] args)
        {
            _logger.LogInformation(eventId, exception, message, args);
        }

        public void LogWarning(string message, params object[] args)
        {
            _logger.LogWarning(message, args);
        }

        public void LogWarning(EventId eventId, string message, params object[] args)
        {
            _logger.LogWarning(eventId, message, args);
        }

        public void LogWarning(Exception exception, string message, params object[] args)
        {
            _logger.LogWarning(exception, message, args);
        }

        public void LogWarning(EventId eventId, Exception exception, string message, params object[] args)
        {
            _logger.LogWarning(eventId, exception, message, args);
        }

        public void LogDebug(string message, params object[] args)
        {
            _logger.LogDebug(message, args);
        }

        public void LogDebug(EventId eventId, string message, params object[] args)
        {
            _logger.LogDebug(eventId, message, args);
        }

        public void LogDebug(Exception exception, string message, params object[] args)
        {
            _logger.LogDebug(exception, message, args);
        }

        public void LogDebug(EventId eventId, Exception exception, string message, params object[] args)
        {
            _logger.LogDebug(eventId, exception, message, args);
        }

        public void LogTrace(string message, params object[] args)
        {
            _logger.LogTrace(message, args);
        }

        public void LogTrace(EventId eventId, string message, params object[] args)
        {
            _logger.LogTrace(eventId, message, args);
        }

        public void LogTrace(Exception exception, string message, params object[] args)
        {
            _logger.LogTrace(exception, message, args);
        }

        public void LogTrace(EventId eventId, Exception exception, string message, params object[] args)
        {
            _logger.LogTrace(eventId, exception, message, args);
        }

        public void LogCritical(string message, params object[] args)
        {
            _logger.LogCritical(message, args);
        }

        public void LogCritical(EventId eventId, string message, params object[] args)
        {
            _logger.LogCritical(eventId, message, args);
        }

        public void LogCritical(Exception exception, string message, params object[] args)
        {
            _logger.LogCritical(exception, message, args);
        }

        public void LogCritical(EventId eventId, Exception exception, string message, params object[] args)
        {
            _logger.LogCritical(eventId, exception, message, args);
        }
    }
}
