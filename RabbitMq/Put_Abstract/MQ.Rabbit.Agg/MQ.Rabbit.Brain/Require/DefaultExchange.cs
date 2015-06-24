using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQ.Rabbit.Require;

namespace MQ.Rabbit.Brain.Require
{
    internal class DefaultExchange : IExchangeRequire
    {
        public string Name
        {
            get { return string.Empty; }
        }

        public string Type
        {
            get { return ExchangeTypeCollection.Direct; }
        }

        public bool IsDefault
        {
            get { return true; }
        }
    }
}
