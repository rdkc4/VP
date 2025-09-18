using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Exceptions
{
    internal class DataReaderException : Exception
    {
        public DataReaderException() { }
        public DataReaderException(string message) : base(message) { }
    }
}
