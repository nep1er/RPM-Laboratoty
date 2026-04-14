using EventMonitoringSystem.Core.Interfaces;

namespace EventMonitoringSystem.Formatting
{
    public class TextFormatStrategy : IFormatStrategy
    {
        public string Format(string message, DateTime timestamp, string metricName, double value, double threshold)
        {
            return $"[{timestamp:yyyy-MM-dd HH:mm:ss}] {message}";
        }
    }
}