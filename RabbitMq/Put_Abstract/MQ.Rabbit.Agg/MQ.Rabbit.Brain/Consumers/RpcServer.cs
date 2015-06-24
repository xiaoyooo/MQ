using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQ.Rabbit.Brain.Require;

namespace MQ.Rabbit.Brain.Consumers
{
    internal class RpcServer : RabbitDualConsumer
    {
        public RpcServer()
            : base(new FairQueue(), new DefaultExchange(), new PersistentMessage())
        {

        }
    }
}
