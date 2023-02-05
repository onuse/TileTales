using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Net.Tiletales.Network.Proto.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TileTales.Utils;

namespace TileTales.Network
{
    internal class ServerConnector
    {
        private readonly SettingsReader _settingsReader;
        private readonly EventBus _eventBus;
        private SocketClient _socketClient;
        private Thread thread;

        public ServerConnector()
        {
            this._eventBus = EventBus.Instance;
            this._settingsReader = SettingsReader.Instance;
            _eventBus.Subscribe("connect", (o) => {
                thread = new Thread(connectToServer);
                thread.Start();
            });
        }

        public void connectToServer()
        {
            // Initialize the SocketClient
            _socketClient = new SocketClient();
            UserSettings set = _settingsReader.GetSettings();
            Exception e = _socketClient.Connect(IPAddress.Parse(set.ServerAdress), set.ServerPort);
            if (e != null)
            {
                _eventBus.Publish("connectfailed", e.Message);
            }
            else
            {
                _eventBus.Publish("connected", null);
                startReadingStream();
            }
        }


        public void startReadingStream()
        {
            System.Diagnostics.Debug.WriteLine("ServerConnector.startReadingStream()");
            thread = new Thread(() => _socketClient.ReadFromStream(MessageCallback));
            thread.Start();
        }
        public void MessageCallback(Any message)
        {
            String typeUrl = message.TypeUrl;
            System.Diagnostics.Debug.WriteLine("ServerConnector.MessageCallback() message RECIEVED: " + message);
            System.Diagnostics.Debug.WriteLine("ServerConnector.MessageCallback() message.TypeUrl: " + typeUrl);
            String type = typeUrl.Substring(typeUrl.LastIndexOf(".") + 1);
            System.Diagnostics.Debug.WriteLine("ServerConnector.MessageCallback() message type: " + type);
            _eventBus.Publish(type, message);
        }

        public void SendMessage(Any message)
        {
            _socketClient.SendMessage(message);
        }
        public void SendMessage(IMessage message)
        {
            Any msg = Any.Pack(message);
            _socketClient.SendMessage(msg);
        }
        public void SendMessageBytes(byte[] messageBytes)
        {
            _socketClient.SendMessageBytes(messageBytes);
        }

        public bool isConnected()
        {
            return _socketClient.isConnected();
        }

        internal void Shutdown()
        {
            if (_socketClient != null)
                _socketClient.shutdown();
        }
    }
}
