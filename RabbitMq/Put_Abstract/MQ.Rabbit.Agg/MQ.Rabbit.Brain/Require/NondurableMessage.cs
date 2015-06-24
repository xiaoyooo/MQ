using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQ.Rabbit.Require;

namespace MQ.Rabbit.Brain.Require
{
    internal class NondurableMessage : IMessageRequire
    {
        public bool PersistentTransmit
        {
            get { return false; }     
        }

        public bool PersistentMq
        {
            get { return false; }
        }
    }
}
