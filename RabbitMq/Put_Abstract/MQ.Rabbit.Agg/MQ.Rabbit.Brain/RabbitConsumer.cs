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
    internal abstract class RabbitConsumer : IConsumer
    {
        private readonly IQueueRequire _queueRequire;
        private readonly IExchangeRequire _exchangeRequire;
        private readonly IMessageRequire _messageRequire;
        protected readonly ConnectionFactory _connectionFactory = new ConnectionFactory();

        public event Action<object> OnCatch;

        protected RabbitConsumer(IQueueRequire queueRequire, IExchangeRequire exchangeRequire,
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

        public virtual void Catch<TMessage>(string queueName)
            where TMessage : class
        {
            Catch<TMessage>(queueName, null);
        }

        protected void Catch<TMessage>(string queueName, Action<object, IModel, IBasicProperties> afterConsume)
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
                        var realQueueName = BindQueue(channel, queueName);

                        var consumer = new QueueingBasicConsumer(channel);

                        channel.BasicConsume(realQueueName, !_queueRequire.NeedAck, consumer);

                        while (WaitNext())
                        {
                            try
                            {
                                var eventargs = consumer.Queue.Dequeue();

                                var message = MessageSerializer.Deserializer<TMessage>(eventargs.Body, MessageProperties.ContentEncoding);

                                OnCatch(message);

                                if (afterConsume != null)
                                {
                                    afterConsume(message, channel, eventargs.BasicProperties);
                                }

                                if (_queueRequire.NeedAck)
                                {
                                    channel.BasicAck(eventargs.DeliveryTag, false);
                                }
                            }
                            catch (Exception)
                            {
                                
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                
            }
        }

        private bool PreCheck()
        {
            return (MessageProperties != null && MessageSerializer != null);
        }

        protected string BindQueue(IModel channel, string queueName)
        {
            if (_exchangeRequire.IsDefault)
            {
                channel.QueueDeclare(queueName, _messageRequire.PersistentMq, false, false, null);

                if (_queueRequire.Fair)
                {
                    channel.BasicQos(0, 1, false);
                }
            }
            else
            {
                channel.ExchangeDeclare(_exchangeRequire.Name, _exchangeRequire.Type);

                var tempQueueName = channel.QueueDeclare().QueueName;

                channel.QueueBind(tempQueueName, _exchangeRequire.Name, queueName);

                queueName = tempQueueName;
            }

            return queueName;
        }

        protected virtual bool WaitNext()
        {
            return true;
        }
    }
}
