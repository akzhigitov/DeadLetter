using System;
using Common;
using Funq;
using ServiceStack;

namespace Router
{
    class Program
    {
        private static bool _isSuccess;

        [Route("/send/{Id}")]
        public class Message
        {
            public int Id { get; set; }
        }
        
        public class HelloService : Service
        {
            public object Any(Message message)
            {
                Console.WriteLine("ID {0} Success {1}", message.Id, _isSuccess);

                return new Status
                {
                    IsSuccess = _isSuccess,
                    Id = message.Id
                };
            }
        }

        public class AppHost : AppSelfHostBase
        {
            public AppHost()
                : base("HttpListener Self-Host", typeof(HelloService).Assembly) { }

            public override void Configure(Container container) { }
        }

        static void Main(string[] args)
        {
            var listeningOn = args.Length == 0 ? "http://*:1337/" : args[0];
            var appHost = new AppHost()
                .Init();

            appHost.Start(listeningOn);

            while (Console.ReadKey(true).Key == ConsoleKey.Escape)
            {
                _isSuccess = !_isSuccess;

                Console.WriteLine("Success: {0}", _isSuccess);
            }
        }
    }
}
