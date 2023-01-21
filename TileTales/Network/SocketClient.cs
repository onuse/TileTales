using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Net.Tiletales.Network.Protomsg.App;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;

namespace TileTales.Network
{
    public class SocketClient
    {
        private TcpClient client;
        private NetworkStream stream;

        public delegate void MessageCallback(Any message);

        public Exception Connect(IPAddress hostAdress, int hostPort)
        {
            try
            {
                // Connect to the server
                client = new TcpClient();

                client.Connect(hostAdress, hostPort);
                stream = client.GetStream();

                return null;
            } catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("SocketClient.Connect() failed: " + e);
                return e;
            }
        }

        public void SendMessageBytes(byte[] message)
        {
            // Send a message to the server
            stream.Write(message, 0, message.Length);
        }

        public void SendMessage(Any message)
        {
            byte[] messageBytes = message.ToByteArray();
            stream.Write(messageBytes, 0, messageBytes.Length);
        }

        public void ReadFromStream(object callback)
        {
            MessageCallback messageCallback = callback as MessageCallback;
            while (true)
            {
                // Receive a message from the server
                byte[] messageBytes = new byte[4096];
                int bytesRead = stream.Read(messageBytes, 0, messageBytes.Length);
                if (bytesRead == 0)
                {
                    // Connection was closed
                    break;
                }
                messageCallback(Any.Parser.ParseFrom(messageBytes));
            }
        }
    }
}