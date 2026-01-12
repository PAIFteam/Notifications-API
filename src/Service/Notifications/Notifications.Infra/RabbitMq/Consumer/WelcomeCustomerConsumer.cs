using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Notifications.Infra.RabbitMq.Consumer.Messages;

namespace Notifications.Infra.RabbitMq.Consumer
{
    public class WelcomeCustomerConsumer: IConsumer<WelcomeCustomerMessage>
    {
        public async Task Consume(ConsumeContext<WelcomeCustomerMessage> context)
        {
            var message = context.Message;
            // Lógica para enviar notificação de boas-vindas ao cliente
            Console.WriteLine($"Enviando notificação de boas-vindas para {message.Name} ({message.Email}) com login {message.Login}");
            // Simulação de envio de e-mail ou outra ação
            await Task.CompletedTask;
        }
        public WelcomeCustomerConsumer() 
        { 
        
        }

    }
}
