using System;
using System.Runtime.Serialization;

namespace Service.Common.Exceptions
{
    [Serializable]
    public class ForbiddenException : Exception
    {
        public ForbiddenException()
        {

        }

        public ForbiddenException(string message) : base(message)
        {

        }

        protected ForbiddenException(SerializationInfo info, StreamingContext context)
          : base(info, context)
        {
        }

    }
}
