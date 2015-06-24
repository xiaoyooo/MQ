using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQ.Rabbit.Brain.Require
{
    internal static class ExchangeTypeCollection
    {
        public static readonly string Fanout = "fanout";

        public static readonly string Direct = "direct";

        public static readonly string Headers = "headers";

        public static readonly string Topic = "topic";
    }
}
