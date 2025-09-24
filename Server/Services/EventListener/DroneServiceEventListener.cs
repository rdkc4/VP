using Common.Events;
using Common.Events.Drone;
using Common.Events.Session;
using Common.Services.Session;
using System;

namespace Server.Services.EventListener
{
    public class DroneServiceEventListener : IDisposable
    {
        private readonly IDroneServiceCallback callback;
        private readonly IDroneService droneService;
        private bool disposed = false;

        public DroneServiceEventListener(IDroneServiceCallback _callback, IDroneService _droneService)
        {
            callback = _callback;
            droneService = _droneService;
            Attach();
        }

        private void OnTransferStarted(object sender, SessionEventArgs e)
        {
            Console.WriteLine($"[Data Transfer]: {e.Message}");
            callback?.OnTransferStarted(e.Message);
        }

        private void OnTransferCompleted(object sender, SessionEventArgs e)
        {
            Console.WriteLine($"[Data Transfer]: {e.Message}");
            callback?.OnTransferCompleted(e.Message);
        }

        private void OnSampleReceived(object sender, SessionEventArgs e)
        {
            Console.WriteLine($"[Sample Received]: {e.Message}");
            callback?.OnSampleReceived(e.Message);
        }

        private void OnWarningRaised(object sender, SessionEventArgs e)
        {
            Console.WriteLine($"[Data Warning]: {e.Message}");
            callback?.OnWarningRaised(e.Message);
        }

        private void OnAccelerationSpike(object sender, DroneEventArgs e)
        {
            Console.WriteLine($"[Data Warning]: {e.Message} - {e.Direction}");
            callback?.OnAccelerationSpike($"{e.Message} - {e.Direction}");
        }

        private void OnOutOfBandWarning(object sender, DroneEventArgs e)
        {
            Console.WriteLine($"[Data Warning]: {e.Message} - {e.Direction}");
            callback?.OnOutOfBandWarning($"{e.Message} - {e.Direction}");
        }

        private void OnWindSpike(object sender, DroneEventArgs e)
        {
            Console.WriteLine($"[Data Warning]: {e.Message} - {e.Direction}");
            callback?.OnWindSpike($"{e.Message} - {e.Direction}");
        }

        private void Attach()
        {
            if (droneService == null) return;

            droneService.OnTransferStarted += OnTransferStarted;
            droneService.OnTransferCompleted += OnTransferCompleted;
            droneService.OnSampleReceived += OnSampleReceived;
            droneService.OnWarningRaised += OnWarningRaised;
            droneService.OnAccelerationSpike += OnAccelerationSpike;
            droneService.OnOutOfBandWarning += OnOutOfBandWarning;
            droneService.OnWindSpike += OnWindSpike;
        }

        private void Detach()
        {
            droneService.OnTransferStarted -= OnTransferStarted;
            droneService.OnTransferCompleted -= OnTransferCompleted;
            droneService.OnSampleReceived -= OnSampleReceived;
            droneService.OnWarningRaised -= OnWarningRaised;
            droneService.OnAccelerationSpike -= OnAccelerationSpike;
            droneService.OnOutOfBandWarning -= OnOutOfBandWarning;
            droneService.OnWindSpike -= OnWindSpike;
        }

        ~DroneServiceEventListener()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            Console.WriteLine("[Drone Service Event Listener]: Disposing...");
            if (disposed) return;

            if (disposing)
            {
                Detach();
            }
            disposed = true;
        }
    }
}
