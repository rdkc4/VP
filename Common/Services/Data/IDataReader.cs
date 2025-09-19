using System;

namespace Common.Services.Data
{
    public interface IDataReader : IDisposable
    {
        string ReadSample();
    }
}
