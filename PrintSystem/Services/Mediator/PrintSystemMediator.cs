using PrintSystem.Core;
using PrintSystem.Core.States;

namespace PrintSystem.Services.Mediator;

public class PrintSystemMediator : IMediator
{
    private readonly Printer _printer;
    private readonly PrintQueue _queue;
    private readonly Logger _logger;

    public PrintSystemMediator(Printer printer, PrintQueue queue, Logger logger)
    {
        _printer = printer;
        _queue = queue;
        _logger = logger;

        _printer.SetMediator(this);
        _queue.SetMediator(this);
        _logger.SetMediator(this);
    }

    public void Notify(Colleague sender, string eventName, Document? document = null)
    {
        switch (eventName)
        {
            case "AddToQueue" when document != null:
                _queue.Enqueue(document);
                break;

            case "Enqueued" when document != null:
                _logger.Log($"Документ '{document.Title}' добавлен в очередь.");
                break;

            case "RequestPrint" when document != null:
                document.SetState(new PrintingState());
                _printer.StartPrint(document);
                break;

            case "ProcessQueue":
                ProcessQueue();
                break;

            case "PrintSuccess" when document != null:
                document.CompletePrinting();
                _logger.Log($"Успешно напечатан: '{document.Title}'");
                break;

            case "PrintFailed" when document != null:
                document.FailPrinting();
                _logger.Log($"Ошибка печати: '{document.Title}'");
                break;
        }
    }

    private void ProcessQueue()
    {
        if (_queue.IsEmpty)
        {
            _logger.Log("Очередь пуста.");
            return;
        }

        _logger.Log($"Начинаю обработку очереди. В очереди: {_queue.Count} документ(ов)");

        while (!_queue.IsEmpty)
        {
            var doc = _queue.Dequeue();
            doc.SetMediator(this);
            doc.Print();
        }

        _logger.Log("Обработка очереди завершена.");
    }
}