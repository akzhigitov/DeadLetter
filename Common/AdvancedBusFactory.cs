using EasyNetQ;
using EasyNetQ.Loggers;

namespace Common
{
    public class AdvancedBusFactory
    {
        public IAdvancedBus CreateAdvancedRabbitBus()
        {
            var rabbitBus = RabbitHutch.CreateBus(
                new ConnectionConfiguration
                {
                    Hosts = new[]
                    {
                        new HostConfiguration
                        {
                            Host = "localhost"
                        }
                    },
                    UserName = "user",
                    Password = "password"
                },
                register => { register.Register<IEasyNetQLogger>(provider => new ConsoleLogger()); });

            var advancedRabbitBus = rabbitBus.Advanced;
            return advancedRabbitBus;
        }
    }
}