using Common.Events;
using Common.Services.Analyzer;
using System;
using System.Configuration;

namespace Server.Services.Analyzer
{
    public class TelemetryAnalyzer : ITelemetryAnalyzer
    {
        private readonly double A_threshold;
        private double prevA_norm;
        private double Amean;
        private int AmeanCounter;

        private readonly double W_threshold;
        private double Wmean;
        private double WmeanCounter;

        private readonly double deviation_threshold;

        private readonly Action<AccelerationSpikeEventArgs> onAccelerationSpike;
        private readonly Action<OutOfBandWarningEventArgs> onOutOfBandWarning;
        private readonly Action<WindSpikeEventArgs> onWindSpike;

        public TelemetryAnalyzer(Action<AccelerationSpikeEventArgs> onASpike, Action<OutOfBandWarningEventArgs> onOutOfBand, Action<WindSpikeEventArgs> onWSpike)
        {
            prevA_norm = 0;
            Amean = 0;
            AmeanCounter = 0;

            Wmean = 0;
            WmeanCounter = 0;

            if(!Double.TryParse(ConfigurationManager.AppSettings["A_threshold"], out A_threshold)){
                A_threshold = 0.02;
            }

            if(!Double.TryParse(ConfigurationManager.AppSettings["W_threshold"], out W_threshold))
            {
                W_threshold = 0.1;
            }

            if(!Double.TryParse(ConfigurationManager.AppSettings["Deviation_threshold"], out deviation_threshold))
            {
                deviation_threshold = 25d;
            }

            onAccelerationSpike = onASpike;
            onOutOfBandWarning = onOutOfBand;
            onWindSpike = onWSpike;
        }

        public void DetectAccelerationAnomaly(double laX, double laY, double laZ)
        {
            Console.WriteLine("[Processing]: Acceleration checking...");
            double A_norm = Math.Sqrt(Math.Pow(laX, 2) + Math.Pow(laY, 2) + Math.Pow(laZ, 2));
            double deltaA = A_norm - prevA_norm;

            if (deltaA > A_threshold)
            {
                string direction = A_norm < prevA_norm ? "below" : "above";
                onAccelerationSpike?.Invoke(
                    new AccelerationSpikeEventArgs(
                        $"Acceleration spike detected: {A_norm:F4} m/s²",
                        $"Acceleration is {direction} the threshold of {A_threshold:F4} m/s²"
                    )
                );
            }

            ++AmeanCounter;
            Amean += (A_norm - Amean) / AmeanCounter;

            double lowerBound = (100d - deviation_threshold) / 100 * Amean;
            double upperBound = (100d + deviation_threshold) / 100 * Amean;
            if (A_norm < lowerBound || A_norm > upperBound)
            {
                string direction = A_norm < lowerBound ? "below" : "above";
                onOutOfBandWarning?.Invoke(
                    new OutOfBandWarningEventArgs(
                        $"Out-of-band acceleration detected: {A_norm} m/s²",
                        $"Acceleration is more than {deviation_threshold:F4}% {direction} the mean {Amean:F4} m/s²"
                    )
                );
            }

            prevA_norm = A_norm;
        }

        public void DetectWindSpikes(double windSpeed, double windAngle)
        {
            Console.WriteLine("[Processing]: Checking wind spikes...");
            double Weffect = Math.Abs(windSpeed * Math.Sin(windAngle));
            if (Weffect > W_threshold)
            {
                string direction = WmeanCounter > 0 && Weffect < Wmean ? "below" : "above";
                onWindSpike?.Invoke(
                    new WindSpikeEventArgs(
                        $"Wind spike detected: {Weffect:F4} m/s, threshold: {W_threshold:F4} m/s",
                        $"Wind speed is {direction:F4} the mean of {Wmean:F4} m/s"
                    )
                );
            }

            ++WmeanCounter;
            Wmean += (Weffect - Wmean) / WmeanCounter;
        }
    }
}
