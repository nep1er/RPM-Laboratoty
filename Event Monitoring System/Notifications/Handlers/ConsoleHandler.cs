using EventMonitoringSystem.Core.Base;
using EventMonitoringSystem.Core.Interfaces;

namespace EventMonitoringSystem.Notifications.Handlers
{
    public class ConsoleHandler : EventHandlerBase
    {
        public ConsoleHandler(IFormatStrategy formatStrategy)
            : base("ConsoleHandler", formatStrategy)
        {
        }

        protected override void SendMessage(string message)
        {
            Console.WriteLine($"[{HandlerName}]");
            Console.WriteLine(message);
            Console.WriteLine(new string('-', 50));
        }

        protected override void LogResult(string message)
        {
            Console.WriteLine($"[Log] Доставлено в консоль в {DateTime.Now:HH:mm:ss.fff}");
        }
    }
}