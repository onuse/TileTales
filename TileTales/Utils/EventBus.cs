using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileTales.Utils
{
    internal class EventBus
    {
        private static EventBus _instance;
        private readonly Dictionary<string, List<Action<object>>> _eventListeners = new Dictionary<string, List<Action<object>>>();
        private readonly Stack<Event> queue= new Stack<Event>();

        public static EventBus Instance
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

        public void Subscribe(string eventName, Action<object> callback)
        {
            if (!_eventListeners.ContainsKey(eventName))
            {
                _eventListeners.Add(eventName, new List<Action<object>>());
            }
            _eventListeners[eventName].Add(callback);
        }

        public void Unsubscribe(string eventName, Action<object> callback)
        {
            if (!_eventListeners.ContainsKey(eventName))
            {
                return;
            }
            _eventListeners[eventName].Remove(callback);
        }

        /*public void Publish(string eventName, object data)
        {
            if (!_eventListeners.ContainsKey(eventName))
            {
                return;
            }
            foreach (var callback in _eventListeners[eventName])
            {
                callback(data);
            }
        }*/
        public void Publish(string eventName, object data)
        {
            if (!_eventListeners.ContainsKey(eventName))
            {
                return;
            }
            queue.Push(new Event(eventName, data));
        }

        public void Update()
        {
            while (queue.Count > 0)
            {
                Event e = queue.Pop();
                if (e == null)
                {
                    continue;
                }
                foreach (var callback in _eventListeners[e.eventName])
                {
                    callback(e.data);
                }
            }
        }
    }
}
