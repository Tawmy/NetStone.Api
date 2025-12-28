using MassTransit;
using NetStone.Common.Enums;
using NetStone.Common.Exceptions;
using NetStone.Data.Interfaces;
using NetStone.Queue.Interfaces;
using NetStone.Queue.Messages;

namespace NetStone.Queue.Consumers;

public class GetCharacterConsumer(ICharacterServiceV4 characterService, IRabbitMqSenderService senderService)
    : IConsumer<GetCharacterMessage>
{
    public async Task Consume(ConsumeContext<GetCharacterMessage> context)
    {
        var m = context.Message;

        try
        {
            var character = await characterService.GetCharacterAsync(m.LodestoneId, m.MaxAge, m.CacheImages ?? false,
                m.UseFallback ?? FallbackTypeV4.None);
            await senderService.SendGetCharacterSuccessfulAsync(character);
        }
        catch (NotFoundException)
        {
            await senderService.SendGetCharacterFailedAsync(m.LodestoneId, "404");
        }
        catch (Exception e)
        {
            await senderService.SendGetCharacterFailedAsync(m.LodestoneId, e.ToString());
            throw;
        }
    }
}