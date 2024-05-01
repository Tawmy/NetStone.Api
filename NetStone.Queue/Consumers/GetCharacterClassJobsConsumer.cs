using MassTransit;
using NetStone.Common.Exceptions;
using NetStone.Data.Interfaces;
using NetStone.Queue.Interfaces;
using NetStone.Queue.Messages;

namespace NetStone.Queue.Consumers;

public class GetCharacterClassJobsConsumer(ICharacterService characterService, IRabbitMqSenderService senderService)
    : IConsumer<GetCharacterClassJobsMessage>
{
    public async Task Consume(ConsumeContext<GetCharacterClassJobsMessage> context)
    {
        var m = context.Message;

        try
        {
            var classJobs = await characterService.GetCharacterClassJobsAsync(m.LodestoneId, m.MaxAge);
            await senderService.SendGetCharacterClassJobsSuccessfulAsync(classJobs);
        }
        catch (NotFoundException)
        {
            await senderService.SendGetCharacterClassJobsFailedAsync(m.LodestoneId, "404");
        }
        catch (Exception e)
        {
            await senderService.SendGetCharacterClassJobsFailedAsync(m.LodestoneId, e.ToString());
            Console.WriteLine(e);
            throw;
        }
    }
}