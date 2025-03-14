using System.Diagnostics;
using AutoMapper;
using NetStone.Cache.Interfaces;

namespace NetStone.Cache.Services;

internal class AutoMapperService(IMapper mapper) : IAutoMapperService
{
    private static readonly ActivitySource ActivitySource = new(nameof(IAutoMapperService));
    
    public TDestination Map<TDestination>(object source)
    {
        using var activity = ActivitySource.StartActivity();
        activity?.AddTag(nameof(TDestination), typeof(TDestination));
        return mapper.Map<TDestination>(source);
    }

    public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
    {
        using var activity = ActivitySource.StartActivity();
        activity?.AddTag(nameof(TSource), typeof(TSource));
        activity?.AddTag(nameof(TDestination), typeof(TDestination));
        return mapper.Map(source, destination);
    }
}