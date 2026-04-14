namespace EventMonitoringSystem.Core.Interfaces
{
    public interface IFormatStrategy
    {
        string Format(string message, DateTime timestamp, string metricName, double value, double threshold);
    }
}