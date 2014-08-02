using System;

namespace Church.Common.Logging
{
    public class LogWriter : ILogWriter
    {
        public ILogger Logger { get; set; }
        public LogLevel Level { get; set; }
        public Type Source { get; set; }

        public ILogWriter Log(string message)
        {
            Logger.Log(Source, Level, message);
            return this;
        }

        public ILogWriter Log(Func<string> func)
        {
            Logger.Log(Source, Level, func);
            return this;
        }

        public ILogWriter Log(string messageFormat, params object[] args)
        {
            Logger.LogWithFormat(Source, Level, messageFormat, args);
            return this;
        }
    }
}
