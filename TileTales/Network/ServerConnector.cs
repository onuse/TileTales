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

        public ServerConnector(EventBus eventBus, SettingsReader settingsReader)
        {
            this._eventBus = eventBus;
            this._settingsReader = settingsReader;
        }

        public Exception connectToServer()
        {
            // Initialize the SocketClient
            _socketClient = new SocketClient();
            Settings set = _settingsReader.GetSettings();
            Exception e = _socketClient.Connect(IPAddress.Parse(set.ServerAdress), set.ServerPort);
            return e;
        }


        public void startReadingStream()
        {

            // Start a new thread to receive messages from the server
            thread = new Thread(_socketClient.ReadFromStream);
            thread.Start(MessageCallback);
        }

        public void MessageCallback(Any message)
        {
            _eventBus.Publish(message.TypeUrl, message);
        }
    }
}
