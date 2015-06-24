using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQ.Rabbit.Brain.Require;

namespace MQ.Rabbit.Brain.Consumers
{
    internal class FmListener : RabbitConsumer
    {
        public FmListener(string exchange)
            : base(new FastAssignQueue(), new DirectExchange(exchange), new NondurableMessage())
        {
            
        }
    }
}
