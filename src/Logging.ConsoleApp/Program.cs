using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Logging.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().Run().Wait();
        }

        async Task Run()
        {
            var sw = Stopwatch.StartNew();
            using (var logger = new DummyLogger())
            {
                for (int i = 0; i < 50000; i++)
                {
                    logger.SetCorrelationId(Guid.NewGuid());

                    var invoice = new ProcessedInvoice();
                    invoice.Id = i;
                    invoice.InvoiceDate = DateTimeOffset.Now;
                    invoice.Orders = new List<Order>
                    {
                        new Order {Id = -i, AuthorizationId = "blub"}
                    };

                    await logger.Info("Erste Meldung", invoice);
                    await logger.Info("Zweite Meldung", invoice.Orders.ToArray()[0]);
                }

                
            }
            Console.WriteLine(sw.Elapsed);
        }
    }

    public interface ILogger : IDisposable
    {
        void SetCorrelationId(Guid id);
        Task Info(string msg, object extraData = null);
    }
}
