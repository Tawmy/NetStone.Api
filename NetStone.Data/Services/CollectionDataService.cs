using Microsoft.Extensions.Configuration;
using NetStone.Common.Extensions;

namespace NetStone.Data.Services;

public class CollectionDataService(IConfiguration configuration)
{
    private int? _totalMinions;
    private int? _totalMounts;

    public async Task<int> GetTotalMinionsAsync()
    {
        if (_totalMinions is null)
        {
            await LoadTotalsAsync();
        }

        return _totalMinions ?? 0;
    }

    public async Task<int> GetTotalMountsAsync()
    {
        if (_totalMounts is null)
        {
            await LoadTotalsAsync();
        }

        return _totalMounts ?? 0;
    }

    private Task LoadTotalsAsync()
    {
        _totalMinions = configuration.GetGuardedConfiguration<int>(EnvironmentVariables.FfxivTotalMinions);
        _totalMounts = configuration.GetGuardedConfiguration<int>(EnvironmentVariables.FfxivTotalMounts);
        return Task.CompletedTask;
    }
}