using Google.Protobuf;
using Google.Protobuf.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileTales.Utils
{
    enum EventType
    {
        Connect,
        LoadGameUI,
        Quit,
        ConnectFailed,
        Connected,
        GameUILoaded,
        AllTilesSaved,
        LoadWorldGenUI,
        GameWorldGenUILoaded,
        LoadWorldGenUII
    }
    internal class Event
    {
        public EventType eventType;
        public object data;
        public Event(EventType eventType, object data)
        {
            this.eventType = eventType;
            this.data = data;
        }
    }

    internal class ProtoEvent
    {
        public MessageDescriptor eventMessage;
        public object data;
        public ProtoEvent(MessageDescriptor eventMessage, object data)
        {
            this.eventMessage = eventMessage;
            this.data = data;
        }
    }
}
