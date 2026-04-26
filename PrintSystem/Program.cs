using PrintSystem.Core;
using PrintSystem.Core.States;
using PrintSystem.Services;
using PrintSystem.Services.Mediator;

namespace PrintSystem;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Print System Demo - State + Mediator Patterns\n");
        Console.WriteLine(new string('=', 60));

        var printer = new Printer();
        var queue = new PrintQueue();
        var logger = new Logger(consoleOutput: true);
        var mediator = new PrintSystemMediator(printer, queue, logger);
        var dispatcher = new Dispatcher();
        dispatcher.SetMediator(mediator);


        Console.WriteLine("\nСЦЕНАРИЙ 1: Успешная печать");
        Console.WriteLine(new string('-', 40));

        var doc1 = new Document("Отчёт_2024.pdf");
        var doc2 = new Document("Диаграмма_проекта.png");

        dispatcher.AddDocumentToQueue(doc1);
        dispatcher.AddDocumentToQueue(doc2);
        dispatcher.StartProcessing();


        Console.WriteLine("\n\nСЦЕНАРИЙ 2: Ошибка принтера и восстановление");
        Console.WriteLine(new string('-', 40));

        var doc3 = new Document("Важный_документ.docx");
        dispatcher.AddDocumentToQueue(doc3);

        //Имитируем сбой принтера
        printer.SimulateNextFailure = true;
        dispatcher.StartProcessing();

        Console.WriteLine("\nВосстанавливаем документ после ошибки:");
        doc3.Reset();
        dispatcher.AddDocumentToQueue(doc3);
        dispatcher.StartProcessing();


        Console.WriteLine("\n\nСЦЕНАРИЙ 3: Проверка состояний");
        Console.WriteLine(new string('-', 40));

        var doc4 = new Document("Тестовый_файл.txt");

        Console.WriteLine("\nПопытка печати документа в состоянии New (должно запросить печать):");
        doc4.Print();

        Console.WriteLine("\nПопытка добавить уже напечатанный документ в очередь:");
        doc4.SetState(new PrintingState()); 
        doc4.CompletePrinting();
        doc4.AddToQueue(); //должно отказать

        Console.WriteLine("\nПопытка сбросить завершённый документ:");
        doc4.Reset(); //должно отказать

        Console.WriteLine("\n" + new string('=', 60));
        Console.WriteLine("Демонстрация завершена!");
        Console.WriteLine($"Всего событий в логе: {logger.GetLogs().Count}");

    }
}