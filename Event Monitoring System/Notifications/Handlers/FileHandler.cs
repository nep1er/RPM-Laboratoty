using EventMonitoringSystem.Core.Base;
using EventMonitoringSystem.Core.Interfaces;

namespace EventMonitoringSystem.Notifications.Handlers
{
    public class FileHandler : EventHandlerBase
    {
        private readonly string _filePath;
        private readonly object _lockObj = new();

        public FileHandler(string filePath, IFormatStrategy formatStrategy)
            : base("FileHandler", formatStrategy)
        {
            _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        }

        protected override void SendMessage(string message)
        {
            lock (_lockObj)
            {
                try
                {
                    File.AppendAllText(_filePath, message + Environment.NewLine + new string('-', 50) + Environment.NewLine);
                    Console.WriteLine($"[{HandlerName}] Написанный для {_filePath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[{HandlerName}] Ошибка при записи файла: {ex.Message}");
                }
            }
        }

        protected override void LogResult(string message)
        {
            Console.WriteLine($"[Log] Файловая запись, созданная на {DateTime.Now:HH:mm:ss}");
        }
    }
}