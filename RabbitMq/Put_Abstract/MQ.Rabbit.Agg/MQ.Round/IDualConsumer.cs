using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQ.Round
{
    public interface IDualConsumer : IConsumer
    {
        event Func<object, object> OnReply;

        void Catch<TMessage, TReplyMessage>(string queue) 
            where TMessage : class
            where TReplyMessage : class;
    }
}
