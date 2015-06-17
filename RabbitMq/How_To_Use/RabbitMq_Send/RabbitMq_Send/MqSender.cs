using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using RabbitMQ.Client;
using TransModel;

namespace RabbitMq_Send
{
    class MqSender
    {
        ConnectionFactory _connectionFactory = new ConnectionFactory();

        public MqSender()
        {
            Init();
        }

        private void Init()
        {
            _connectionFactory.HostName = ConfigurationManager.AppSettings["sender.host"];
            _connectionFactory.UserName = ConfigurationManager.AppSettings["sender.user"];
            _connectionFactory.Password = ConfigurationManager.AppSettings["sender.pwd"];

            //_connectionFactory.RequestedHeartbeat = 60;
        }

        public void Send()
        {
            using (var conn = _connectionFactory.CreateConnection())
            {
                using (var channel = conn.CreateModel())
                {
                    channel.QueueDeclare("acc_queue", true, false, false, null);

                    for (int i = 1; i <= 10; i++)
                    {
                        string message = MakeModel(1, 2, i%5, i);
                        var body = Encoding.UTF8.GetBytes(message);

                        var props = channel.CreateBasicProperties();
                        props.DeliveryMode = 2;

                        channel.BasicPublish("", "acc_queue", props, body);

                        Console.WriteLine("Send Queue: {0} + {1}", 1.ToString(), 2.ToString());
                    }
                }
            }
        }

        private string MakeModel(int left, int right, int seconds, int no)
        {
            var jss = new JavaScriptSerializer();
            
            return jss.Serialize(new AccModel() {Left = left, Right = right, SleepSeconds = seconds, No = no.ToString()});
        }

        public void Publish1()
        {
            using (var conn = _connectionFactory.CreateConnection())
            {
                using (var channel = conn.CreateModel())
                {
                    channel.ExchangeDeclare("acc_exchange", "fanout");

                    for (int i = 1; i <= 10; i++)
                    {
                        string message = MakeModel(1, 2, i % 5, i);
                        var body = Encoding.UTF8.GetBytes(message);

                        var props = channel.CreateBasicProperties();
                        props.DeliveryMode = 2;
                        props.Type = "acc";
                        props.Headers = new Dictionary<string, object>();
                        props.Headers.Add("optype","acc");

                        channel.BasicPublish("acc_exchange", "acc_routekey", props, body); // in fanout mode exchange, the routekey is ignored.

                        Console.WriteLine("Send Exchange: {0} + {1}", 1.ToString(), 2.ToString());
                    }
                }
            }
        }

        public void Publish2(string pubkey)
        {
            using (var conn = _connectionFactory.CreateConnection())
            {
                using (var channel = conn.CreateModel())
                {
                    channel.ExchangeDeclare("acc_exchange_direct", "direct");

                    for (int i = 1; i <= 1; i++)
                    {
                        string message = MakeModel(1, 2, i % 5, i);
                        var body = Encoding.UTF8.GetBytes(message);

                        var props = channel.CreateBasicProperties();
                        props.DeliveryMode = 2;
                        props.Type = "acc";
                        props.Headers = new Dictionary<string, object>();
                        props.Headers.Add("optype", "acc");

                        channel.BasicPublish("acc_exchange_direct", pubkey, null, body); // in fanout mode exchange, the routekey is ignored.

                        Console.WriteLine("Send Exchange in route {2}: {0} + {1}", 1.ToString(), 2.ToString(), pubkey);
                    }
                }
            }
        }

        public void PublishTopic(string topic)
        {
            using (var conn = _connectionFactory.CreateConnection())
            {
                using (var channel = conn.CreateModel())
                {
                    channel.ExchangeDeclare("acc_exchange_topic", "topic");

                   for (int i = 1; i <= 1; i++)
                    {
                        string message = MakeModel(1, 2, i % 5, i);
                        var body = Encoding.UTF8.GetBytes(message);

                        var props = channel.CreateBasicProperties();
                        props.DeliveryMode = 2;
                        props.Type = "acc";
                        props.Headers = new Dictionary<string, object>();
                        props.Headers.Add("optype", "acc");

                        channel.BasicPublish("acc_exchange_topic", topic, null, body); // in fanout mode exchange, the routekey is ignored.

                        Console.WriteLine("Send Exchange in topic {2}: {0} + {1}", 1.ToString(), 2.ToString(), topic);
                    }
                }
            }
        }

		public void RpcCall()
		{
			using (var conn = _connectionFactory.CreateConnection())
            {
                using (var channel = conn.CreateModel())
                {
					channel.QueueDeclare("acc_rpc", false,false,false,null);
                    var replyQueueName = channel.QueueDeclare().QueueName;
                    var consumer = new QueueingBasicConsumer(channel);
					channel.BasicConsume(replyQueueName,true,consumer);

					string message = MakeModel(1,2,1,1);

					var body = Encoding.UTF8.GetBytes(message);
					var props = channel.CreateBasicProperties();
					props.ReplyTo = replyQueueName;
					var correlationId = Guid.NewGuid().ToString();
				    props.CorrelationId = correlationId;	

					channel.BasicPublish("","acc_rpc",props,body);

					Console.WriteLine("Send in rpc reply {0} - {1}: {2} + {3}", replyQueueName, correlationId, 1.ToString(), 2.ToString());

                    Thread.Sleep(3000);

					while(true)
					{
						var replyEventArgs = consumer.Queue.Dequeue();
						if (replyEventArgs.BasicProperties.CorrelationId == correlationId)
						{
							Console.WriteLine("Get Reply : {0}", Encoding.UTF8.GetString(replyEventArgs.Body));
							break;
						}
					}
				}
			}
		}
    }
}
