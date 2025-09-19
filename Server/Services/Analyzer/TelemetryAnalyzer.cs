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

        private const double defaultA_threshold = 0.02;
        private const double defaultW_threshold = 0.1;

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

            bool success = Double.TryParse(ConfigurationManager.AppSettings["A_threshold"], out double tempCfgThreshold);
            A_threshold = success ? tempCfgThreshold : defaultA_threshold;

            success = Double.TryParse(ConfigurationManager.AppSettings["W_threshold"], out tempCfgThreshold);
            W_threshold = success ? tempCfgThreshold : defaultW_threshold;

            onAccelerationSpike = onASpike;
            onOutOfBandWarning = onOutOfBand;
            onWindSpike = onWSpike;
        }

        public void DetectAccelerationAnomaly(double laX, double laY, double laZ)
        {
            double A_norm = Math.Sqrt(Math.Pow(laX, 2) + Math.Pow(laY, 2) + Math.Pow(laZ, 2));
            double deltaA = A_norm - prevA_norm;

            if (deltaA > A_threshold)
            {
                string direction = A_norm < prevA_norm ? "below" : "above";
                onAccelerationSpike?.Invoke(
                    new AccelerationSpikeEventArgs(
                        $"Acceleration spike detected: {A_norm} m/s²",
                        $"Acceleration is {direction} the threshold of {A_threshold} m/s²"
                    )
                );
            }

            ++AmeanCounter;
            Amean += (A_norm - Amean) / AmeanCounter;

            if (A_norm < 0.75 * Amean || A_norm > 1.25 * Amean)
            {
                string direction = A_norm < 0.75 * Amean ? "below" : "above";
                onOutOfBandWarning?.Invoke(
                    new OutOfBandWarningEventArgs(
                        $"Out-of-band acceleration detected: {A_norm} m/s²",
                        $"Acceleration is 25% {direction} the mean {Amean} m/s²"
                    )
                );
            }

            prevA_norm = A_norm;
        }

        public void DetectWindSpikes(double windSpeed, double windAngle)
        {
            double Weffect = Math.Abs(windSpeed * Math.Sin(windAngle));
            if (Weffect > W_threshold)
            {
                string direction = WmeanCounter > 0 && Weffect < Wmean ? "below" : "above";
                onWindSpike?.Invoke(
                    new WindSpikeEventArgs(
                        $"Wind spike detected: {Weffect} m/s, threshold: {W_threshold} m/s",
                        $"Wind speed is {direction} the mean of {Wmean} m/s"
                    )
                );
            }

            ++WmeanCounter;
            Wmean += (Weffect - Wmean) / WmeanCounter;
        }
    }
}
