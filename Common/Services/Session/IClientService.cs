using System;

namespace Common.Services.Session
{
    public interface IClientService : IDroneServiceCallback, IDisposable
    {
        void Run();
    }
}
