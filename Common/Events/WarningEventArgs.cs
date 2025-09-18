using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.Events
{
    [DataContract]
    public class WarningEventArgs
    {
        [DataMember]
        public string Message { get; }

        public WarningEventArgs(string message)
        {
            Message = message;
        }
    }
}
