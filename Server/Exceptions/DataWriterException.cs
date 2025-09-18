using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Exceptions
{
    public class DataWriterException : Exception
    {
        public DataWriterException() { }
        public DataWriterException(string message) : base(message) { }
    }
}
