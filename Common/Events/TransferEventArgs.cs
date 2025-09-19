namespace Common.Events
{
    public class TransferEventArgs
    {
        public string Message { get; }

        public TransferEventArgs(string message)
        {
            Message = message;
        }
    }
}
