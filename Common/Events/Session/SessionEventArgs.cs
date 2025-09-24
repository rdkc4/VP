﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Events.Session
{
    public class SessionEventArgs
    {
        public string Message { get; }

        public SessionEventArgs(string message)
        {
            Message = message;
        }
    }
}
