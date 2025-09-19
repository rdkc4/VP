using System;
using System.Runtime.Serialization;

namespace Common.Samples
{
    [DataContract]
    public class DroneSample
    {
        private double linearAccelarationX;
        private double linearAccelarationY;
        private double linearAccelarationZ;
        private double windSpeed;
        private double windAngle;
        private DateTime time;

        [DataMember]
        public double LinearAccelarationX { get => linearAccelarationX; set => linearAccelarationX = value; }

        [DataMember]
        public double LinearAccelarationY { get => linearAccelarationY; set => linearAccelarationY = value; }

        [DataMember]
        public double LinearAccelarationZ { get => linearAccelarationZ; set => linearAccelarationZ = value; }

        [DataMember]
        public double WindSpeed { get => windSpeed; set => windSpeed = value; }

        [DataMember]
        public double WindAngle { get => windAngle; set => windAngle = value; }

        [DataMember]
        public DateTime Time { get => time; set => time = value; }

        public DroneSample(double laX, double laY, double laZ, double ws, double wa, DateTime time)
        {
            LinearAccelarationX = laX;
            LinearAccelarationY = laY;
            LinearAccelarationZ = laZ;
            WindSpeed = ws;
            WindAngle = wa;
            Time = time;
        }

        public override string ToString()
        {
            return $"{LinearAccelarationX},{LinearAccelarationY},{LinearAccelarationZ},{WindSpeed},{WindAngle},{Time:dd/MM/yyyy HH:mm:ss:fff}";
        }
    }
}
