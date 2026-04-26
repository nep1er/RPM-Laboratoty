namespace PrintSystem.Core.States;

public class ErrorState : IDocumentState
{
    public void Print(Document document) =>
        Console.WriteLine("[FSM: Error] Печать невозможна. Сначала сбросьте документ (Reset).");
    public void AddToQueue(Document document) =>
        Console.WriteLine("[FSM: Error] Нельзя добавить в очередь. Сначала сбросьте документ.");
    public void CompletePrinting(Document document) =>
        Console.WriteLine("[FSM: Error] Нельзя завершить — ошибка не устранена.");
    public void FailPrinting(Document document) =>
        Console.WriteLine("[FSM: Error] Документ уже в состоянии ошибки.");
    public void Reset(Document document)
    {
        document.SetState(new NewState());
        Console.WriteLine("[FSM: Error New] Документ сброшен и готов к повторной печати.");
    }
}