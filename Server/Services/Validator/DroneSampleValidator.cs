using System;
using Common.Results;
using Common.Samples;
using Common.Services.Validator;
using Common.Exceptions;

namespace Server.Services.Validator
{
    internal class DroneSampleValidator : IDroneSampleValidator
    {

        private const double minWindSpeed = 0d;
        private const double maxWindSpeed = 5d;
        private const double minWindAngle = 0d;
        private const double maxWindAngle = 360d;
        private const double minAccelerationX = -1d;
        private const double maxAccelerationX = 1d;
        private const double minAccelerationY = -1d;
        private const double maxAccelerationY = 1d;
        private const double minAccelerationZ = -10d;
        private const double maxAccelerationZ = 10d;

        public void Validate(DroneSample droneSample)
        {
            Console.WriteLine("[Processing]: Data validation...");
            if (droneSample == null)
            {
                throw new InvalidSampleException("Drone sample not provided");
            }

            if (droneSample.WindSpeed <= minWindSpeed || droneSample.WindSpeed > maxWindSpeed)
            {
                throw new InvalidSampleException($"Invalid wind speed: {droneSample.WindSpeed} m/s");
            }

            if(droneSample.WindAngle < minWindAngle || droneSample.WindAngle > maxWindAngle)
            {
                throw new InvalidSampleException($"Invalid wind angle: {droneSample.WindAngle} degrees");
            }

            if (droneSample.LinearAccelerationX < minAccelerationX || droneSample.LinearAccelerationX > maxAccelerationX)
            {
                throw new InvalidSampleException($"Invalid linear acceleration X: {droneSample.LinearAccelerationX} m/s²");
            }

            if (droneSample.LinearAccelerationY < minAccelerationY || droneSample.LinearAccelerationY > maxAccelerationY)
            {
                throw new InvalidSampleException($"Invalid linear acceleration Y: {droneSample.LinearAccelerationY} m/s²");
            }

            if (droneSample.LinearAccelerationZ < minAccelerationZ || droneSample.LinearAccelerationZ > maxAccelerationZ)
            {
                throw new InvalidSampleException($"Invalid linear acceleration Z: {droneSample.LinearAccelerationZ} m/s²");
            }
        }
    }
}
