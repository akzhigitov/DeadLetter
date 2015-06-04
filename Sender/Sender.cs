using Common;

namespace Sender
{
    public class SenderConsumer
    {
        private readonly AdvancedBusProvider _advancedBusProvider;
        private readonly QueueFactory _queueFactory;
        private readonly MessageHandler _messageHandler;

        public SenderConsumer(
            AdvancedBusProvider advancedBusProvider, 
            QueueFactory queueFactory, 
            MessageHandler  messageHandler)
        {
            _advancedBusProvider = advancedBusProvider;
            _queueFactory = queueFactory;
            _messageHandler = messageHandler;
        }

        public void Send()
        {
            var advancedBus = _advancedBusProvider.GetAdvancedBus();
            var queue = _queueFactory.DeclareQueue("testLogin");

            advancedBus.Consume<IntegrationMessage>(
                queue,
                (message, info) => _messageHandler.Process(message.Body));
        }
    }
}