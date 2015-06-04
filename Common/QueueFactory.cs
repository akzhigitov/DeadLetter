using EasyNetQ.Topology;

namespace Common
{
    public class QueueFactory
    {
        private readonly AdvancedBusProvider _advancedBusProvider;

        public QueueFactory(AdvancedBusProvider advancedBusProvider)
        {
            _advancedBusProvider = advancedBusProvider;
        }

        public IQueue DeclareQueue(IExchange exchange, string userLogin)
        {
            var advancedBus = _advancedBusProvider.GetAdvancedBus();

            var queue = advancedBus.QueueDeclare("integration_test_" + userLogin, deadLetterExchange: "integration_test_dead_letter.exchange", deadLetterRoutingKey: userLogin);

            advancedBus.Bind(exchange, queue, userLogin);

            return queue;
        }

        public IQueue DeclareQueue(string userLogin)
        {
            var advancedBus = _advancedBusProvider.GetAdvancedBus();

            var queue = advancedBus.QueueDeclare("integration_test_" + userLogin, deadLetterExchange: "integration_test_dead_letter.exchange", deadLetterRoutingKey: userLogin);

            return queue;
        }
    }
}