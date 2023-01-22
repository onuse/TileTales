using Google.Protobuf.WellKnownTypes;
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
            Settings set = _settingsReader.GetSettings();
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
            // Start a new thread to receive messages from the server
            //thread = new Thread(_socketClient.ReadFromStream);
            //thread.Start(MessageCallback);

            thread = new Thread(() => _socketClient.ReadFromStream(MessageCallback));
            thread.Start();
        }
        public void MessageCallback(Any message)
        {
            System.Diagnostics.Debug.WriteLine("ServerConnector.MessageCallback() message: " + message);
            System.Diagnostics.Debug.WriteLine("ServerConnector.MessageCallback() message.TypeUrl: " + message.TypeUrl);
            _eventBus.Publish(message.TypeUrl, message);
        }

        public void SendMessage(Any message)
        {
            _socketClient.SendMessage(message);
        }

        public void SendMessageBytes(byte[] messageBytes)
        {
            _socketClient.SendMessageBytes(messageBytes);
        }

        public bool isConnected()
        {
            return _socketClient.isConnected();
        }
    }
}
