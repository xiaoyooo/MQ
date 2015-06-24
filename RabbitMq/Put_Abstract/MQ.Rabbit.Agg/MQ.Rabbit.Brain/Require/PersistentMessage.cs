using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQ.Rabbit.Require;

namespace MQ.Rabbit.Brain.Require
{
    internal class PersistentMessage : IMessageRequire
    {
        public bool PersistentTransmit
        {
            get { return true; }
        }

        public bool PersistentMq
        {
            get { return true; }
        }
    }
}
