namespace Common.Services.Analyzer
{
    public interface ITelemetryAnalyzer
    {
        void DetectAccelerationAnomaly(double laX, double laY, double laZ);
        void DetectWindSpikes(double windSpeed, double windAngle);
    }
}
