using MassTransit;
using NetStone.Common.Enums;
using NetStone.Common.Exceptions;
using NetStone.Data.Interfaces;
using NetStone.Queue.Interfaces;
using NetStone.Queue.Messages;

namespace NetStone.Queue.Consumers;

public class GetFreeCompanyConsumer(IFreeCompanyServiceV3 freeCompanyService, IRabbitMqSenderService senderService)
    : IConsumer<GetFreeCompanyMessage>
{
    public async Task Consume(ConsumeContext<GetFreeCompanyMessage> context)
    {
        var m = context.Message;

        try
        {
            var freeCompany = await freeCompanyService.GetFreeCompanyAsync(m.LodestoneId, m.MaxAge,
                m.UseFallback ?? FallbackType.None);
            await senderService.SendGetFreeCompanySuccessfulAsync(freeCompany);
        }
        catch (NotFoundException)
        {
            await senderService.SendGetFreeCompanyFailedAsync(m.LodestoneId, "404");
        }
        catch (Exception e)
        {
            await senderService.SendGetFreeCompanyFailedAsync(m.LodestoneId, e.ToString());
            Console.WriteLine(e);
            throw;
        }
    }
}