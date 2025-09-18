using Common.Results;
using Common.Samples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services.Data
{
    public interface IDataWriter : IDisposable
    {
        OperationResult Init();
        void WriteValidData(string data);
        void WriteRejectedData(string data);
    }
}
