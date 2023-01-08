using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TileTales.Network
{
    public class SocketClient
    {
        private TcpClient client;
        private NetworkStream stream;
        private Thread thread;

        public void Connect(IPAddress hostAdress, int hostPort)
        {
            // Connect to the server
            client = new TcpClient();

            client.Connect(hostAdress, hostPort);
            stream = client.GetStream();

            // Start a new thread to receive messages from the server
            thread = new Thread(new ThreadStart(ReceiveMessages));
            thread.Start();
        }

        public void SendMessage(byte[] message)
        {
            // Send a message to the server
            stream.Write(message, 0, message.Length);
        }

        private void ReceiveMessages()
        {
            while (true)
            {
                // Receive a message from the server
                byte[] message = new byte[4096];
                int bytesRead = stream.Read(message, 0, message.Length);
                if (bytesRead == 0)
                {
                    // Connection was closed
                    break;
                }

                // Process the received message here
            }
        }
    }
}