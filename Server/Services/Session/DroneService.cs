using Common.Results;
using Common.Samples;
using Common.Events.Session;
using Common.Services.Analyzer;
using Common.Services.Data;
using Common.Services.Session;
using Common.Services.Validator;
using Common.Exceptions;
using Server.Services.Analyzer;
using Server.Services.Data;
using Server.Services.EventListener;
using Server.Services.Validator;
using System;
using System.ServiceModel;
using Common.Services.EventHandlers;

namespace Server.Services.Session
{
    public class DroneService : IDroneService
    {
        private readonly IDroneSampleValidator droneSampleValidator = new DroneSampleValidator();
        private IDataWriter dataWriter;
        private ITelemetryAnalyzer telemetryAnalyzer;

        private IClientChannel clientChannel;
        private bool sessionActive = false;
        private bool sessionDisposed = false;
        private int sessionCounter = 0;

        private DroneServiceEventListener droneEventListener;

        public event SessionEventHandler OnTransferStarted;
        public event SessionEventHandler OnTransferCompleted;
        public event SessionEventHandler OnSampleReceived;
        public event SessionEventHandler OnWarningRaised;

        public event DroneEventHandler OnAccelerationSpike;
        public event DroneEventHandler OnOutOfBandWarning;
        public event DroneEventHandler OnWindSpike;

        public OperationResult StartSession(string meta)
        {
            dataWriter = new DataWriter();

            IDroneServiceCallback callback = OperationContext.Current.GetCallbackChannel<IDroneServiceCallback>();
            droneEventListener = new DroneServiceEventListener(callback, this);
            telemetryAnalyzer = new TelemetryAnalyzer(
                e => OnAccelerationSpike?.Invoke(this, e),
                e => OnOutOfBandWarning?.Invoke(this, e),
                e => OnWindSpike?.Invoke(this, e)
            );

            OnTransferStarted?.Invoke(this, new TransferEventArgs("Transfer Started"));

            sessionActive = true;
            sessionDisposed = false;

            clientChannel = OperationContext.Current.GetCallbackChannel<IClientChannel>();
            if (clientChannel is ICommunicationObject channel)
            {
                channel.Faulted += (sender, e) => { sessionActive = false; EndSession(); };
            }

            try
            {
                var initResult = dataWriter.Init(append: sessionCounter > 0);
                if (initResult.Success && sessionCounter == 0)
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

            Console.WriteLine("[Processing]: Starting...");

            try
            {
                droneSampleValidator.Validate(droneSample);
            }
            catch (InvalidSampleException sampleException)
            {
                Console.WriteLine($"[Processing error]: {sampleException.Message}");

                try
                {
                    dataWriter.WriteRejectedData($"{droneSample}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Processing error]: {ex.Message}");
                    return new OperationResult(false, ex.Message);
                }

                return new OperationResult(false, sampleException.Message);
            }

            try
            {
                dataWriter.WriteValidData($"{droneSample}");
                telemetryAnalyzer.DetectAccelerationAnomaly(
                    droneSample.LinearAccelerationX,
                    droneSample.LinearAccelerationY,
                    droneSample.LinearAccelerationZ
                );

                telemetryAnalyzer.DetectWindSpikes(droneSample.WindSpeed, droneSample.WindAngle);

                return new OperationResult(true, "");
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
            if (sessionDisposed) return;

            if (sessionActive)
            {
                OnTransferCompleted?.Invoke(this, new TransferEventArgs("Transfer Completed"));
            }
            else
            {
                Console.WriteLine("[Data Transfer]: Transfer Canceled");
            }

            dataWriter?.Dispose();
            droneEventListener?.Dispose();

            sessionDisposed = true;
            sessionActive = false;
            ++sessionCounter;
        }

    }
}
