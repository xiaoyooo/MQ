using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMq_Send
{
    class Program
    {
        static void Main(string[] args)
        {
            //new MqSender().Send();

            //new MqSender().Publish1();

            //Publish2();

            //Publish3();

            Publish4();

            Console.ReadLine();
        }

        static void Publish2()
        {
            var sender = new MqSender();
            foreach (var key in ConfigurationManager.AppSettings["pub.2.keys"].Split(','))
            {
                sender.Publish2(key);
            }
        }

        static void Publish3()
        {
            var sender = new MqSender();
            foreach (var key in ConfigurationManager.AppSettings["pub.3.topics"].Split(','))
            {
                sender.PublishTopic(key);
            }
        }

        static void Publish4()
        {
            var sender = new MqSender();

            sender.RpcCall();
        }
    }
}
