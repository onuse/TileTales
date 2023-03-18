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
using TileTales.Utils;

namespace TileTales.Network
{
    public class SocketClient
    {
        private static readonly int s_bufferSize = 1024 * 1024; // 1 MB read buffer
        private TcpClient _client;
        private NetworkStream _stream;
        private Boolean _keepReading = true;

        public delegate void MessageCallback(Any message);

        public Exception Connect(IPAddress hostAdress, int hostPort)
        {
            try
            {
                _client = new TcpClient();
                _client.Connect(hostAdress, hostPort);
                _stream = _client.GetStream();
                return null;
            } catch (Exception e)
            {
                Log.Error("Connect failed: " + e);
                return e;
            }
        }

        private void Send(byte[] messageBytes)
        {
            Log.Verbose("SENDING messageBytes: " + messageBytes);
            _stream.Write(messageBytes, 0, messageBytes.Length);
            Log.Debug("SENT messageBytes.Length: " + messageBytes.Length);
            // Create String from byte[]
            string strMessage = Encoding.UTF8.GetString(messageBytes);
            Log.Verbose("SENT messageBytes as string: " + strMessage);
        }

        public void SendMessageBytes(byte[] messageBytes)
        {
            //System.Diagnostics.Debug.WriteLine("SocketClient.SendMessageBytes() SENDING: messageBytes: " + messageBytes);
            // Send a message to the server in its own thread
            //Task.Run(() => Send(messageBytes));

            Parallel.Invoke(() => Send(messageBytes));

            //Send(messageBytes);
        }

        public void SendMessage(Any message)
        {
            byte[] messageBytes = message.ToByteArray();
            SendMessageBytes(messageBytes);
        }

        public void ReadFromStream(MessageCallback messageCallback)
        {
            Log.Debug("START Thread.CurrentThread.ManagedThreadId: " + Thread.CurrentThread.ManagedThreadId);
            byte[] buffer = new byte[s_bufferSize];
            while (_keepReading)
            {
                Log.Verbose("keepReading: " + _keepReading);
                int bytesRead = 0;
                int totalBytesRead = 0;
                try
                {
                    bytesRead = _stream.Read(buffer, totalBytesRead, buffer.Length - totalBytesRead);
                }
                catch (Exception e)
                {
                    handleReadException(e);
                    break;
                }
                if (bytesRead == -1)
                {
                    Shutdown();
                    break;
                }
                totalBytesRead += bytesRead;

                Log.Debug("bytesRead: " + bytesRead);
                /*
                 * Heres the deal; protobuf messages might come several at once
                 * They are packed with the most recent one first, and the oldest one last
                 * Parsing the entire lump of bytes will result in that only the last message in the buffer is "found"
                 * But that is the only way we can figure out how much data that each message used up.
                 * So we read all messages front to back, and then just reverse the list,
                 * to make the client read order match the server send order.
                 * Not always important, but since we are relying on TCP anyway, someone might rely on it.
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
            Log.Fatal("ReadFromStream failed: " + e);
            Shutdown();
        }

        public bool IsConnected()
        {
            if (_client != null && _stream != null)
            {
                return _client.Connected && _stream.CanRead;
            }
            return false;
        }

        public void Shutdown()
        {
            Log.Info("Shutting down");
            _keepReading = false;
            if (_stream != null)
            {
                try
                {
                    _stream.Dispose();
                    _stream.Socket.Shutdown(SocketShutdown.Both);
                } catch (Exception) { }
            }
            if (_client != null)
            {
                try
                {
                    _client.Close();
                    _client.Dispose();
                }
                catch (Exception) { }
            }
        }
    }
}