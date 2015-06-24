using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQ.Rabbit.Require;

namespace MQ.Rabbit.Brain.Require
{
    internal class PassRouteKey : IRouteKeyRequire
    {
        public PassRouteKey(string key)
        {
            this.Key = key;
        }

        public string Key { get; private set; }
    }
}
