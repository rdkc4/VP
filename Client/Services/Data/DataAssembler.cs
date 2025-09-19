using Common.Samples;
using Common.Services.Data;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Client.Services.Data
{
    internal class DataAssembler : IDataAssembler
    {
        private const string meta = "linear_acceleration_x,linear_acceleration_y,linear_acceleration_z,wind_speed,wind_angle,time";
        private const string linearAccelerationX = "linear_acceleration_x";
        private const string linearAccelerationY = "linear_acceleration_y";
        private const string linearAccelerationZ = "linear_acceleration_z";
        private const string windSpeed = "wind_speed";
        private const string windAngle = "wind_angle";
        private const string time = "time";

        private readonly Dictionary<string, int> metaDictionary;

        public DataAssembler(string csvHeader)
        {
            string[] headerMetaData = csvHeader.Split(',');
            metaDictionary = new Dictionary<string, int>(headerMetaData.Length);

            for (int i = 0; i < headerMetaData.Length; ++i)
            {
                metaDictionary.Add(headerMetaData[i], i);
            }
        }

        public string GetMeta()
        {
            return meta;
        }

        public DroneSample AssembleDroneSample(string csvLine, out double _time)
        {
            string[] data = csvLine.Split(',');

            try
            {
                _time = Double.Parse(data[metaDictionary[time]], CultureInfo.InvariantCulture);

                return new DroneSample(
                    Double.Parse(data[metaDictionary[linearAccelerationX]], CultureInfo.InvariantCulture),
                    Double.Parse(data[metaDictionary[linearAccelerationY]], CultureInfo.InvariantCulture),
                    Double.Parse(data[metaDictionary[linearAccelerationZ]], CultureInfo.InvariantCulture),
                    Double.Parse(data[metaDictionary[windSpeed]], CultureInfo.InvariantCulture),
                    Double.Parse(data[metaDictionary[windAngle]], CultureInfo.InvariantCulture),
                    DateTime.Now
                );
            }
            catch
            {
                _time = 0d;
                return null;
            }
        }

    }
}
