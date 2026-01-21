using MassTransit;
using Microsoft.Extensions.Logging;
using Payments.Core.Domain.Entities.RabbitMQ;

namespace Notifications.Core.Entities.RabbitMq
{
    public class PaymentProcessedEventConsumer: IConsumer<PaymentProcessedMessage>
    {
        
        private readonly ILogger<PaymentProcessedEventConsumer> _logger;

        public PaymentProcessedEventConsumer(
            ILogger<PaymentProcessedEventConsumer> logger
            )
        {
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<PaymentProcessedMessage> context)
        {
            Console.WriteLine($"SIMULAR E-mail com o resultado do processamento do Pagamento do e-mail, IdUser {context.Message.IdUser} " +
                $" IdGame ({context.Message.IdGame}) e Price {context.Message.Price.ToString()}" +
                $" Aprovado ({context.Message.Aproved}) e Mensagem {context.Message.Message}");
            
            await Task.CompletedTask;
        }
    }
}
