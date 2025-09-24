using Common.Results;
using System;

namespace Common.Services.Data
{
    public interface IDataWriter : IDisposable
    {
        OperationResult Init(bool append);
        void WriteValidData(string data);
        void WriteRejectedData(string data);
    }
}
