using MQ.Rabbit.Require;

namespace MQ.Rabbit.Brain.Require
{
    internal class NonRouteKey : IRouteKeyRequire
    {
        public string Key
        {
            get { return string.Empty; }
        }
    }
}
