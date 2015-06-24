using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQ.Round
{
    public interface IConsumer
    {
        event Action<object> OnCatch;

        void Catch<TMessage>(string queue) where TMessage : class;
    }
}
