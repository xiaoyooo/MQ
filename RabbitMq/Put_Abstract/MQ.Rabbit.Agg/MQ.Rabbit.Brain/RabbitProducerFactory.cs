using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQ.Rabbit.Brain.Producers;
using MQ.Round;

namespace MQ.Rabbit.Brain
{
    public class RabbitProducerFactory
    {
        public IProducer CreateOnewayStart()
        {
            return new OnewayStart();
        }

        public IProducer CreateWorkEnqueue()
        {
            return new WorkEnQueue();
        }

        public IProducer CreateFfBroadcaster(string exchange)
        {
            return new FfBroadcaster(exchange);
        }

        public IProducer CreateFmBroadcaster(string exchange)
        {
            return new FmBroadcaster(exchange);
        }

        public IProducer CreateTopicReporter(string exchange)
        {
            return new Reporter(exchange);
        }

        public IDualProducer CreateRpcClient()
        {
            return new RpcClient();
        }
    }
}
