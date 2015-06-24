using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQ.Rabbit.Configuration
{
    public class RabbitMqConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("server", IsRequired = true)]
        public RabbitServerElement Server
        {
            get
            {
                return (RabbitServerElement)base["server"];
            }

            set
            {
                base["server"] = value;
            }
        }
    }

    public class RabbitServerElement : ConfigurationElement
    {
        [ConfigurationProperty("host", IsRequired = true)]
        public string Host
        {
            get { return (string) base["host"]; }

            set { base["host"] = value; }
        }

        [ConfigurationProperty("user", IsRequired = true)]
        public string User
        {
            get { return (string)base["user"]; }

            set { base["user"] = value; }
        }

        [ConfigurationProperty("password", IsRequired = true)]
        public string Password
        {
            get { return (string)base["password"]; }

            set { base["password"] = value; }
        }
    }
}
