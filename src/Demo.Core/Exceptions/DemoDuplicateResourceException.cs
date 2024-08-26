using System.Runtime.Serialization;

namespace Demo.Core.Exceptions
{
    public class DemoDuplicateResourceException: ExceptionBase
    {
        
        public DemoDuplicateResourceException()
        {
        }

        public DemoDuplicateResourceException(string message) : base(message)
        {
        }

        public DemoDuplicateResourceException(string message, Exception inner) : base(message, inner)
        {
        }

        protected DemoDuplicateResourceException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}