#define CONTRACTS_FULL
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQ.Message;
using MQ.Rabbit.Configuration;
using MQ.Rabbit.Require;
using MQ.Round;
using RabbitMQ.Client;

namespace MQ.Rabbit.Brain
{
    internal abstract class RabbitProducer : IProducer
    {
        protected readonly IQueueRequire _queueRequire;
        protected readonly IExchangeRequire _exchangeRequire;
        protected readonly IMessageRequire _messageRequire;
        protected readonly ConnectionFactory _connectionFactory = new ConnectionFactory();
        
        protected RabbitProducer(IQueueRequire queueRequire, IExchangeRequire exchangeRequire,
            IMessageRequire messageRequire)
        {
            Contract.Requires(queueRequire != null);
            Contract.Requires(exchangeRequire != null);
            Contract.Requires(messageRequire != null);

            _queueRequire = queueRequire;
            _exchangeRequire = exchangeRequire;
            _messageRequire = messageRequire;

            Init();
        }

        private void Init()
        {
            var rabbitConfig = ConfigurationManager.GetSection("rabbitmq") as RabbitMqConfiguration;

            if (rabbitConfig == null)
                throw new InvalidDataException("config rabbitmq do not config well");

            _connectionFactory.Uri = rabbitConfig.Server.Host;
            _connectionFactory.UserName = rabbitConfig.Server.User;
            _connectionFactory.Password = rabbitConfig.Server.Password;
        }

        public IMessageProperties MessageProperties { get; set; }

        public IMessageSerializer MessageSerializer { get; set; }

        public virtual void Raise<TMessage>(string queueName, TMessage message)
            where TMessage : class
        {
            Raise(queueName, message, MessageSerializer);
        }

        protected void Raise<TMessage>(string queueName, TMessage message, IMessageSerializer serializer,
            Action<object, IBasicProperties, IMessageSerializer> afterPublish = null)
            where TMessage : class
        {
            if (!PreCheck())
            {
                return;
            }

            try
            {
                using (var conn = _connectionFactory.CreateConnection())
                {
                    using (var channel = conn.CreateModel())
                    {
                        BindQueue(channel, queueName);

                        var state = BeforePublish(channel);

                        var body = serializer.Serializer(message, MessageProperties.ContentEncoding);

                        var basicProperties = CreateBasicProperties(channel, state);

                        channel.BasicPublish(_exchangeRequire.Name, queueName, basicProperties, body);

                        if (afterPublish != null)
                        {
                            afterPublish(state, basicProperties, serializer);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        private bool PreCheck()
        {
            return (MessageProperties != null && MessageSerializer != null);
        }

        protected void BindQueue(IModel channel, string queueName)
        {
            if (_exchangeRequire.IsDefault)
            {
                channel.QueueDeclare(queueName, _messageRequire.PersistentMq, false, false, null);
            }
            else
            {
                channel.ExchangeDeclare(_exchangeRequire.Name, _exchangeRequire.Type);
            }
        }

        protected virtual object BeforePublish(IModel channel)
        {
            return null;
        }

        protected virtual IBasicProperties CreateBasicProperties(IModel channel, dynamic state)
        {
            if (_messageRequire.PersistentTransmit)
            {
                var props = channel.CreateBasicProperties();
                props.DeliveryMode = 2;

                return props;
            }

            return null;
        }
    }
}
