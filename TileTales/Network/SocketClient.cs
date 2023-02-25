using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Net.Tiletales.Network.Proto.App;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using System.IO;

namespace TileTales.Network
{
    public class SocketClient
    {
        private static int BUFFER_SIZE = 1024 * 1024; // 1MB read buffer
        private TcpClient client;
        private NetworkStream stream;
        private Boolean keepReading = true;

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

        private void Send(byte[] messageBytes)
        {
            System.Diagnostics.Debug.WriteLine("SocketClient SENDING: SendMessageBytes: " + messageBytes);
            stream.Write(messageBytes, 0, messageBytes.Length);
            System.Diagnostics.Debug.WriteLine("SocketClient SENT_A: SendMessageBytes.Length: " + messageBytes.Length);
            // Create String from byte[]
            string strMessage = Encoding.UTF8.GetString(messageBytes);
            System.Diagnostics.Debug.WriteLine("SocketClient SENT_B: SendMessageBytes as string: " + strMessage);
        }

        public void SendMessageBytes(byte[] messageBytes)
        {
            // Send a message to the server in its own thread
            Task.Run(() => Send(messageBytes));

            //Send(messageBytes);
        }

        public void SendMessage(Any message)
        {
            byte[] messageBytes = message.ToByteArray();
            SendMessageBytes(messageBytes);
        }

        public void ReadFromStream(MessageCallback messageCallback)
        {
            System.Diagnostics.Debug.WriteLine("SocketClient.ReadFromStream START");
            byte[] buffer = new byte[BUFFER_SIZE];
            while (keepReading)
            {
                System.Diagnostics.Debug.WriteLine("SocketClient.ReadFromStream keepReading: " + keepReading);
                int bytesRead = 0;
                int totalBytesRead = 0;
                try
                {
                    bytesRead = stream.Read(buffer, totalBytesRead, buffer.Length - totalBytesRead);
                }
                catch (Exception e)
                {
                    handleReadException(e);
                    break;
                }
                if (bytesRead == -1)
                {
                    shutdown();
                    break;
                }
                totalBytesRead += bytesRead;

                System.Diagnostics.Debug.WriteLine("SocketClient.ReadFromStream bytesRead: " + bytesRead);
                /*
                 * Heres the deal; protobuf messages might come several at once
                 * They are packed with the most recent one first, and the oldest one last
                 * Parsing the entire lump of bytes will result in that only the last message in the buffer is "found"
                 * But that is the only way we can figure out how much data that each message used up.
                 * So we read all messages in reverse order and put them in a list, and then just reverse the list,
                 * to make the client read order match the server send order.
                 */
                List<Any> messages = new List<Any>();
                int usedBytes = 0;
                while (usedBytes < bytesRead)
                {
                    byte[] readBytes = new byte[bytesRead - usedBytes];
                    Array.Copy(buffer, 0, readBytes, 0, bytesRead - usedBytes);
                    Any messagePart = Any.Parser.ParseFrom(readBytes);
                    int messageLength = messagePart.CalculateSize();
                    usedBytes += messageLength;
                    messages.Add(messagePart);
                }
                messages.Reverse();
                messages.ForEach(message => messageCallback(message));
            }
        }

        private void handleReadException(Exception e)
        {
            shutdown();
        }

        public bool isConnected()
        {
            if (client != null && stream != null)
            {
                return client.Connected && stream.CanRead;
            }
            return false;
        }

        public void shutdown()
        {
            System.Diagnostics.Debug.WriteLine("SocketClient.shutdown()");
            keepReading = false;
            if (stream != null)
            {
                try
                {
                    stream.DisposeAsync();
                    stream.Socket.Shutdown(SocketShutdown.Both);
                } catch (Exception e) { }
            }
            if (client != null)
            {
                try
                {
                    client.Close();
                    client.Dispose();
                }
                catch (Exception e) { }
            }
        }
    }
}