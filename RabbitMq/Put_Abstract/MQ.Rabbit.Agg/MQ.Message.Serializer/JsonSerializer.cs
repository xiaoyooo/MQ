using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQ.Message;
using Newtonsoft.Json;

namespace MQ.Message.Serializer
{
    public class JsonSerializer : IMessageSerializer
    {
        public static readonly IMessageSerializer Instance = new JsonSerializer();

        public byte[] Serializer<TMessage>(TMessage message, string encoding) where TMessage : class
        {
            var json = JsonConvert.SerializeObject(message);

            return TrivalStringSerializer.Instance.Serializer(json, encoding);
        }

        public TMessage Deserializer<TMessage>(byte[] message, string encoding) where TMessage : class
        {
            var json = TrivalStringSerializer.Instance.Deserializer<string>(message, encoding);

            return JsonConvert.DeserializeObject<TMessage>(json);
        }
    }
}
