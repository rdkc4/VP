using System;

namespace Common.Exceptions
{
    public class ServerUnavailableException : Exception
    {
        public ServerUnavailableException() { }
        public ServerUnavailableException(string message) : base(message) { }
    }
}
