using System;

namespace Client.Exceptions
{
    internal class DataReaderException : Exception
    {
        public DataReaderException() { }
        public DataReaderException(string message) : base(message) { }
    }
}
