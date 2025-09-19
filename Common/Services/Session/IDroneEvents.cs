using Common.Events;
using System;

namespace Common.Services.Session
{
    public interface IDroneEvents
    {
        event EventHandler<TransferEventArgs> OnTransferStarted;
        event EventHandler<TransferEventArgs> OnTransferCompleted;
        event EventHandler<SampleReceivedEventArgs> OnSampleReceived;
        event EventHandler<WarningEventArgs> OnWarningRaised;
        event EventHandler<AccelerationSpikeEventArgs> OnAccelerationSpike;
        event EventHandler<OutOfBandWarningEventArgs> OnOutOfBandWarning;
        event EventHandler<WindSpikeEventArgs> OnWindSpike;
    }
}
