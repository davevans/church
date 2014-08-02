using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Church.Common.Service;
using log4net;
using log4net.Config;

namespace Church.Common.Logging
{
    /// <summary>
    /// An implenentation if the ILogger interface, that uses log4net as the underlying 
    /// log provider.
    /// </summary>
    public class Log4NetLogger : ILogger, IExtendedService
    {
        private static readonly BlockingCollection<QueueLog> Queue = new BlockingCollection<QueueLog>();

        static Log4NetLogger()
        {
            XmlConfigurator.Configure();

            Task.Factory.StartNew(() =>
            {
                while (!Queue.IsCompleted)
                {
                    try
                    {
                        var next = Queue.Take();
                        InternalLog(next);
                    }
                    catch (InvalidOperationException)
                    {
                        //this happens if the queue is market as complete.
                    }
                }
            }, TaskCreationOptions.LongRunning);
        }

        public void Flush()
        {
            //wait will queue is not empty;
            while (Queue.Count > 0)
            {
                Thread.Sleep(1000);
            }
        }

        public void Log(Type source, LogLevel level, Func<string> messageGenerator)
        {
            Queue.Add(new QueueLog { Source = source, Level = level, MessageGenerator = messageGenerator });
        }

        public void Log(Type source, LogLevel level, string message)
        {
            Queue.Add(new QueueLog { Source = source, Level = level, Message = message });
        }

        private static void InternalLog(QueueLog message)
        {
            var iLog = LogManager.GetLogger(message.Source);

            switch (message.Level)
            {
                case LogLevel.Debug:
                    if (iLog.IsDebugEnabled)
                    {
                        if (message.MessageGenerator != null)
                        {
                            message.Message = message.MessageGenerator();
                        }

                        iLog.Debug(message.Message);
                    }
                    break;
                case LogLevel.Info:
                    if (iLog.IsInfoEnabled)
                    {
                        if (message.MessageGenerator != null)
                        {
                            message.Message = message.MessageGenerator();
                        }

                        iLog.Info(message.Message);
                    }
                    break;
                case LogLevel.Error:
                    if (iLog.IsErrorEnabled)
                    {
                        if (message.MessageGenerator != null)
                        {
                            message.Message = message.MessageGenerator();
                        }

                        if (message.ThrownException != null)
                        {
                            iLog.Error(message.Message, message.ThrownException);
                        }
                        else
                        {
                            iLog.Error(message.Message);
                        }

                    }
                    break;
            }
        }


        public void LogWithFormat(Type source, LogLevel level, string message, params object[] args)
        {
            Log(source, level, string.Format(message, args));
        }

        public void Exception(Type source, string message, Exception ex)
        {
            Queue.Add(new QueueLog
            {
                Level = LogLevel.Error,
                Message = message,
                Source = source,
                ThrownException = ex
            });
        }

        private sealed class QueueLog
        {
            public Type Source;
            public LogLevel Level;
            public string Message;
            public Func<string> MessageGenerator;
            public Exception ThrownException;
        }

        public void Start()
        {

        }

        public void Stop()
        {
            Flush();
        }

        public void PreStart()
        {

        }

        public void PreStop()
        {

        }
    }
}

