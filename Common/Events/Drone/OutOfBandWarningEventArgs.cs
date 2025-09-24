namespace Common.Events.Drone
{
    public class OutOfBandWarningEventArgs : DroneEventArgs
    {
        public OutOfBandWarningEventArgs(string message, string direction) : base(message, direction) { }
    }
}
