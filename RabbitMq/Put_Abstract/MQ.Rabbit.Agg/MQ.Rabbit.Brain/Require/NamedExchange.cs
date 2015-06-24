#define CONTRACTS_FULL
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MQ.Rabbit.Require;

namespace MQ.Rabbit.Brain.Require
{
    internal abstract class NamedExchange : IExchangeRequire
    {
        protected NamedExchange(string name, string type)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));

            this.Name = name;

            this.Type = type;
        }

        public string Name { get; private set; }

        public string Type { get; private set; }

        public bool IsDefault { get { return false; } }
    }

    internal class DirectExchange : NamedExchange
    {
        public DirectExchange(string name)
            : base(name, ExchangeTypeCollection.Direct)
        {
            
        }
    }

    internal class FanoutExchange : NamedExchange
    {
        public FanoutExchange(string name)
            : base(name, ExchangeTypeCollection.Fanout)
        {

        }
    }

    internal class HeadersExchange : NamedExchange
    {
        public HeadersExchange(string name)
            : base(name, ExchangeTypeCollection.Headers)
        {
            
        }
    }

    internal class TopicExchange : NamedExchange
    {
        public TopicExchange(string name)
            : base(name, ExchangeTypeCollection.Fanout)
        {
            
        }
    }
}
