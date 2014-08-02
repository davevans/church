using System;

namespace Church.Common.Logging
{
    public sealed class ConsoleLogger : ILogger
    {
        private const string FORMAT = "L: {0} S: {1} Message: {2}";

        public void Log(Type source, LogLevel level, Func<string> messageGenerator)
        {
            Console.WriteLine(string.Format(FORMAT, level, source, messageGenerator()));
        }

        public void Log(Type source, LogLevel level, string message)
        {
            Console.WriteLine(string.Format(FORMAT, level, source, message));
        }

        public void LogWithFormat(Type source, LogLevel level, string message, params object[] args)
        {
            Console.WriteLine(string.Format(FORMAT, level, source, string.Format(message, args)));
        }

        public void Exception(Type source, string message, Exception ex)
        {
            Log(source, LogLevel.Error, message + ex);
        }

        public void Flush()
        {
            
        }
    }
}
