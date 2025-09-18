using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Common.Results;
using Common.Samples;

namespace Common.Services.Session
{
    [ServiceContract(CallbackContract = typeof(IDroneServiceCallback))]
    public interface IDroneService
    {
        [OperationContract]
        OperationResult StartSession(string meta);

        [OperationContract]
        OperationResult PushSample(DroneSample droneSample);

        [OperationContract]
        void EndSession();
    }
}
