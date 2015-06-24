using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQ.Rabbit.Brain.Require;
using RabbitMQ.Client;

namespace MQ.Rabbit.Brain.Producers
{
    internal class RpcClient : RabbitDualProducer
    {
        public RpcClient()
            : base(new FairQueue(), new DefaultExchange(), new PersistentMessage())
        {

        }
    }
}
