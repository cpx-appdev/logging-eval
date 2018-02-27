using System;
using System.Threading.Tasks;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Sinks.Elasticsearch;

namespace ConsoleApp7
{
    internal class ElasticLogger : ILogger
    {
        private Logger _logger;
        private Guid _correlationId;

        public ElasticLogger()
        {
            _logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
                {
                    AutoRegisterTemplate = true
                })
                .CreateLogger();

            _logger.Dispose();
        }

        public void SetCorrelationId(Guid id)
        {
            _correlationId = id;
            
        }

        public Task Info(string msg, object extraData = null)
        {
            using (LogContext.PushProperty("correlationId", _correlationId))
            {
                _logger.Information(msg, extraData);
            }
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _logger?.Dispose();
        }
    }
}