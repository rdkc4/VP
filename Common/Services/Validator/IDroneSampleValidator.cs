using Common.Results;
using Common.Samples;

namespace Common.Services.Validator
{
    public interface IDroneSampleValidator
    {
        ValidationResult Validate(DroneSample droneSample);
    }
}
