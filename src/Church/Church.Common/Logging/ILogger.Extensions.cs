using System;

namespace Church.Common.Logging
{
    public static class LoggerExtensions
    {
        public static ILogWriter With(this ILogger logger, Type source, LogLevel level)
        {
            return new LogWriter
                       {
                           Level = level,
                           Logger = logger,
                           Source = source
                       };
        }
    }
}
