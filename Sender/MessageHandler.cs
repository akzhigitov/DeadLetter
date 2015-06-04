using System;
using Common;
using EasyNetQ;
using EasyNetQ.Topology;
using ServiceStack;

namespace Sender
{
    public class MessageHandler
    {
        private readonly AdvancedBusProvider _advancedBusProvider;

        public MessageHandler(AdvancedBusProvider advancedBusProvider)
        {
            _advancedBusProvider = advancedBusProvider;
        }

        public void Process(IntegrationMessage message)
        {
            var client = new JsonServiceClient("http://localhost:1337/");
            var response = client.Get<Status>(string.Format("/send/{0}", message.Number));
            Console.WriteLine("Response: ID {0} Success: {1}", response.Id, response.IsSuccess);

            if (!response.IsSuccess)
            {
                var advancedBus = _advancedBusProvider.GetAdvancedBus();

                var delay = TimeSpan.FromMinutes(1);
                var delayString = delay.ToString(@"dd\_hh\_mm\_ss");
                var exchangeName = "integration_test.exchange";
                var futureExchangeName = exchangeName + "_" + delayString;
                var futureQueueName = "integration_test_future";

                advancedBus.ExchangeDeclareAsync(futureExchangeName, ExchangeType.Topic)
                    .Then(futureExchange =>
                            TaskUtils.Then(advancedBus.QueueDeclareAsync(futureQueueName, perQueueMessageTtl: (int)delay.TotalMilliseconds,
                                    deadLetterExchange: exchangeName), futureQueue => advancedBus.BindAsync(futureExchange, futureQueue, "testLogin"))
                                .Then(() => advancedBus.PublishAsync(
                                    futureExchange,
                                    "testLogin",
                                    false,
                                    false,
                                    new Message<IntegrationMessage>(message))));
            }
        }
    }
}