using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQ.Round
{
    public interface IDualProducer : IProducer
    {
        event Action<object> OnReply;

        void Raise<TMessage, TReplyMessage>(string queueName, TMessage message) 
            where TMessage : class
            where TReplyMessage: class;
    }
}
