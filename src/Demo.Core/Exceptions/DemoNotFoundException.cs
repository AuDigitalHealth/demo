using System;
using System.Runtime.Serialization;

namespace Demo.Core.Exceptions
{
    public class DemoNotFoundException : ExceptionBase
    {
        
        public DemoNotFoundException()
        {
        }

        public DemoNotFoundException(string message) : base(message)
        {
        }

        public DemoNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }

        protected DemoNotFoundException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}