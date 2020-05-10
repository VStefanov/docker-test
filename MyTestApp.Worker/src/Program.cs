using Microsoft.Extensions.Configuration;
using ServiceStack.Redis;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Worker.MyTestApp.Worker
{
    class Program
    {
        private static readonly AutoResetEvent _closingEvent = new AutoResetEvent(false);
       // private static Timer timer;
        private static RedisClient redisClient;
        private static FibCalculator fibCalculator;
        static void Main(string[] args)
        {
            var count = 0;
            var host = Environment.GetEnvironmentVariable("REDIS_HOST");
            var port = Environment.GetEnvironmentVariable("REDIS_PORT");
            redisClient = new RedisClient(host, int.Parse(port));
            fibCalculator = new FibCalculator();

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    foreach (var index in redisClient.GetNonCalculatedIndexes())
                    {
                        int fibNumber = fibCalculator.Execute(int.Parse(index));
                        redisClient.SetValueForIndex(index, fibNumber);
                    }
                    Thread.Sleep(1000);
                }
            });

            Console.CancelKeyPress += ((s, a) =>
            {
                Console.WriteLine("Bye!");
                _closingEvent.Set();
            });

            _closingEvent.WaitOne();


           
            /*
            timer = new Timer();
            timer.Interval = 2000;
            timer.Elapsed += Timer_Elapsed;
            timer.AutoReset = true;
            timer.Enabled = true;

            Console.ReadKey();
      */
        }

      
    }
}
