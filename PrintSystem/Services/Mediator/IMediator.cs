using PrintSystem.Core;

namespace PrintSystem.Services.Mediator;
public interface IMediator
{
    void Notify(Colleague sender, string eventName, Document? document = null);
}