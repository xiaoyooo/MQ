using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQ.Rabbit.Brain.Consumers;
using MQ.Round;

namespace MQ.Rabbit.Brain
{
    public class RabbitConsumerFactory
    {
        public IConsumer CreateOnewayEnd()
        {
            return new OnewayEnd();
        }

        public IConsumer CreateWorkDequeue()
        {
            return new WorkDeQueue();
        }

        public IConsumer CreateFfListener(string exchange)
        {
            return new FfListener(exchange);
        }

        public IConsumer CreateFmListener(string exchange)
        {
            return new FmListener(exchange);
        }

        public IConsumer CreateTopicSelecter(string exchange)
        {
            return new Audience(exchange);
        }

        public IDualConsumer CreateRpcServer()
        {
            return new RpcServer();
        }
    }
}
