namespace Common.Events.Session
{
    public class WarningEventArgs : SessionEventArgs
    {
        public WarningEventArgs(string message) : base(message) { }
    }
}
