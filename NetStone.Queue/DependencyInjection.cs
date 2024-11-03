using System.Net.Mime;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetStone.Common.Extensions;
using NetStone.Queue.Consumers;
using NetStone.Queue.Interfaces;
using NetStone.Queue.Services;
using RabbitMQ.Client;

namespace NetStone.Queue;

public static class DependencyInjection
{
    public static void AddQueueServices(this IServiceCollection services, IConfiguration configuration)
    {
        if (Environment.GetEnvironmentVariable(EnvironmentVariables.RabbitMqHost) is not { } rabbitMqHost)
        {
            return;
        }

        services.AddMassTransit(x =>
        {
            x.AddConsumer<GetCharacterConsumer>();
            x.AddConsumer<GetCharacterClassJobsConsumer>();
            x.AddConsumer<GetCharacterMinionsConsumer>();
            x.AddConsumer<GetCharacterMountsConsumer>();
            x.AddConsumer<GetFreeCompanyConsumer>();
            x.AddConsumer<GetFreeCompanyMembersConsumer>();
            x.AddConsumer<GetCharacterAchievementsConsumer>();

            x.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(rabbitMqHost, y =>
                {
                    y.Username(configuration.GetGuardedConfiguration(EnvironmentVariables.RabbitMqUsername));
                    y.Password(configuration.GetGuardedConfiguration(EnvironmentVariables.RabbitMqPassword));
                });

                configurator.ReceiveEndpoint("netstone-get-character",
                    z => z.ConfigureReceiveEndpoint<GetCharacterConsumer>(context, "netstone", "get-character"));
                configurator.ReceiveEndpoint("netstone-get-character-class-jobs",
                    z => z.ConfigureReceiveEndpoint<GetCharacterClassJobsConsumer>(context, "netstone",
                        "get-character-class-jobs"));
                configurator.ReceiveEndpoint("netstone-get-character-minions",
                    z => z.ConfigureReceiveEndpoint<GetCharacterMinionsConsumer>(context, "netstone",
                        "get-character-minions"));
                configurator.ReceiveEndpoint("netstone-get-character-mounts",
                    z => z.ConfigureReceiveEndpoint<GetCharacterMountsConsumer>(context, "netstone",
                        "get-character-mounts"));
                configurator.ReceiveEndpoint("netstone-get-character-achievements",
                    z => z.ConfigureReceiveEndpoint<GetCharacterAchievementsConsumer>(context, "netstone",
                        "get-character-achievements"));
                configurator.ReceiveEndpoint("netstone-get-free-company",
                    z => z.ConfigureReceiveEndpoint<GetFreeCompanyConsumer>(context, "netstone",
                        "get-free-company"));
                configurator.ReceiveEndpoint("netstone-get-free-company-members",
                    z => z.ConfigureReceiveEndpoint<GetFreeCompanyMembersConsumer>(context, "netstone",
                        "get-free-company-members"));
            });
        });

        services.AddTransient<IRabbitMqSenderService, RabbitMqSenderService>();
        services.AddHostedService<CharacterEventSubscriber>();
        services.AddHostedService<FreeCompanyEventSubscriber>();
    }

    private static void ConfigureReceiveEndpoint<TConsumer>(this IRabbitMqReceiveEndpointConfigurator configurator,
        IRegistrationContext context, string exchangeName, string routingKey)
        where TConsumer : class, IConsumer
    {
        configurator.ConfigureConsumeTopology = false;
        configurator.ConfigureConsumer<TConsumer>(context);

        configurator.DefaultContentType = new ContentType("application/json");
        configurator.UseRawJsonDeserializer();
        configurator.UseRawJsonSerializer();

        configurator.Bind(exchangeName, bindConfigurator =>
        {
            bindConfigurator.ExchangeType = ExchangeType.Direct;
            bindConfigurator.RoutingKey = routingKey;
        });
    }
}