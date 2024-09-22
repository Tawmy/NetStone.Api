using MassTransit;
using NetStone.Common.Exceptions;
using NetStone.Data.Interfaces;
using NetStone.Queue.Interfaces;
using NetStone.Queue.Messages;

namespace NetStone.Queue.Consumers;

public class GetCharacterAchievementsConsumer(ICharacterService characterService, IRabbitMqSenderService senderService)
    : IConsumer<GetCharacterAchievementsMessage>
{
    public async Task Consume(ConsumeContext<GetCharacterAchievementsMessage> context)
    {
        var m = context.Message;

        try
        {
            var achievements = await characterService.GetCharacterAchievementsAsync(m.LodestoneId, m.MaxAge);
            await senderService.SendGetCharacterAchievementsSuccessfulAsync(achievements);
        }
        catch (NotFoundException)
        {
            await senderService.SendGetCharacterAchievementsFailedAsync(m.LodestoneId, "404");
        }
        catch (Exception e)
        {
            await senderService.SendGetCharacterAchievementsFailedAsync(m.LodestoneId, e.ToString());
            Console.WriteLine(e);
            throw;
        }
    }
}