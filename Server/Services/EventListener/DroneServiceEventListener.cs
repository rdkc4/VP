using Common.Events;
using Common.Services.Session;
using Server.Services.Session;
using System;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services.EventListener
{
    public class DroneServiceEventListener : IDisposable
    {
        private readonly IDroneServiceCallback callback;
        private bool disposed = false;

        public DroneServiceEventListener()
        {
            callback = OperationContext.Current.GetCallbackChannel<IDroneServiceCallback>();
            Attach();
        }

        public void OnTransferStarted(object sender, TransferEventArgs e)
        {
            Console.WriteLine($"[Data Transfer]: {e.Message}");
            callback?.OnTransferStarted(e.Message);
        }

        public void OnTransferCompleted(object sender, TransferEventArgs e)
        {
            Console.WriteLine($"[Data Transfer]: {e.Message}");
            callback?.OnTransferCompleted(e.Message);
        }

        public void OnSampleReceived(object sender, SampleReceivedEventArgs e)
        {
            Console.WriteLine($"[Sample Received]: {e.Message}");
            callback?.OnSampleReceived(e.Message);
        }

        public void OnWarningRaised(object sender, WarningEventArgs e)
        {
            Console.WriteLine($"[Data Warning]: {e.Message}");
            callback?.OnWarningRaised(e.Message);
        }

        private void Attach()
        {
            DroneService.OnTransferStarted += OnTransferStarted;
            DroneService.OnTransferCompleted += OnTransferCompleted;
            DroneService.OnSampleReceived += OnSampleReceived;
            DroneService.OnWarningRaised += OnWarningRaised;
        }

        private void Detach()
        {
            DroneService.OnTransferStarted -= OnTransferStarted;
            DroneService.OnTransferCompleted -= OnTransferCompleted;
            DroneService.OnSampleReceived -= OnSampleReceived;
            DroneService.OnWarningRaised -= OnWarningRaised;
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
            if (disposed) return;

            if (disposing)
            {
                Detach();
            }
            disposed = true;
        }
    }
}
