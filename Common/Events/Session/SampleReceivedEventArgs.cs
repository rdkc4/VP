namespace Common.Events.Session
{
    public class SampleReceivedEventArgs : SessionEventArgs
    {
        public SampleReceivedEventArgs(string message) : base(message) { }
    }
}
