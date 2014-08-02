using System;

namespace Church.Common.Logging
{
    public interface ILogWriter
    {
        ILogger Logger { get; }
        LogLevel Level { get; }
        Type Source { get; }

        ILogWriter Log(string message);
        ILogWriter Log(Func<string> func);
        ILogWriter Log(string messageFormat, params object[] args);
    }
}
