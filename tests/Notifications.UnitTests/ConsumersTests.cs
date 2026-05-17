using FluentAssertions;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using Notifications.Core.Entities.RabbitMq;
using Payments.Core.Domain.Entities.RabbitMQ;
using Users.Core.Entities.RabbitMq;

namespace Notifications.UnitTests;

public class ConsumersTests
{
    [Fact]
    public async Task WelcomeCustomerConsumer_Consume_MensagemValida_NaoDeveFalhar()
    {
        var context = new Mock<ConsumeContext<WelcomeCustomerMessage>>();
        context.SetupGet(x => x.Message).Returns(new WelcomeCustomerMessage("Arthur", "arthur", "arthur@test.com"));
        var sut = new WelcomeCustomerConsumer();
        var writer = new StringWriter();
        var original = Console.Out;
        Console.SetOut(writer);

        try
        {
            var act = async () => await sut.Consume(context.Object);

            await act.Should().NotThrowAsync();
            writer.ToString().Should().Contain("Arthur").And.Contain("arthur@test.com");
        }
        finally
        {
            Console.SetOut(original);
        }
    }

    [Fact]
    public async Task PaymentProcessedEventConsumer_Consume_MensagemValida_NaoDeveFalhar()
    {
        var context = new Mock<ConsumeContext<PaymentProcessedMessage>>();
        context.SetupGet(x => x.Message).Returns(new PaymentProcessedMessage(1, 2, 10m, true, "ok"));
        var sut = new PaymentProcessedEventConsumer(Mock.Of<ILogger<PaymentProcessedEventConsumer>>());
        var writer = new StringWriter();
        var original = Console.Out;
        Console.SetOut(writer);

        try
        {
            var act = async () => await sut.Consume(context.Object);

            await act.Should().NotThrowAsync();
            writer.ToString().Should().Contain("Aprovado (True)").And.Contain("Mensagem ok");
        }
        finally
        {
            Console.SetOut(original);
        }
    }

    [Fact]
    public async Task PaymentProcessedEventConsumer_Consume_PagamentoRecusado_DeveEscreverResultadoRecusado()
    {
        var context = new Mock<ConsumeContext<PaymentProcessedMessage>>();
        context.SetupGet(x => x.Message).Returns(new PaymentProcessedMessage(1, 2, 500m, false, "recusado"));
        var sut = new PaymentProcessedEventConsumer(Mock.Of<ILogger<PaymentProcessedEventConsumer>>());
        var writer = new StringWriter();
        var original = Console.Out;
        Console.SetOut(writer);

        try
        {
            await sut.Consume(context.Object);

            writer.ToString().Should().Contain("Aprovado (False)").And.Contain("Mensagem recusado");
        }
        finally
        {
            Console.SetOut(original);
        }
    }
}
