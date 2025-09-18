using Common.Results;
using Common.Samples;
using Common.Services.Validator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services.Validator
{
    internal class DroneSampleValidator : IDroneSampleValidator
    {
        public ValidationResult validate(DroneSample droneSample)
        {
            if(droneSample == null)
            {
                return new ValidationResult(false, "Drone sample not provided");
            }

            if(droneSample.WindSpeed <= 0)
            {
                return new ValidationResult(false, "Invalid wind speed");
            }

            return new ValidationResult(true, "");
        }
    }
}
