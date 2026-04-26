using PrintSystem.Services.Mediator;

namespace PrintSystem.Services;
public abstract class Colleague
{
    internal IMediator? Mediator { get; private set; }

    public void SetMediator(IMediator mediator)
    {
        Mediator = mediator;
    }
}