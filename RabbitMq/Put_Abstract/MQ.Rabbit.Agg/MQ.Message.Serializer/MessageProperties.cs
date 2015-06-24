using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQ.Message.Serializer
{
    public class MessageProperties : IMessageProperties
    {
        public string ContentEncoding { get; set; }

        public string ContentType { get; set; }

        public static readonly IMessageProperties Utf8JsonMessage = new MessageProperties()
        {
            ContentEncoding = Encoding.UTF8.EncodingName,
            ContentType = "application/json"
        };
    }
}
