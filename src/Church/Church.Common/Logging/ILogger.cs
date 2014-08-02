using System;

namespace Church.Common.Logging
{
    /// <summary>
    /// Provides access to logging.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Logs to the underlying logging provider.
        /// </summary>
        /// <param name="level">The log level.</param>
        /// <param name="source">The log source.</param>
        /// <param name="messageGenerator">A Func(string) that returns the message to be logged.</param>
        /// <remarks>
        /// If the logging level isn't enabled, the messageGenerator is not called.
        /// </remarks>
        void Log(Type source, LogLevel level, Func<string> messageGenerator);

        /// <summary>
        /// Logs to the underlying logging provider.
        /// </summary>
        /// <param name="level">The log level.</param>
        /// <param name="source">The log source.</param>
        /// <param name="message">The message to log.</param>
        void Log(Type source, LogLevel level, string message);

        /// <summary>
        /// Logs to the underlying logging provider.
        /// </summary>
        /// <param name="level">The log level.</param>
        /// <param name="source">The log source.</param>
        /// <param name="message">The message format to log.</param>
        /// <param name="args">The object args used in the message formatting.</param>
        void LogWithFormat(Type source, LogLevel level, string message, params object[] args);

        /// <summary>
        /// Logs an exception with the underlying logging provider.
        /// </summary>
        /// <param name="source">The log source.</param>
        /// <param name="message">The message format to log.</param>
        /// <param name="ex">The exception to log.</param>
        void Exception(Type source, string message, Exception ex);


        void Flush();
    }
}
