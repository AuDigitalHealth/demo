using System;
using System.Runtime.Serialization;

namespace Demo.Core.Exceptions
{
    [Serializable]
    public class ExceptionBase : Exception
    {
        public ExceptionBase()
        {
        }

        public ExceptionBase(string message) : base(message)
        {
        }

        public ExceptionBase(string message, Exception inner) : base(message, inner)
        {
        }

        protected ExceptionBase(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
