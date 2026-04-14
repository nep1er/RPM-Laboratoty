using EventMonitoringSystem.Core.Interfaces;

namespace EventMonitoringSystem.Formatting
{
    public class HtmlFormatStrategy : IFormatStrategy
    {
        public string Format(string message, DateTime timestamp, string metricName, double value, double threshold)
        {
            string severityClass = value > threshold ? "critical" : "normal";

            return $@"
            <!DOCTYPE html>
            <html>
            <head><style>
              .alert {{ padding: 12px; border-left: 4px solid #ff6b6b; background: #fff5f5; font-family: sans-serif; }}
              .critical {{ border-color: #e74c3c; background: #ffeaea; }}
              .metric {{ font-weight: bold; color: #2c3e50; }}
              .timestamp {{ color: #7f8c8d; font-size: 0.9em; }}
            </style></head>
            <body>
              <div class=""alert {severityClass}"">
                <span class=""timestamp"">[{timestamp:yyyy-MM-dd HH:mm:ss}]</span><br>
                <span class=""metric"">{message}</span>
              </div>
            </body>
            </html>".Trim();
        }
    }
}