using System.Runtime.Serialization;

namespace PlainOldStoreApp.Ui
{
    [Serializable]
    internal class ServerException : Exception
    {
        public ServerException()
        {
        }

        public ServerException(string? message) : base(message)
        {
        }

        public ServerException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ServerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}