using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Results;
using Common.Samples;
using Common.Services.Session;


namespace Server.Services.Session
{
    internal class ServiceContract : IServiceContract
    {
        public OperationResult StartSession(string meta)
        {
            throw new NotImplementedException();
        }

        public OperationResult PushSample(DroneSample droneSample)
        {
            throw new NotImplementedException();
        }

        public void EndSession()
        {
            throw new NotImplementedException();
        }
    }
}
