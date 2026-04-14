using System.Text.Json;
using EventMonitoringSystem.Core.Interfaces;

namespace EventMonitoringSystem.Formatting
{
    public class JsonFormatStrategy : IFormatStrategy
    {
        public string Format(string message, DateTime timestamp, string metricName, double value, double threshold)
        {
            var payload = new
            {
                timestamp = timestamp.ToString("O"),
                level = "WARNING",
                metric = new
                {
                    name = metricName,
                    value,
                    threshold,
                    exceeded = value > threshold
                },
                rawMessage = message
            };

            return JsonSerializer.Serialize(payload, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
        }
    }
}