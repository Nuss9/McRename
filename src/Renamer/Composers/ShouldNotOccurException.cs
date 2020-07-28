using System;
using System.Runtime.Serialization;

namespace Renamer.Composers
{
    [Serializable]
    internal class ShouldNotOccurException : Exception
    {
        public ShouldNotOccurException()
        {
        }

        public ShouldNotOccurException(string message) : base(message)
        {
        }

        public ShouldNotOccurException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ShouldNotOccurException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}