using System;
using System.Text;

namespace Church.Common.Structures
{
    public class ErrorException : ApplicationException
    {
        public Error Error { get; private set; }
        private readonly string _stack;

        public ErrorException(Error error)
        {
            Error = error;
        }

        public ErrorException(string message, Error error) : base(message)
        {
            Error = error;
            _stack = Environment.StackTrace;
        }

        public override string ToString()
        {
            var sb = new StringBuilder(base.ToString());
            if (Error != null)
            {
                try
                {
                    sb.AppendLine().AppendFormat(" SystemCode:{0}", Error.SystemCode);
                    sb.AppendLine().AppendFormat(" Code:{0}", Error.Code);
                }
                    // ReSharper disable EmptyGeneralCatchClause
                catch
                    // ReSharper restore EmptyGeneralCatchClause
                {

                }
            }

            sb.AppendLine().Append(_stack);
            return sb.ToString();
        }
    }
}