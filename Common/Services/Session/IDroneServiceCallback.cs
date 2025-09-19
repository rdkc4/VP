using System.ServiceModel;

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

        [OperationContract(IsOneWay = true)]
        void OnAccelerationSpike(string message);

        [OperationContract(IsOneWay = true)]
        void OnOutOfBandWarning(string message);

        [OperationContract(IsOneWay = true)]
        void OnWindSpike(string message);
    }
}
