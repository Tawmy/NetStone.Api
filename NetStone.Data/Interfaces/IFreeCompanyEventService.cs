namespace NetStone.Data.Interfaces;

public interface IFreeCompanyEventService : IFreeCompanyEvents, IEventService<IFreeCompanyEventSubscriber>;