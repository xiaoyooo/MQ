using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQ.Message
{
    public interface IMessageProperties
    {
        string ContentEncoding { get; set; }

        string ContentType { get; set; }
    }
}
