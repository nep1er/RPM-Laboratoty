namespace EventMonitoringSystem.Monitoring.Models
{
    public class MetricData
    {
        public string MetricName { get; }
        public double Value { get; }
        public double Threshold { get; }
        public DateTime Timestamp { get; }

        public MetricData(string metricName, double value, double threshold, DateTime timestamp)
        {
            MetricName = metricName ?? throw new ArgumentNullException(nameof(metricName));
            Value = value;
            Threshold = threshold;
            Timestamp = timestamp;
        }

        public override string ToString()
        {
            return $"Metric: {MetricName}, Value: {Value:F2} (Threshold: {Threshold:F2}) at {Timestamp:HH:mm:ss}";
        }
    }
}