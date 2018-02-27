using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp7
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

            using (var logger = new ElasticLogger())
            {
                for (int i = 0; i < 5000; i++)
                {
                    logger.SetCorrelationId(Guid.NewGuid());

                    var invoice = new ProcessedInvoice();
                    invoice.Id = i;
                    invoice.InvoiceDate = DateTimeOffset.Now;
                    invoice.Orders = new List<Order>
                    {
                        new Order {Id = -i, AuthorizationId = "blub"},
                        new Order {Id = -i*2, AuthorizationId = "blub2"}
                    };

                    await logger.Info("{@invoice} erstellt", invoice);
                    await logger.Info("Enthält eine {@order}", invoice.Orders.ToArray()[0]);
                }
                Console.WriteLine(sw.Elapsed);
            }
            
        }
    }

    public interface ILogger : IDisposable
    {
        void SetCorrelationId(Guid id);
        Task Info(string msg, object extraData = null);
    }
}