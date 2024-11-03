namespace NetStone.Data.Interfaces;

public interface ICharacterEventService : ICharacterEvents, IEventService<ICharacterEventSubscriber>;