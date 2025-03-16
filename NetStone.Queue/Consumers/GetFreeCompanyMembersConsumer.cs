using MassTransit;
using NetStone.Common.Exceptions;
using NetStone.Data.Interfaces;
using NetStone.Queue.Interfaces;
using NetStone.Queue.Messages;

namespace NetStone.Queue.Consumers;

public class GetFreeCompanyMembersConsumer(
    IFreeCompanyServiceV3 freeCompanyService,
    IRabbitMqSenderService senderService)
    : IConsumer<GetFreeCompanyMembersMessage>
{
    public async Task Consume(ConsumeContext<GetFreeCompanyMembersMessage> context)
    {
        var m = context.Message;

        try
        {
            var freeCompanyMembers = await freeCompanyService.GetFreeCompanyMembersAsync(m.LodestoneId, m.MaxAge,
                m.UseFallback ?? false);
            await senderService.SendGetFreeCompanyMembersSuccessfulAsync(freeCompanyMembers);
        }
        catch (NotFoundException)
        {
            await senderService.SendGetFreeCompanyMembersFailedAsync(m.LodestoneId, "404");
        }
        catch (Exception e)
        {
            await senderService.SendGetFreeCompanyMembersFailedAsync(m.LodestoneId, e.ToString());
            Console.WriteLine(e);
            throw;
        }
    }
}