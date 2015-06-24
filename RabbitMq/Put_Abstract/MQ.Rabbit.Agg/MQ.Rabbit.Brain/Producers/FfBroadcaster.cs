﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQ.Rabbit.Brain.Require;
using RabbitMQ.Client;

namespace MQ.Rabbit.Brain.Producers
{
    internal class FfBroadcaster : RabbitProducer
    {
        public FfBroadcaster(string exchange)
            : base(new FastAssignQueue(), new FanoutExchange(exchange), new NondurableMessage())
        {

        }
    }
}
