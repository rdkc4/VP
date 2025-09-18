using Common.Results;
using Common.Samples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services.Validator
{
    public interface IDroneSampleValidator
    {
        ValidationResult validate(DroneSample droneSample);
    }
}
