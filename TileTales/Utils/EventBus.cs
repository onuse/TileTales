using Google.Protobuf;
using Google.Protobuf.Reflection;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TileTales.Utils
{
    internal class EventBus
    {
        private static EventBus _instance;
        private readonly Dictionary<EventType, List<Action<object>>> _eventListeners = new Dictionary<EventType, List<Action<object>>>();
        private readonly Dictionary<MessageDescriptor, List<Action<object>>> _protoBufeventListeners = new Dictionary<MessageDescriptor, List<Action<object>>>();
        private readonly Stack<Event> queue= new Stack<Event>();
        private readonly Stack<ProtoEvent> protoQueue = new Stack<ProtoEvent>();

        public static EventBus Singleton
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EventBus();
                }
                return _instance;
            }
        }

        private EventBus() {
        }

        public void Subscribe(EventType eventType, Action<object> callback)
        {
            if (!_eventListeners.ContainsKey(eventType))
            {
                _eventListeners.Add(eventType, new List<Action<object>>());
            }
            _eventListeners[eventType].Add(callback);
        }

        public void Subscribe(MessageDescriptor eventType, Action<object> callback)
        {
            if (!_protoBufeventListeners.ContainsKey(eventType))
            {
                _protoBufeventListeners.Add(eventType, new List<Action<object>>());
            }
            _protoBufeventListeners[eventType].Add(callback);
        }

        public void Subscribe(IMessage iMessage, Action<object> callback)
        {
            Subscribe(iMessage.Descriptor, callback);
        }

        public void Unsubscribe(EventType eventType, Action<object> callback)
        {
            if (!_eventListeners.ContainsKey(eventType))
            {
                return;
            }
            _eventListeners[eventType].Remove(callback);
        }

        public void Unsubscribe(MessageDescriptor eventType, Action<object> callback)
        {
            if (!_protoBufeventListeners.ContainsKey(eventType))
            {
                return;
            }
            _protoBufeventListeners[eventType].Remove(callback);
        }
        
        public void Publish(EventType eventType, object data)
        {
            if (!_eventListeners.ContainsKey(eventType))
            {
                return;
            }
            queue.Push(new Event(eventType, data));
        }

        public void Publish(MessageDescriptor descriptor, object data)
        {
            if (!_protoBufeventListeners.ContainsKey(descriptor))
            {
                return;
            }
            protoQueue.Push(new ProtoEvent(descriptor, data));
        }

        public void Publish(Any message, object data)
        {
            String typeUrl = message.TypeUrl;
            String typeStr = typeUrl.Substring(typeUrl.LastIndexOf(".") + 1);
            var type = Assembly.GetExecutingAssembly().GetTypes().First(t => t.Name == typeStr);
            MessageDescriptor descriptor = (MessageDescriptor)type.GetProperty("Descriptor", BindingFlags.Public | BindingFlags.Static).GetValue(null, null);
            Publish(descriptor, data);
        }

        public void Update()
        {
            while (protoQueue.Count > 0 || queue.Count > 0)
            {
                if (protoQueue.Count > 0) {
                    ProtoEvent pe = protoQueue.Pop();
                    if (pe != null)
                    {
                        foreach (var callback in _protoBufeventListeners[pe.eventMessage])
                        {
                            callback(pe.data);
                        }
                    }
                }
                if (queue.Count > 0)
                {
                    Event e = queue.Pop();
                    if (e != null)
                    {
                        foreach (var callback in _eventListeners[e.eventType])
                        {
                            callback(e.data);
                        }
                    }
                }
            }
        }
    }
}
