using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQ.Rabbit.Brain.Require;

namespace MQ.Rabbit.Brain.Consumers
{
    internal class FfListener : RabbitConsumer
    {
        public FfListener(string exchange)
            : base(new FastAssignQueue(), new FanoutExchange(exchange), new NondurableMessage())
        {

        }
    }
}
