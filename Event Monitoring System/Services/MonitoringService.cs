using EventMonitoringSystem.Core.Base;
using EventMonitoringSystem.Monitoring;
using EventMonitoringSystem.Monitoring.Models;

namespace EventMonitoringSystem.Services
{
    public class MonitoringService
    {
        private readonly EventMonitor _monitor;
        private readonly List<EventHandlerBase> _handlers = new();

        public MonitoringService()
        {
            _monitor = new EventMonitor();
            _monitor.MetricExceeded += OnMetricExceeded;
        }

        public void RegisterHandler(EventHandlerBase handler)
        {
            _handlers.Add(handler);
            Console.WriteLine($"[Service] Зарегистрированный обработчик: {handler.HandlerName}");
        }

        private void OnMetricExceeded(object? sender, MetricEventArgs e)
        {
            foreach (var handler in _handlers)
            {
                handler.ProcessEvent(e);
            }
        }

        public void RunSimulation()
        {
            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("Старт Monitoring System Simulation");
            Console.WriteLine(new string('=', 60) + "\n");

            _monitor.SimulateMetrics();

            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("Симуляция завершена");
            Console.WriteLine(new string('=', 60) + "\n");
        }
    }
}