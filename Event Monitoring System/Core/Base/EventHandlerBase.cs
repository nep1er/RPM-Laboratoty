using EventMonitoringSystem.Monitoring.Models;
using EventMonitoringSystem.Core.Interfaces;

namespace EventMonitoringSystem.Core.Base
{
    public abstract class EventHandlerBase
    {
        protected IFormatStrategy _formatStrategy;
        public string HandlerName { get; }

        protected EventHandlerBase(string handlerName, IFormatStrategy formatStrategy)
        {
            HandlerName = handlerName ?? throw new ArgumentNullException(nameof(handlerName));
            _formatStrategy = formatStrategy ?? throw new ArgumentNullException(nameof(formatStrategy));
        }

        public void ProcessEvent(MetricEventArgs e)
        {
            //Форматирование
            var formattedMessage = FormatMessage(e.EventType, e.Data);

            //Отправка уведомления
            SendMessage(formattedMessage);

            //Логирование результата
            LogResult(formattedMessage);
        }

        protected string FormatMessage(string eventType, MetricData data)
        {
            return _formatStrategy.Format(
                message: $"[{eventType}] {data.MetricName}: {data.Value} (threshold: {data.Threshold})",
                timestamp: data.Timestamp,
                metricName: data.MetricName,
                value: data.Value,
                threshold: data.Threshold
            );
        }

        protected abstract void SendMessage(string message);

        protected virtual void LogResult(string message){ }

        public void SetFormatStrategy(IFormatStrategy strategy)
        {
            _formatStrategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
        }
    }
}