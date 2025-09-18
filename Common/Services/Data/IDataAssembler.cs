using Common.Samples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services.Data
{
    public interface IDataAssembler
    {
        string GetMeta();
        DroneSample AssembleDroneSample(string csvLine, out  double _time);
    }
}
