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
    public class SampleReceivedEventArgs
    {
        [DataMember]
        public string Message { get; }
        public SampleReceivedEventArgs(string message)
        {
            Message = message;
        }
    }
}
