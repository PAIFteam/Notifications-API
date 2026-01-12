
using Notifications.Core.Domain.Entities.RabbitMQ;
using GreenPipes;
using MassTransit;
using MassTransit.MultiBus;
using Microsoft.Extensions.DependencyInjection;


namespace Notifications.Infra.RabbitMq.Consumer
{
    public static class RabbitConsumerConfiguration
    {
            public static IServiceCollection AddConsumer(this IServiceCollection services,RabbitMqConfigurationSettings rabbitSettings)
            {
                if (rabbitSettings.StartConsumer)
                {
                    CreateBus<IBus>(services, rabbitSettings);
                    services.AddMassTransitHostedService();
                }

                return services;
            }
            
        private static void CreateBus<T>(IServiceCollection services, RabbitMqConfigurationSettings rabbitSettings) where T : class, IBus
        {
            services.AddMassTransit<IBus>(_ =>
            {
                _.AddConsumer<WelcomeCustomerConsumer>();

                _.AddBus(context => Bus.Factory.CreateUsingRabbitMq(configure =>
                {
                    var rabbitUri = new Uri($"ampq://{rabbitSettings.Username}:{rabbitSettings.Password}@{rabbitSettings.HostName}:5672");

                    configure.Host(rabbitUri, h =>
                    {   
                    });

                    configure.UseCircuitBreaker(CorrelatedBy =>
                    {
                        CorrelatedBy.TrackingPeriod = TimeSpan.FromMinutes(1);
                        CorrelatedBy.TripThreshold = 15;
                        CorrelatedBy.ActiveThreshold = 10;
                        CorrelatedBy.ResetInterval = TimeSpan.FromMinutes(5);
                    });

                    configure.UseInMemoryScheduler(rabbitSettings.SchedulerQueueName);

                    configure.ReceiveEndpoint(rabbitSettings.QueueName, configureEndpoint =>
                    {
                        var redeliveryIntervals = GetIntervals(rabbitSettings.RedeliveryInSeconds);
                        var retryIntervals = GetIntervals(rabbitSettings.RetryInSeconds);

                        if(!Equals(redeliveryIntervals, null) && redeliveryIntervals.Any())
                        {
                            configureEndpoint.UseScheduledRedelivery(r => r.Intervals(redeliveryIntervals));
                        }

                        if (!Equals(redeliveryIntervals, null) && retryIntervals.Any())
                        {
                            configureEndpoint.UseMessageRetry(r => r.Intervals(retryIntervals));
                            
                        }

                        configureEndpoint.ConfigureConsumer<WelcomeCustomerConsumer>(context);
                    });

                    //configure.useHealthCheck(context);
                }));
            });
        }
        private static TimeSpan[] GetIntervals(List<int> intervals)
        {
            if (Equals(intervals,null))
            {
                return new TimeSpan[0];
            }

            var nonZeroIntervals = intervals.Where(interval =>!Equals(interval,0));

            return nonZeroIntervals.Select(interval => TimeSpan.FromSeconds(interval)).ToArray();
        }
    }
}
