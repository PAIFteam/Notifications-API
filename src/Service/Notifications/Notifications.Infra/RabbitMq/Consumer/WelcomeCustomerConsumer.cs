using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Core.Entities.RabbitMq
{
    public class WelcomeCustomerConsumer: IConsumer<WelcomeCustomerMessage>
    {
        public async Task Consume(ConsumeContext<WelcomeCustomerMessage> context)
        {
            var message = context.Message;
            Console.WriteLine($"Enviando notificação de boas-vindas para {message.Name} ({message.Email}) com login {message.Login}");
            await Task.CompletedTask;
        }
        public WelcomeCustomerConsumer() 
        { 
        
        }

    }
}
