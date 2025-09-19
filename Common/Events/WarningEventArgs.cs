namespace Common.Events
{
    public class WarningEventArgs
    {
        public string Message { get; }

        public WarningEventArgs(string message)
        {
            Message = message;
        }
    }
}
