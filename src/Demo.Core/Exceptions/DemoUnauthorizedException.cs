using System;
using System.Runtime.Serialization;

namespace Demo.Core.Exceptions
{
    public class DemoUnauthorizedException : ExceptionBase
    {
        public DemoUnauthorizedException()
        {
        }

        public DemoUnauthorizedException(string message) : base(message)
        {
        }

        public DemoUnauthorizedException(string message, Exception inner) : base(message, inner)
        {
        }

        protected DemoUnauthorizedException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
