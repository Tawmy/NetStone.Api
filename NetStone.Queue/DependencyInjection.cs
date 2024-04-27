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

            x.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(rabbitMqHost, y =>
                {
                    y.Username(configuration.GetGuardedConfiguration(EnvironmentVariables.RabbitMqUsername));
                    y.Password(configuration.GetGuardedConfiguration(EnvironmentVariables.RabbitMqPassword));
                });

                configurator.ReceiveEndpoint("netstone-get-character",
                    z => z.ConfigureReceiveEndpoint<GetCharacterConsumer>(context, "netstone", "get-character"));
            });
        });

        services.AddTransient<IRabbitMqSenderService, RabbitMqSenderService>();
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