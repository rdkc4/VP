using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services.Logger
{
    public interface ILogger : IDisposable
    {
        void Log(string _event, string message);
    }
}
