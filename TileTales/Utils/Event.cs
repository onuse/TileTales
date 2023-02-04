using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileTales.Utils
{
    internal class Event
    {
        public string eventName;
        public object data;
        public Event(string eventName, object data)
        {
            this.eventName = eventName;
            this.data = data;
        }
    }
}
