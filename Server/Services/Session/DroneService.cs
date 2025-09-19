using Common.Events;
using Common.Results;
using Common.Samples;
using Common.Services.Analyzer;
using Common.Services.Data;
using Common.Services.Session;
using Common.Services.Validator;
using Server.Services.Analyzer;
using Server.Services.Data;
using Server.Services.EventListener;
using Server.Services.Validator;
using System;
using System.ServiceModel;


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

        private DroneServiceEventListener droneEventListener;

        public event EventHandler<TransferEventArgs> OnTransferStarted;
        public event EventHandler<TransferEventArgs> OnTransferCompleted;
        public event EventHandler<SampleReceivedEventArgs> OnSampleReceived;
        public event EventHandler<WarningEventArgs> OnWarningRaised;
        public event EventHandler<AccelerationSpikeEventArgs> OnAccelerationSpike;
        public event EventHandler<OutOfBandWarningEventArgs> OnOutOfBandWarning;
        public event EventHandler<WindSpikeEventArgs> OnWindSpike;

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
            ValidationResult validationResult = droneSampleValidator.Validate(droneSample);
            try
            {
                if (validationResult.Success)
                {
                    dataWriter.WriteValidData($"{droneSample}");

                    telemetryAnalyzer.DetectAccelerationAnomaly(
                        droneSample.LinearAccelarationX,
                        droneSample.LinearAccelarationY,
                        droneSample.LinearAccelarationZ
                    );

                    telemetryAnalyzer.DetectWindSpikes(droneSample.WindSpeed, droneSample.WindAngle);

                    return new OperationResult(true, "");
                }
                else
                {
                    OnWarningRaised?.Invoke(this, new WarningEventArgs(validationResult.Message));
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
        }

    }
}
