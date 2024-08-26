using System;
using System.Runtime.Serialization;

namespace Demo.Core.Exceptions
{
    [Serializable]
    public class DemoValidationException : ExceptionBase
    {
        public DemoValidationException()
        {
        }

        public DemoValidationException(string message) : base(message)
        {
        }

        public DemoValidationException(string message, Exception inner) : base(message, inner)
        {
        }

        protected DemoValidationException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
