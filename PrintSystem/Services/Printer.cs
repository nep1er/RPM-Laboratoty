using PrintSystem.Core;
using PrintSystem.Services.Mediator;

namespace PrintSystem.Services;

public class Printer : Colleague
{
    public bool SimulateNextFailure { get; set; } = false;

    public void StartPrint(Document document)
    {
        Console.WriteLine($"[Принтер] Печатаю: '{document.Title}'...");


        if (SimulateNextFailure)
        {
            SimulateNextFailure = false;
            Mediator?.Notify(this, "PrintFailed", document);
        }
        else
        {
            Mediator?.Notify(this, "PrintSuccess", document);
        }
    }
}