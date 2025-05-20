using MassTransit;
using NetStone.Common.Enums;
using NetStone.Common.Exceptions;
using NetStone.Data.Interfaces;
using NetStone.Queue.Interfaces;
using NetStone.Queue.Messages;

namespace NetStone.Queue.Consumers;

public class GetFreeCompanyMembersConsumer(
    IFreeCompanyService freeCompanyService,
    IRabbitMqSenderService senderService)
    : IConsumer<GetFreeCompanyMembersMessage>
{
    public async Task Consume(ConsumeContext<GetFreeCompanyMembersMessage> context)
    {
        var m = context.Message;

        try
        {
            var freeCompanyMembers = await freeCompanyService.GetFreeCompanyMembersAsync(m.LodestoneId, m.MaxAge,
                m.UseFallback ?? FallbackType.None);
            await senderService.SendGetFreeCompanyMembersSuccessfulAsync(freeCompanyMembers);
        }
        catch (NotFoundException)
        {
            await senderService.SendGetFreeCompanyMembersFailedAsync(m.LodestoneId, "404");
        }
        catch (Exception e)
        {
            await senderService.SendGetFreeCompanyMembersFailedAsync(m.LodestoneId, e.ToString());
            throw;
        }
    }
}