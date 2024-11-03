namespace NetStone.Data.Interfaces;

public interface IEventService<in T> where T : class
{
    void Subscribe(T subscriber);
    void Unsubscribe(T subscriber);
}