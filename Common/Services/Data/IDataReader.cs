using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services.Data
{
    public interface IDataReader : IDisposable
    {
        string ReadSample();
    }
}
