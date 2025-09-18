using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services.Session
{
    [ServiceContract]
    public interface IDroneServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void OnTransferStarted(string message);

        [OperationContract(IsOneWay = true)]
        void OnTransferCompleted(string message);

        [OperationContract(IsOneWay = true)]
        void OnSampleReceived(string message);

        [OperationContract(IsOneWay = true)]
        void OnWarningRaised(string message);
    }
}
