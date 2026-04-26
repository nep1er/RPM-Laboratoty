using PrintSystem.Services.Mediator;

namespace PrintSystem.Core.States;

public class NewState : IDocumentState
{
    public void Print(Document document)
    {
        //Документ через посредника запрашивает печать
        document.Mediator?.Notify(document, "RequestPrint", document);
    }


    public void AddToQueue(Document document)
    {
        document.Mediator?.Notify(document, "AddToQueue", document);
    }
    public void CompletePrinting(Document document) =>
        Console.WriteLine("[FSM: New] Нельзя завершить — документ ещё не печатался.");

    public void FailPrinting(Document document) =>
        Console.WriteLine("[FSM: New] Ошибка невозможна — документ ещё не в печати.");

    public void Reset(Document document) =>
        Console.WriteLine("[FSM: New] Документ уже в начальном состоянии.");
}