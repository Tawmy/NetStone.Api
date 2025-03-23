namespace NetStone.Cache.Interfaces;

public interface IAutoMapperService
{
    TDestination Map<TDestination>(object source);
    TDestination Map<TSource, TDestination>(TSource source, TDestination destination);
}