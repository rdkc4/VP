using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services.Session
{
    public interface IClientService : IDroneServiceCallback, IDisposable
    {
        void Run();
    }
}
