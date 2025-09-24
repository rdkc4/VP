namespace Common.Events.Drone
{
    public class AccelerationSpikeEventArgs : DroneEventArgs
    {
        public AccelerationSpikeEventArgs(string message, string direction) : base(message, direction) { }
    }
}
