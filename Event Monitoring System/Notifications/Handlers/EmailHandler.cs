using EventMonitoringSystem.Core.Base;
using EventMonitoringSystem.Core.Interfaces;

namespace EventMonitoringSystem.Notifications.Handlers
{
    public class EmailHandler : EventHandlerBase
    {
        private readonly string _recipientEmail;

        public EmailHandler(string recipientEmail, IFormatStrategy formatStrategy)
            : base("EmailHandler", formatStrategy)
        {
            _recipientEmail = recipientEmail ?? throw new ArgumentNullException(nameof(recipientEmail));
        }

        protected override void SendMessage(string message)
        {
            Console.WriteLine($"[{HandlerName}] Отправка электронной почты на {_recipientEmail}:");
            Console.WriteLine($"Тема: Предупреждение: Метрическое число");
            Console.WriteLine($"Предварительный просмотр: {message.Substring(0, Math.Min(60, message.Length))}...");
        }

        protected override void LogResult(string message)
        {
            Console.WriteLine($"[Log] Электронное письмо поставлено в очередь на доставку по адресу {DateTime.Now:HH:mm:ss}");
        }
    }
}