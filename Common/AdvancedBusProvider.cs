using EasyNetQ;

namespace Common
{
    public class AdvancedBusProvider
    {
        private readonly AdvancedBusFactory _advancedBusFactory;
        private IAdvancedBus _advancedRabbitBus;

        public AdvancedBusProvider(AdvancedBusFactory advancedBusFactory)
        {
            _advancedBusFactory = advancedBusFactory;
        }

        public IAdvancedBus GetAdvancedBus()
        {
            if (_advancedRabbitBus == null)
            {
                _advancedRabbitBus = _advancedBusFactory.CreateAdvancedRabbitBus();
            }

            return _advancedRabbitBus;
        }
    }
}