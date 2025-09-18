using Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services.Analyzer
{
    public class TelemetryAnalyzer
    {
        private readonly double A_threshold;
        private double prevA_norm;
        private double Amean;
        private int meanCounter;
        private readonly double W_threshold;

        public TelemetryAnalyzer() 
        {
            prevA_norm = 0;
            Amean = 0;
            meanCounter = 0;
        }

        public void DetectAccelerationAnomaly(double laX, double laY, double laZ)
        {
            double A_norm = Math.Sqrt(Math.Pow(laX, 2) + Math.Pow(laY, 2) + Math.Pow(laZ, 2));
            double deltaA = A_norm - prevA_norm;

            if(deltaA > A_threshold)
            {
                string direction = deltaA < 0 ? "below" : "above";
                // raise event
            }

            Amean = (Amean + A_norm) / ++meanCounter;

            if(A_norm < 0.75 * Amean || A_norm > 1.25 * Amean)
            {
                string direction = A_norm < 0.75 * Amean ? "below" : "above";
                // raise event
            }

            prevA_norm = A_norm;
        }

        public void DetectWindSpikes(double windSpeed, double windAngle)
        {
            double Weffect = Math.Abs(windSpeed * Math.Sin(windAngle));
            if(Weffect > W_threshold)
            {
                string direction = windAngle < 0 ? "below" : "above";
                // raise event
            }
        }
    }
}
