using Common.Results;
using Common.Samples;
using Common.Services.Validator;

namespace Server.Services.Validator
{
    internal class DroneSampleValidator : IDroneSampleValidator
    {
        public ValidationResult Validate(DroneSample droneSample)
        {
            if (droneSample == null)
            {
                return new ValidationResult(false, "Drone sample not provided");
            }

            if (droneSample.WindSpeed <= 0 || droneSample.WindSpeed > 1.1)
            {
                return new ValidationResult(false, "Invalid wind speed");
            }

            return new ValidationResult(true, "");
        }
    }
}
