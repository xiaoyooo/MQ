using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQ.Message;

namespace MQ.Message.Serializer
{
    public class ProtoBufSerializer : IMessageSerializer
    {
        public byte[] Serializer<TMessage>(TMessage message, string encoding)
            where TMessage : class
        {
            return Encoding.GetEncoding(encoding).GetBytes(message as string);
        }

        public TMessage Deserializer<TMessage>(byte[] message, string encoding)
            where TMessage : class
        {
            return Encoding.GetEncoding(encoding).GetString(message) as TMessage;
        }
    }
}
