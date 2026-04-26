using PrintSystem.Services.Mediator;

namespace PrintSystem.Services;

public class Dispatcher : Colleague
{
    public void AddDocumentToQueue(Core.Document document)
    {
        document.SetMediator(Mediator);
        document.AddToQueue();
    }

    public void StartProcessing()
    {
        Mediator?.Notify(this, "ProcessQueue");
    }
}