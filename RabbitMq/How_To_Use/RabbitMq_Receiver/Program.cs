using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMq_Receiver
{
    class Program
    {
        static void Main(string[] args)
        {
            //new MqReceiver().Receive();

            //new MqReceiver().Suscribe1();

            //Suscribe2();

            //Suscribe3();

            DualWork_Server1();

            Console.ReadLine();
        }

        static void Suscribe2()
        {
            var keys = ConfigurationManager.AppSettings["pub.2.keys"].Split(',');
            var random = new Random((int)DateTime.Now.Ticks);
            var index = random.Next(0, keys.Count());

            new MqReceiver().Suscribe2(keys[index]);
        }

        static void Suscribe3()
        {
            var keys = ConfigurationManager.AppSettings["pub.3.topicpatterns"].Split(',');
            var random = new Random((int)DateTime.Now.Ticks);
            var index = random.Next(0, keys.Count());

            new MqReceiver().SuscribeTopic(keys[index]);
        }

        static void DualWork_Server1()
        {
            new MqReceiver().RpcService();
        }
    }
}
