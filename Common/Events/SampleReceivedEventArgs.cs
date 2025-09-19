namespace Common.Events
{
    public class SampleReceivedEventArgs
    {
        public string Message { get; }

        public SampleReceivedEventArgs(string message)
        {
            Message = message;
        }
    }
}
