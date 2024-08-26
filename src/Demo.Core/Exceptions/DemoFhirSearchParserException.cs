using System;
using System.Runtime.Serialization;

namespace Demo.Core.Exceptions
{
    public class DemoFhirSearchParserException : ExceptionBase
    {
        
        public DemoFhirSearchParserException()
        {
        }

        public DemoFhirSearchParserException(string message) : base(message)
        {
        }

        public DemoFhirSearchParserException(string message, Exception inner) : base(message, inner)
        {
        }

        protected DemoFhirSearchParserException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }

}