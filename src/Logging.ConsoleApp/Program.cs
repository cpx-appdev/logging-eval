using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Serilog.Context;
using Serilog.Core;

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
            using (var logger = new SeriLogger())
            {
                for (var i = 0; i < 50000; i++)
                {
                    logger.SetCorrelationId(Guid.NewGuid());

                    var invoice = new ProcessedInvoice
                    {
                        Id = i,
                        InvoiceDate = DateTimeOffset.Now,
                        Orders = new List<Order>
                        {
                            new Order {Id = -i, AuthorizationId = "blub"}
                        }
                    };

                    await logger.Info("Erste Meldung {@invoice}", invoice);
                    await logger.Info("Zweite Meldung {@order}", invoice.Orders.ToArray()[0]);
                }
                
            }
            Console.WriteLine(sw.Elapsed);
            Console.ReadKey();
        }
    }

    public interface ILogger : IDisposable
    {
        void SetCorrelationId(Guid id);
        Task Info(string msg, object extraData = null);
    }

    public class SeriLogger : ILogger
    {
        private readonly Logger _logger;
        private Guid _id;

        public SeriLogger()
        {
            _logger = new LoggerConfiguration()
                .WriteTo.Seq("http://localhost:5341")
                .Enrich.FromLogContext()
                .CreateLogger();
        }

        public void Dispose()
        {
            _logger.Dispose();
        }

        public void SetCorrelationId(Guid id)
        {
            _id = id;
        }

        public Task Info(string msg, object extraData = null)
        {
            using (LogContext.PushProperty("correlationId", _id))
            {
                _logger.Information(msg, extraData);
                return Task.CompletedTask;
            }
        }
    }
}
