using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Events.Drone
{
    public class DroneEventArgs
    {
        public string Message { get; }
        public string Direction { get; }

        public DroneEventArgs(string message, string direction)
        {
            Message = message;
            Direction = direction;
        }
    }
}
