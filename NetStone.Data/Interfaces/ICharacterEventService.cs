namespace NetStone.Data.Interfaces;

public interface ICharacterEventService : ICharacterEvents
{
    void Subscribe(ICharacterEventSubscriber subscriber);
    void Unsubscribe(ICharacterEventSubscriber subscriber);
}