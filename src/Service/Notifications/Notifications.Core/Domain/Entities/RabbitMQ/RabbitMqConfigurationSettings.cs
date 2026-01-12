using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Core.Domain.Entities.RabbitMQ
{
    public class RabbitMqConfigurationSettings
    {
        public const string OPTION_KEY = "RabbitMqSettings";

        public string HostName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public List<int> RedeliveryInSeconds { get; set; }
        public List<int> RetryInSeconds { get; set; }
        public string QueueName { get; set; }
        public string SchedulerQueueName { get; set; }
        public bool StartConsumer { get; set; } = false;

         public Uri GetQueueAdress() => new Uri($"amqp://{Username}:{Password}@{HostName}:5672/{QueueName}");
    }
}
