using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Common.Results;
using Common.Samples;
using Common.Services.Data;
using Common.Services.Session;
using Common.Services.Validator;
using Server.Services.Data;
using Server.Services.Validator;


namespace Server.Services.Session
{
    internal class ServiceContract : IServiceContract
    {
        private readonly IDataWriter dataWriter = new DataWriter();
        private readonly IDroneSampleValidator droneSampleValidator = new DroneSampleValidator();

        public OperationResult StartSession(string meta)
        {
            try
            {
                var initResult = dataWriter.Init();
                if (initResult.Success)
                {
                    dataWriter.WriteValidData(meta);
                    dataWriter.WriteRejectedData(meta);
                }

                return initResult;
            }
            catch (Exception ex)
            {
                return new OperationResult(false, ex.Message);
            }
        }

        public OperationResult PushSample(DroneSample droneSample)
        {
            ValidationResult validationResult = droneSampleValidator.validate(droneSample);
            try
            {
                Console.WriteLine("recv");
                if (validationResult.Success)
                {
                    Console.WriteLine($"valid {droneSample}");
                    dataWriter.WriteValidData(droneSample.ToString());
                    return new OperationResult(true, "");
                }
                else
                {
                    Console.WriteLine($"reject {droneSample}");
                    dataWriter.WriteRejectedData(droneSample.ToString());
                    return new OperationResult(false, validationResult.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new OperationResult(false, ex.Message);
            }
        }

        public void EndSession()
        {
            dataWriter.Dispose();
        }
    }
}
