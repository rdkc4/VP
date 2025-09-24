using System;

namespace Common.Exceptions
{
    public class DataReaderException : Exception
    {
        public DataReaderException() { }
        public DataReaderException(string message) : base(message) { }
    }
}
