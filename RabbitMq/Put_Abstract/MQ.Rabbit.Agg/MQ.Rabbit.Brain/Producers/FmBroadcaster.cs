using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQ.Rabbit.Brain.Require;
using RabbitMQ.Client;

namespace MQ.Rabbit.Brain.Producers
{
    internal class FmBroadcaster : RabbitProducer
    {
        public FmBroadcaster(string exchange)
            : base(new FastAssignQueue(), new DirectExchange(exchange), new NondurableMessage())
        {
            
        }
    }
}
