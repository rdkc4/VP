using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exceptions
{
    public class InvalidSampleException : Exception
    {
        public InvalidSampleException() { }
        public InvalidSampleException(string message) : base(message) { }
    }
}
