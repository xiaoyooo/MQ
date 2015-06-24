using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQ.Rabbit.Require;

namespace MQ.Rabbit.Brain.Require
{
    internal class FairQueue : IQueueRequire
    {
        public bool NeedAck
        {
            get { return true; }
        }

        public bool Fair
        {
            get { return true; }
        }
    }
}
