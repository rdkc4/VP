namespace Common.Events.Drone
{
    public class WindSpikeEventArgs : DroneEventArgs
    {
        public WindSpikeEventArgs(string message, string direction) : base(message, direction) { }
    }
}
