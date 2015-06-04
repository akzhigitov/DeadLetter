using Common;
using DryIoc;

namespace Sender
{
    class Program
    {
        private static void Main(string[] args)
        {
            var c = new Container();
            c.Register<AdvancedBusProvider>();
            c.Register<AdvancedBusFactory>();
            c.Register<QueueFactory>();
            c.Register<MessageHandler>();
            c.Register<SenderConsumer>();

            var sender = c.Resolve<SenderConsumer>();

            sender.Send();
        }
    }
}
