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
    internal abstract class RabbitDualConsumer : RabbitConsumer, IDualConsumer
    {
        public event Func<object, object> OnReply;

        protected RabbitDualConsumer(IQueueRequire queueRequire, IExchangeRequire exchangeRequire,
                IMessageRequire messageRequire)
            : base(queueRequire, exchangeRequire,messageRequire)
        {
            
        }

        public override void Catch<TMessage>(string queueName)
        {
            throw new InvalidOperationException("do not support this operation");
        }

        public void Catch<TMessage, TReplyMessage>(string queue)
            where TMessage : class
            where TReplyMessage : class
        {
            base.Catch<TMessage>(queue, AfterConsume<TReplyMessage>);
        }

        protected void AfterConsume<TReplyMessage>(object message, IModel channel, IBasicProperties properties)
            where TReplyMessage : class
        {
            var replyProps = channel.CreateBasicProperties();
            replyProps.CorrelationId = properties.CorrelationId;

            object reply = OnReply(message);

            var replyBody = MessageSerializer.Serializer(reply as TReplyMessage, MessageProperties.ContentEncoding);

            channel.BasicPublish(string.Empty, properties.ReplyTo, replyProps, replyBody);
        }
    }
}
