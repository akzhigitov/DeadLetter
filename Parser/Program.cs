using System;
using Common;
using DryIoc;
using EasyNetQ;
using EasyNetQ.Topology;

namespace Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            var c = new Container();
            c.Register<AdvancedBusProvider>();
            c.Register<AdvancedBusFactory>();
            c.Register<QueueFactory>();

            var advancedBusProvider = c.Resolve<AdvancedBusProvider>();

            var advancedRabbitBus = advancedBusProvider.GetAdvancedBus();
            CreateAlternateQueue(advancedRabbitBus);

            string exchangeName = "integration_test.exchange";

            var exchange = advancedRabbitBus.ExchangeDeclare(exchangeName, ExchangeType.Topic,
                alternateExchange: "integration_test_alt.exchange");

            var queueFactory = c.Resolve<QueueFactory>();
            queueFactory.DeclareQueue(exchange, "testLogin");

            for (int i = 0; i < 1000; i++)
            {
                advancedRabbitBus.PublishAsync(
                    exchange,
                    "testLogin",
                    false,
                    false,
                    new Message<IntegrationMessage>(
                        new IntegrationMessage(i)))
                    .ContinueWith(
                        task =>
                        {
                            if (task.IsCompleted)
                            {
                                Console.WriteLine("Completed");
                            }
                            if (task.IsFaulted)
                            {
                                Console.WriteLine(task.Exception);
                            }
                        });
            }
        }

        private static void CreateAlternateQueue(IAdvancedBus advancedRabbitBus)
        {
            var exchangeAlt = advancedRabbitBus.ExchangeDeclare("integration_test_alt.exchange", ExchangeType.Topic);
            var queueAlt = advancedRabbitBus.QueueDeclare("integration_test_alt");
            advancedRabbitBus.Bind(exchangeAlt, queueAlt, "*");
        }
    }
}
