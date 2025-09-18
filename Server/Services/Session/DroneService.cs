using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Common.Events;
using Common.Results;
using Common.Samples;
using Common.Services.Data;
using Common.Services.Session;
using Common.Services.Validator;
using Server.Services.Data;
using Server.Services.EventListener;
using Server.Services.Validator;


namespace Server.Services.Session
{
    public class DroneService : IDroneService
    {
        private readonly IDataWriter dataWriter = new DataWriter();
        private readonly IDroneSampleValidator droneSampleValidator = new DroneSampleValidator();

        private DroneServiceEventListener eventListener;

        public static event EventHandler<TransferEventArgs> OnTransferStarted;
        public static event EventHandler<TransferEventArgs> OnTransferCompleted;
        public static event EventHandler<SampleReceivedEventArgs> OnSampleReceived;
        public static event EventHandler<WarningEventArgs> OnWarningRaised;
        
        public OperationResult StartSession(string meta)
        {
            eventListener = new DroneServiceEventListener();
            OnTransferStarted?.Invoke(this, new TransferEventArgs("Transfer Started"));
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
            OnSampleReceived?.Invoke(this, new SampleReceivedEventArgs($"{droneSample}"));

            Console.WriteLine("[Processing]: Started");
            ValidationResult validationResult = droneSampleValidator.validate(droneSample);
            try
            {
                if (validationResult.Success)
                {
                    dataWriter.WriteValidData($"{droneSample}");
                    return new OperationResult(true, "");
                }
                else
                {
                    dataWriter.WriteRejectedData($"{droneSample}");
                    return new OperationResult(false, validationResult.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Processing Error]: {ex.Message}");
                return new OperationResult(false, ex.Message);
            }
            finally
            {
                Console.WriteLine("[Processing]: Completed");
            }
        }

        public void EndSession()
        {
            dataWriter?.Dispose();
            OnTransferCompleted?.Invoke(this, new TransferEventArgs("Transfer Completed"));
            eventListener?.Dispose();
        }
    }
}
