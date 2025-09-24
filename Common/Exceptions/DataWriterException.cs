using System;

namespace Common.Exceptions
{
    public class DataWriterException : Exception
    {
        public DataWriterException() { }
        public DataWriterException(string message) : base(message) { }
    }
}
