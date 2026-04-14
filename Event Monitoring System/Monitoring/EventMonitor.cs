using EventMonitoringSystem.Monitoring.Models;

namespace EventMonitoringSystem.Monitoring
{
    public class EventMonitor
    {
        public event EventHandler<MetricEventArgs>? MetricExceeded;

        public void CheckMetric(string metricName, double value, double threshold)
        {
            Console.WriteLine($"[Monitor] Checking {metricName}: {value:F2} vs threshold {threshold:F2}");

            if (value > threshold)
            {
                var metricData = new MetricData(metricName, value, threshold, DateTime.Now);
                var eventArgs = new MetricEventArgs($"{metricName}_Exceeded", metricData);

                MetricExceeded?.Invoke(this, eventArgs);

                Console.WriteLine($"[Alert] Событие, инициированное для {metricName}!\n");
            }
            else
            {
                Console.WriteLine($"[OK] {metricName} в пределах допустимогоs\n");
            }
        }

        public void SimulateMetrics()
        {
            CheckMetric("CPU_Usage", 85.5, 80.0);      // Превышение
            CheckMetric("Memory_Usage", 65.2, 90.0);   // В норме
            CheckMetric("Network_Traffic", 120.8, 100.0); // Превышение
            CheckMetric("Disk_Usage", 45.0, 85.0);     // В норме
        }
    }
}