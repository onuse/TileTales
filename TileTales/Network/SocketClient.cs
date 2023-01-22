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

        public void SendMessageBytes(byte[] messageBytes)
        {
            // Send a message to the server
            stream.Write(messageBytes, 0, messageBytes.Length);
            System.Diagnostics.Debug.WriteLine("SENT: SendMessageBytes: " + messageBytes);
            System.Diagnostics.Debug.WriteLine("SENT2: SendMessageBytes.Length: " + messageBytes.Length);
            // Create String from byte[]
            string strMessage = Encoding.UTF8.GetString(messageBytes);
            System.Diagnostics.Debug.WriteLine("SENT3: SendMessageBytes as string: " + strMessage);
        }

        public void SendMessage(Any message)
        {
            byte[] messageBytes = message.ToByteArray();
            SendMessageBytes(messageBytes);
        }

        public void ReadFromStream(MessageCallback messageCallback)
        {
            while (true)
            {
                // Receive a message from the server
                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead == 0)
                {
                    // Connection was closed
                    break;
                }
                System.Diagnostics.Debug.WriteLine("SocketClient.ReadFromStream bytesRead: " + bytesRead);
                byte[] readBytes = new byte[bytesRead];
                Array.Copy(buffer, 0, readBytes, 0, bytesRead);
                messageCallback(Any.Parser.ParseFrom(readBytes));
            }
        }

        public bool isConnected()
        {
            if (client != null && stream != null)
            {
                return client.Connected && stream.CanRead;
            }
            return false;
        }
    }
}