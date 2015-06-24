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

namespace RabbitMq_Receiver
{
    internal class MqReceiver
    {
        private ConnectionFactory _connectionFactory = new ConnectionFactory();

        public MqReceiver()
        {
            Init();
        }

        private void Init()
        {
            _connectionFactory.HostName = ConfigurationManager.AppSettings["recv.host"];
            _connectionFactory.UserName = ConfigurationManager.AppSettings["recv.user"];
            _connectionFactory.Password = ConfigurationManager.AppSettings["recv.pwd"];
        }

        public void Receive()
        {
            using (var conn = _connectionFactory.CreateConnection())
            {
                using (var channel = conn.CreateModel())
                {
                    channel.QueueDeclare("acc_queue", true, false, false, null);
                    channel.BasicQos(0, 1, false);

                    var consumer = new QueueingBasicConsumer(channel);

                    channel.BasicConsume("acc_queue", bool.Parse(ConfigurationManager.AppSettings["recv.noack"]),
                        consumer);

                    Console.WriteLine("Waiting for a message");

                    while (true)
                    {
                        var eventargs = consumer.Queue.Dequeue();

                        var body = eventargs.Body;

                        var message = Encoding.UTF8.GetString(body);

                        var model = GetAccModel(message);
                        var oneSecond = 300;

                        if (model.SleepSeconds > 0)
                        {
                            Thread.Sleep(model.SleepSeconds*oneSecond);
                        }

                        Console.WriteLine("Receiver Queue: op([#{3}], {0},{1})={2}, in {4} seconds", model.Left,
                            model.Right,
                            model.Left + model.Right, model.No, model.SleepSeconds);

                        channel.BasicAck(eventargs.DeliveryTag, false);
                    }
                }
            }
        }

        private AccModel GetAccModel(string json)
        {
            var jss = new JavaScriptSerializer();
            return jss.Deserialize<AccModel>(json);
        }

        public void Suscribe1()
        {
            using (var conn = _connectionFactory.CreateConnection())
            {
                using (var channel = conn.CreateModel())
                {
                    channel.ExchangeDeclare("acc_exchange", "fanout");

                    var queueName = channel.QueueDeclare().QueueName;

                    channel.QueueBind(queueName, "acc_exchange", "acc_routekey1");
                        // in fanout mode exchange, the routekey is ignored.

                    Console.WriteLine("Waiting for a message");

                    var consumer = new QueueingBasicConsumer(channel);

                    channel.BasicConsume(queueName, true, consumer);

                    while (true)
                    {
                        var eventargs = consumer.Queue.Dequeue();

                        var body = eventargs.Body;

                        var message = Encoding.UTF8.GetString(body);
                        var optype = Encoding.UTF8.GetString((byte[]) eventargs.BasicProperties.Headers["optype"]);

                        var model = GetAccModel(message);
                        var oneSecond = 300;

                        if (model.SleepSeconds > 0)
                        {
                            Thread.Sleep(model.SleepSeconds*oneSecond);
                        }

                        Console.WriteLine("Exchange , Receiver Queue{5}: op([#{3}], {0},{1})={2}, in {4} seconds",
                            model.Left, model.Right,
                            model.Left + model.Right, model.No, model.SleepSeconds, queueName);

                        //channel.BasicAck(eventargs.DeliveryTag, false);
                    }
                }
            }
        }

        public void Suscribe2(string pubkey)
        {
            using (var conn = _connectionFactory.CreateConnection())
            {
                using (var channel = conn.CreateModel())
                {
                    channel.ExchangeDeclare("acc_exchange_direct", "direct");

                    var queueName = channel.QueueDeclare().QueueName;

                    channel.QueueBind(queueName, "acc_exchange_direct", pubkey);

                    Console.WriteLine("Waiting for a message of {0}", pubkey);

                    var consumer = new QueueingBasicConsumer(channel);

                    channel.BasicConsume(queueName, true, consumer);

                    while (true)
                    {
                        var eventargs = consumer.Queue.Dequeue();

                        var body = eventargs.Body;

                        var message = Encoding.UTF8.GetString(body);
                        //var optype = Encoding.UTF8.GetString((byte[])eventargs.BasicProperties.Headers["optype"]);

                        var model = GetAccModel(message);
                        var oneSecond = 300;

                        if (model.SleepSeconds > 0)
                        {
                            Thread.Sleep(model.SleepSeconds*oneSecond);
                        }

                        Console.WriteLine(
                            "Exchange , Receiver Queue {5} at route {6}: op([#{3}], {0},{1})={2}, in {4} seconds",
                            model.Left, model.Right,
                            model.Left + model.Right, model.No, model.SleepSeconds, queueName, eventargs.RoutingKey);

                        //channel.BasicAck(eventargs.DeliveryTag, false);
                    }
                }
            }
        }

        public void SuscribeTopic(string topicPattern)
        {
            using (var conn = _connectionFactory.CreateConnection())
            {
                using (var channel = conn.CreateModel())
                {
                    channel.ExchangeDeclare("acc_exchange_topic", "topic");

                    var queueName = channel.QueueDeclare().QueueName;

                    channel.QueueBind(queueName, "acc_exchange_topic", topicPattern);

                    Console.WriteLine("Waiting for a message of topic {0}", topicPattern);

                    var consumer = new QueueingBasicConsumer(channel);

                    channel.BasicConsume(queueName, true, consumer);

                    while (true)
                    {
                        var eventargs = consumer.Queue.Dequeue();

                        var body = eventargs.Body;

                        var message = Encoding.UTF8.GetString(body);
                        //var optype = Encoding.UTF8.GetString((byte[])eventargs.BasicProperties.Headers["optype"]);

                        var model = GetAccModel(message);
                        var oneSecond = 300;

                        if (model.SleepSeconds > 0)
                        {
                            Thread.Sleep(model.SleepSeconds*oneSecond);
                        }

                        Console.WriteLine(
                            "Exchange , Receiver Queue {5} at route {6}: op([#{3}], {0},{1})={2}, in {4} seconds",
                            model.Left, model.Right,
                            model.Left + model.Right, model.No, model.SleepSeconds, queueName, eventargs.RoutingKey);

                        //channel.BasicAck(eventargs.DeliveryTag, false);
                    }
                }
            }
        }

        public void RpcService()
        {
            using (var conn = _connectionFactory.CreateConnection())
            {
                using (var channel = conn.CreateModel())
                {
                    channel.QueueDeclare("acc_rpc", false, false, false, null);
                    channel.BasicQos(0, 1, false);
                    var consumer = new QueueingBasicConsumer(channel);
                    channel.BasicConsume("acc_rpc", false, consumer);
                    
                    while (true)
                    {
                        var eventargs = consumer.Queue.Dequeue();
                        var body = Encoding.UTF8.GetString(eventargs.Body);
                        var props = eventargs.BasicProperties;
                        var replyProps = channel.CreateBasicProperties();
                        replyProps.CorrelationId = props.CorrelationId;
                        var model = GetAccModel(body);

                        var result = model.Left + model.Right;
                        Console.WriteLine("Receiver Queue: op([#{3}], {0},{1})={2}, in {4} seconds", model.Left,
                            model.Right, model.Left + model.Right, model.No, model.SleepSeconds);
                        var replyBody = Encoding.UTF8.GetBytes(result.ToString());

                        channel.BasicPublish("", props.ReplyTo, replyProps, replyBody);

                        Console.WriteLine("Reply Queue {0} - {1}: {2}", props.ReplyTo,
                            props.CorrelationId, result.ToString());

                        channel.BasicAck(eventargs.DeliveryTag, false);
                    }
                }
            }
        }
    }
}
