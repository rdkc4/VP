using Common.Results;
using Common.Samples;
using System.ServiceModel;

namespace Common.Services.Session
{
    [ServiceContract(CallbackContract = typeof(IDroneServiceCallback))]
    public interface IDroneService : IDroneEvents
    {
        [OperationContract]
        OperationResult StartSession(string meta);

        [OperationContract]
        OperationResult PushSample(DroneSample droneSample);

        [OperationContract]
        void EndSession();
    }
}
