using PrintSystem.Core;
using PrintSystem.Services.Mediator;

namespace PrintSystem.Services;

public class PrintQueue : Colleague
{
    private readonly Queue<Document> _queue = new();

    public bool IsEmpty => _queue.Count == 0;
    public int Count => _queue.Count;

    public void Enqueue(Document document)
    {
        _queue.Enqueue(document);
        Mediator?.Notify(this, "Enqueued", document);
    }

    public Document Dequeue()
    {
        if (_queue.Count == 0)
            throw new InvalidOperationException("Очередь пуста");

        return _queue.Dequeue();
    }
}