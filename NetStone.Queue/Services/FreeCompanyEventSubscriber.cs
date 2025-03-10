using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetStone.Common.DTOs.FreeCompany;
using NetStone.Data.Interfaces;
using NetStone.Queue.Interfaces;

namespace NetStone.Queue.Services;

public class FreeCompanyEventSubscriber(
    IServiceProvider serviceProvider,
    IFreeCompanyEventService freeCompanyEventService) : IFreeCompanyEventSubscriber, IHostedService
{
    public Task FreeCompanyRefreshedAsync(FreeCompanyDto freeCompanyDto)
    {
        using var scope = serviceProvider.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IRabbitMqSenderService>();
        return service.SendGetFreeCompanyRefreshedAsync(freeCompanyDto);
    }

    public Task FreeCompanyMembersRefreshedAsync(FreeCompanyMembersOuterDto freeCompanyMemberDto)
    {
        using var scope = serviceProvider.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IRabbitMqSenderService>();
        return service.SendGetFreeCompanyMembersRefreshedAsync(freeCompanyMemberDto);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        freeCompanyEventService.Subscribe(this);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        freeCompanyEventService.Unsubscribe(this);
        return Task.CompletedTask;
    }
}