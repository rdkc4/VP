namespace Common.Events
{
    public class WindSpikeEventArgs
    {
        public string Message { get; }
        public string Direction { get; }

        public WindSpikeEventArgs(string message, string direction)
        {
            Message = message;
            Direction = direction;
        }
    }
}
