using System;

namespace Server.Exceptions
{
    public class DataWriterException : Exception
    {
        public DataWriterException() { }
        public DataWriterException(string message) : base(message) { }
    }
}
