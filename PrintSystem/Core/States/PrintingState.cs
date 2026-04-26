namespace PrintSystem.Core.States;

public class PrintingState : IDocumentState
{
    public void Print(Document document) =>
        Console.WriteLine("[FSM: Printing] Документ уже печатается.");

    public void AddToQueue(Document document) =>
        Console.WriteLine("[FSM: Printing] Нельзя добавить в очередь — документ уже в печати.");

    public void CompletePrinting(Document document)
    {
        document.SetState(new DoneState());
        Console.WriteLine("[FSM: Printing -> Done] Документ успешно напечатан.");
    }

    public void FailPrinting(Document document)
    {
        document.SetState(new ErrorState());
        Console.WriteLine("[FSM: Printing -> Error] Произошла ошибка печати.");
    }

    public void Reset(Document document) =>
        Console.WriteLine("[FSM: Printing] Сброс невозможен во время печати.");
}