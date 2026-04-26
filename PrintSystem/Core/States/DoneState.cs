namespace PrintSystem.Core.States;

public class DoneState : IDocumentState
{
    public void Print(Document document) =>
        Console.WriteLine("[FSM: Done] Документ уже напечатан. Печать не требуется.");

    public void AddToQueue(Document document) =>
        Console.WriteLine("[FSM: Done] Нельзя добавить в очередь — документ уже завершён.");

    public void CompletePrinting(Document document) =>
        Console.WriteLine("[FSM: Done] Документ уже в финальном состоянии.");

    public void FailPrinting(Document document) =>
        Console.WriteLine("[FSM: Done] Ошибка невозможна — документ уже напечатан.");

    public void Reset(Document document) =>
        Console.WriteLine("[FSM: Done] Сброс завершённого документа не предусмотрен.");
}