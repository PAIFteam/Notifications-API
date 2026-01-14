
using Notifications.Core.Domain.Entities.RabbitMQ;
using Notifications.Infra.RabbitMq.Consumer;
using GreenPipes;
using MassTransit;
using MassTransit.MultiBus;
using Microsoft.Extensions.DependencyInjection;


namespace Notifications.API.Extensions
{
    public static class RabbitConsumerConfiguration
    {
            public static IServiceCollection AddConsumer(this IServiceCollection services, IConfiguration configuration)
            {
                var rabbitSettings = new RabbitMqConfigurationSettings();

                configuration
                    .GetSection(RabbitMqConfigurationSettings.OPTION_KEY)
                    .Bind(rabbitSettings);
                services.AddScoped(_ => rabbitSettings);

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


                _.UsingRabbitMq((context, configure) =>
                {
                    var rabbitUri = new Uri($"rabbitmq://{rabbitSettings.Username}:{rabbitSettings.Password}@{rabbitSettings.HostName}:5672"); ///{rabbitSettings.QueueName}

                    configure.Host(rabbitUri, h =>
                    {
                    });


                    configure.ReceiveEndpoint(rabbitSettings.QueueName, e =>
                    {
                        e.Consumer<WelcomeCustomerConsumer>(context);
                    });
                });


                //_.AddBus(context => Bus.Factory.CreateUsingRabbitMq(configure =>
                //{
                //    var rabbitUri = new Uri($"rabbitmq://{rabbitSettings.Username}:{rabbitSettings.Password}@{rabbitSettings.HostName}:5672"); ///{rabbitSettings.QueueName}

                //    configure.Host(rabbitUri, h =>
                //    {   
                //    });

                //    configure.UseCircuitBreaker(CorrelatedBy =>
                //    {
                //        CorrelatedBy.TrackingPeriod = TimeSpan.FromMinutes(1);
                //        CorrelatedBy.TripThreshold = 15;
                //        CorrelatedBy.ActiveThreshold = 10;
                //        CorrelatedBy.ResetInterval = TimeSpan.FromMinutes(5);
                //    });

                //    configure.UseInMemoryScheduler(rabbitSettings.ScheduleQueueName);

                //    configure.ReceiveEndpoint(rabbitSettings.QueueName, configureEndpoint =>
                //    {
                //        //var redeliveryIntervals = GetIntervals(rabbitSettings.RedeliveryInSeconds);
                //        //var retryIntervals = GetIntervals(rabbitSettings.RetryInSeconds);

                //        //if(!Equals(redeliveryIntervals, null) && redeliveryIntervals.Any())
                //        //{
                //        //    configureEndpoint.UseScheduledRedelivery(r => r.Intervals(redeliveryIntervals));
                //        //}

                //        //if (!Equals(redeliveryIntervals, null) && retryIntervals.Any())
                //        //{
                //        //    configureEndpoint.UseMessageRetry(r => r.Intervals(retryIntervals));
                            
                //        //}

                //        configureEndpoint.Consumer<WelcomeCustomerConsumer>(context);
                //    });

                //    //configure.useHealthCheck(context);
                //}));
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
