using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Net;
using System.Threading;
using TileTales.Utils;

namespace TileTales.Network {
    internal class ServerConnector {
        private readonly SettingsReader _settingsReader;
        private readonly EventBus _eventBus;
        private SocketClient _socketClient;
        private Thread _thread;

        public ServerConnector() {
            this._eventBus = EventBus.Singleton;
            this._settingsReader = SettingsReader.Singleton;
            _eventBus.Subscribe(EventType.Connect, (o) => {
                _thread = new Thread(ConnectToServer);
                _thread.Start();
            });
        }

        public void ConnectToServer() {
            // Initialize the SocketClient
            _socketClient = new SocketClient();
            UserSettings set = _settingsReader.GetSettings();
            Exception e = _socketClient.Connect(IPAddress.Parse(set.ServerAdress), set.ServerPort);
            if (e != null) {
                _eventBus.Publish(EventType.ConnectFailed, e.Message);
            } else {
                StartReadingStream();
                _eventBus.Publish(EventType.Connected, null);
            }
        }


        public void StartReadingStream() {
            Log.Info("Starting reading stream on thread: " + Thread.CurrentThread.ManagedThreadId);
            _thread = new Thread(() => _socketClient.ReadFromStream(MessageCallback));
            _thread.Start();
        }
        public void MessageCallback(Any message) {
            Log.Verbose("message RECIEVED: " + message);

            /*String typeUrl = message.TypeUrl;
            String type = typeUrl.Substring(typeUrl.LastIndexOf(".") + 1);

            var typ = Assembly.GetExecutingAssembly().GetTypes().First(t => t.Name == type); //get the type using the string we got, here it is 'Person'
            var descriptor = (MessageDescriptor)typ.GetProperty("Descriptor", BindingFlags.Public | BindingFlags.Static).GetValue(null, null); // get the static property Descriptor

            var mess = (IMessage)message;
            System.Diagnostics.Debug.WriteLine("ServerConnector.MessageCallback() descriptor.Name: " + descriptor.Name);
            //var descriptor = (MessageDescriptor)mess.GetProperty("Descriptor", BindingFlags.Public | BindingFlags.Static).GetValue(null, null); // get the static property Descriptor
            //String typeUrl = message.TypeUrl;
            System.Diagnostics.Debug.WriteLine("ServerConnector.MessageCallback() message.TypeUrl: " + typeUrl);
            //String type = typeUrl.Substring(typeUrl.LastIndexOf(".") + 1);
            System.Diagnostics.Debug.WriteLine("ServerConnector.MessageCallback() message type: " + type);*/
            _eventBus.Publish(message, message);
        }

        public void SendMessage(Any message) {
            _socketClient.SendMessage(message);
        }
        public void SendMessageBytes(byte[] messageBytes) {
            _socketClient.SendMessageBytes(messageBytes);
        }
        public void SendMessage(IMessage message) {
            Any msg = Any.Pack(message);
            SendMessage(msg);
        }

        public bool IsConnected() {
            return _socketClient.IsConnected();
        }

        internal void Shutdown() {
            if (_socketClient != null)
                _socketClient.Shutdown();
        }
    }
}
