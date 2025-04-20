using MassTransit;
using NetStone.Common.Enums;
using NetStone.Common.Exceptions;
using NetStone.Data.Interfaces;
using NetStone.Queue.Interfaces;
using NetStone.Queue.Messages;

namespace NetStone.Queue.Consumers;

public class GetCharacterMountsConsumer(ICharacterServiceV3 characterService, IRabbitMqSenderService senderService)
    : IConsumer<GetCharacterMountsMessage>
{
    public async Task Consume(ConsumeContext<GetCharacterMountsMessage> context)
    {
        var m = context.Message;

        try
        {
            var mounts = await characterService.GetCharacterMountsAsync(m.LodestoneId, m.MaxAge,
                m.UseFallback ?? FallbackType.None);
            await senderService.SendGetCharacterMountsSuccessfulAsync(mounts);
        }
        catch (NotFoundException)
        {
            await senderService.SendGetCharacterMountsFailedAsync(m.LodestoneId, "404");
        }
        catch (Exception e)
        {
            await senderService.SendGetCharacterMountsFailedAsync(m.LodestoneId, e.ToString());
            throw;
        }
    }
}