using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQ.Message;
using MQ.Rabbit.Require;
using MQ.Round;
using RabbitMQ.Client;

namespace MQ.Rabbit.Brain
{
    internal abstract class RabbitDualProducer : RabbitProducer, IDualProducer
    {
        class Reply
        {
            public QueueingBasicConsumer Consumer { get; set; }

            public string ReplyQueueName { get; set; }
        }

        public event Action<object> OnReply;

        protected RabbitDualProducer(IQueueRequire queueRequire, IExchangeRequire exchangeRequire,
            IMessageRequire messageRequire)
            :base(queueRequire,exchangeRequire,messageRequire)
        {
            
        }

        public override void Raise<TMessage>(string queueName, TMessage message)
        {
            throw new InvalidOperationException("do not support this operation");
        }

        public void Raise<TMessage, TReplyMessage>(string queueName, TMessage message)
            where TMessage : class
            where TReplyMessage : class
        {
            base.Raise(queueName, message, MessageSerializer, AfterPublish<TReplyMessage>);
        }

        protected override object BeforePublish(IModel channel)
        {
            var replyQueueName = channel.QueueDeclare().QueueName;

            var consumer = new QueueingBasicConsumer(channel);

            channel.BasicConsume(replyQueueName, true, consumer);

            return new Reply {Consumer = consumer, ReplyQueueName = replyQueueName};
        }

        protected void AfterPublish<TReplyMessage>(object state, IBasicProperties properties, IMessageSerializer serializer)
            where TReplyMessage : class
        {
            var reply = state as Reply;

            if (reply != null && reply.Consumer != null && properties != null)
            {
                while (true)
                {
                    var replyEventArgs = reply.Consumer.Queue.Dequeue();
                    if (replyEventArgs.BasicProperties.CorrelationId == properties.CorrelationId)
                    {
                        var message = serializer.Deserializer<TReplyMessage>(replyEventArgs.Body,
                            MessageProperties.ContentEncoding);

                        OnReply(message);

                        break;
                    }
                }
            }
        }

        protected override IBasicProperties CreateBasicProperties(IModel channel, dynamic state)
        {
            var props = channel.CreateBasicProperties();
            props.ReplyTo = state.ReplyQueueName;
            var correlationId = Guid.NewGuid().ToString();
            props.CorrelationId = correlationId;

            if (_messageRequire.PersistentTransmit)
            {
                props.DeliveryMode = 2;
            }

            return props;
        }
    }
}
