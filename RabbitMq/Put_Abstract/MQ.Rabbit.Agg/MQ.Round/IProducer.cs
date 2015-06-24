using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQ.Message;

namespace MQ.Round
{
    public interface IProducer
    {
        IMessageProperties MessageProperties { get; set; }

        IMessageSerializer MessageSerializer { get; set; }

        void Raise<TMessage>(string queueName, TMessage message) where TMessage : class;
    }
}
