namespace Common.Events
{
    public class OutOfBandWarningEventArgs
    {
        public string Message { get; }
        public string Direction { get; }

        public OutOfBandWarningEventArgs(string message, string direction)
        {
            Message = message;
            Direction = direction;
        }
    }
}
