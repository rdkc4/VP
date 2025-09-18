using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace Common.Events
{
    [DataContract]
    public class TransferEventArgs
    {
        [DataMember]
        public string Message { get; }
        public TransferEventArgs(string message)
        {
            Message = message;
        }
    }
}
