using Common.Services.EventHandlers;

namespace Common.Services.Session
{
    public interface IDroneEvents
    {
        event SessionEventHandler OnTransferStarted;
        event SessionEventHandler OnTransferCompleted;
        event SessionEventHandler OnSampleReceived;
        event SessionEventHandler OnWarningRaised;
        event DroneEventHandler OnAccelerationSpike;
        event DroneEventHandler OnOutOfBandWarning;
        event DroneEventHandler OnWindSpike;
    }
}
