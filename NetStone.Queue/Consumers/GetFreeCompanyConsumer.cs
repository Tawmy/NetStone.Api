using MassTransit;
using NetStone.Common.Exceptions;
using NetStone.Queue.Interfaces;
using NetStone.Queue.Messages;

namespace NetStone.Queue.Consumers;

public class GetFreeCompanyConsumer(IFreeCompanyService freeCompanyService, IRabbitMqSenderService senderService)
    : IConsumer<GetFreeCompanyMessage>
{
    public async Task Consume(ConsumeContext<GetFreeCompanyMessage> context)
    {
        var m = context.Message;

        try
        {
            var freeCompany = await freeCompanyService.GetFreeCompanyAsync(m.LodestoneId, m.MaxAge);
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