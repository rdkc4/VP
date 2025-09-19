namespace Common.Events
{
    public class AccelerationSpikeEventArgs
    {
        public string Message { get; }
        public string Direction { get; }

        public AccelerationSpikeEventArgs(string message, string direction)
        {
            Message = message;
            Direction = direction;
        }
    }
}
