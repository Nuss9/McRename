using System;
using System.Runtime.Serialization;

namespace Renamer.Exceptions
{
    [Serializable]
    public class UnknownComposeModeException : Exception
    {
        public UnknownComposeModeException()
        {
        }

        public UnknownComposeModeException(string message) : base(message)
        {
        }

        public UnknownComposeModeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnknownComposeModeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}