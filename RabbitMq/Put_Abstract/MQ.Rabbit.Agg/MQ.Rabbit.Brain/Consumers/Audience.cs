using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQ.Rabbit.Brain.Require;

namespace MQ.Rabbit.Brain.Consumers
{
    internal class Audience : RabbitConsumer
    {
        public Audience(string exchange)
            : base(new FastAssignQueue(), new TopicExchange(exchange), new NondurableMessage())
        {

        }
    }
}
