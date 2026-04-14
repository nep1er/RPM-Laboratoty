using EventMonitoringSystem.Formatting;
using EventMonitoringSystem.Notifications.Handlers;
using EventMonitoringSystem.Services;

namespace EventMonitoringSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // 1. Создаём сервис мониторинга
            var monitoringService = new MonitoringService();

            // 2. Регистрируем обработчики с разными стратегиями форматирования
            //(демонстрация Strategy + Template Method)

            // Обработчик 1: Консоль + текстовый формат
            monitoringService.RegisterHandler(
                new ConsoleHandler(new TextFormatStrategy())
            );

            //Обработчик 2: Файл + JSON формат
            monitoringService.RegisterHandler(
                new FileHandler("alerts.log", new JsonFormatStrategy())
            );

            // Обработчик 3: Email + HTML формат
            monitoringService.RegisterHandler(
                new EmailHandler("admin@example.com", new HtmlFormatStrategy())
            );

            // 3. Запускаем симуляцию событий
            monitoringService.RunSimulation();

            // 4.содержимое лог
            if (File.Exists("alerts.log"))
            {
                Console.WriteLine("\nСодержание alerts.log:");
                Console.WriteLine(new string('-', 50));
                Console.WriteLine(File.ReadAllText("alerts.log"));
            }

        }
    }
}