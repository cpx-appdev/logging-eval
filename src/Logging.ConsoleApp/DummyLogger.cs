using System;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace Logging.ConsoleApp
{
    internal class DummyLogger : ILogger
    {
        public DummyLogger()
        {
        }

        public Task Info(string msg, object extraData = null)
        {
            return Task.CompletedTask;
        }

        public void SetCorrelationId(Guid id)
        {
            
        }
    }
}