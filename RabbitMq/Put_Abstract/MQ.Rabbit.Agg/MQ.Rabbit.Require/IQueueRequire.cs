﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQ.Rabbit.Require
{
    public interface IQueueRequire
    {
        bool NeedAck { get; }

        bool Fair { get; }
    }
}
