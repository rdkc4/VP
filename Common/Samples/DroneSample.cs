using System;
using System.Runtime.Serialization;

namespace Common.Samples
{
    [DataContract]
    public class DroneSample
    {
        private double linearAccelerationX;
        private double linearAccelerationY;
        private double linearAccelerationZ;
        private double windSpeed;
        private double windAngle;
        private DateTime time;

        [DataMember]
        public double LinearAccelerationX { get => linearAccelerationX; set => linearAccelerationX = value; }

        [DataMember]
        public double LinearAccelerationY { get => linearAccelerationY; set => linearAccelerationY = value; }

        [DataMember]
        public double LinearAccelerationZ { get => linearAccelerationZ; set => linearAccelerationZ = value; }

        [DataMember]
        public double WindSpeed { get => windSpeed; set => windSpeed = value; }

        [DataMember]
        public double WindAngle { get => windAngle; set => windAngle = value; }

        [DataMember]
        public DateTime Time { get => time; set => time = value; }

        public DroneSample(double laX, double laY, double laZ, double ws, double wa, DateTime time)
        {
            LinearAccelerationX = laX;
            LinearAccelerationY = laY;
            LinearAccelerationZ = laZ;
            WindSpeed = ws;
            WindAngle = wa;
            Time = time;
        }

        public override string ToString()
        {
            return $"{LinearAccelerationX:F4},{LinearAccelerationY:F4},{LinearAccelerationZ:F4},{WindSpeed:F4},{WindAngle:F4},{Time:dd/MM/yyyy HH:mm:ss:fff}";
        }
    }
}
