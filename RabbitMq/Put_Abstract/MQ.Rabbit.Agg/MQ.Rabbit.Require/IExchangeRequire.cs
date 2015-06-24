using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQ.Rabbit.Require
{
    public interface IExchangeRequire
    {
        string Name { get; }

        string Type { get; }

        bool IsDefault { get; }
    }
}
