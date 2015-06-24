using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQ.Message
{
    public interface IMessageSerializer
    {
        byte[] Serializer<TMessage>(TMessage message, string encoding)
            where TMessage : class;

        TMessage Deserializer<TMessage>(byte[] message, string encoding)
            where TMessage : class;
    }
}
