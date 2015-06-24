using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQ.Rabbit.Require;

namespace MQ.Rabbit.Brain.Require
{
    internal class FastAssignQueue : IQueueRequire
    {
        public bool NeedAck
        {
            get { return false; }
        }

        public bool Fair
        {
            get { return false; }
        }
    }
}
