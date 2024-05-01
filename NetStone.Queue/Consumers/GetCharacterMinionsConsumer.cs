using MassTransit;
using NetStone.Common.Exceptions;
using NetStone.Data.Interfaces;
using NetStone.Queue.Interfaces;
using NetStone.Queue.Messages;

namespace NetStone.Queue.Consumers;

public class GetCharacterMinionsConsumer(ICharacterService characterService, IRabbitMqSenderService senderService)
    : IConsumer<GetCharacterMinionsMessage>
{
    public async Task Consume(ConsumeContext<GetCharacterMinionsMessage> context)
    {
        var m = context.Message;

        try
        {
            var minions = await characterService.GetCharacterMinionsAsync(m.LodestoneId, m.MaxAge);
            await senderService.SendGetCharacterMinionsSuccessfulAsync(minions);
        }
        catch (NotFoundException)
        {
            await senderService.SendGetCharacterMinionsFailedAsync(m.LodestoneId, "404");
        }
        catch (Exception e)
        {
            await senderService.SendGetCharacterMinionsFailedAsync(m.LodestoneId, e.ToString());
            Console.WriteLine(e);
            throw;
        }
    }
}