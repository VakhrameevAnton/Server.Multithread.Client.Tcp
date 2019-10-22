using System;
using System.Net.Sockets;
using System.Text;

namespace Сhat.Common
{
    public static class NetworkStreamExtension
    {
        public static void WriteMessage(this NetworkStream stream, string messageString)
        {
            var messageData = Encoding.UTF8.GetBytes(messageString);
            stream.Write(messageData, 0, messageData.Length);
        }

        public static string ReadMessage(this NetworkStream stream, byte[] buffer)
        {
            Array.Clear(buffer, 0, buffer.Length);
            stream.Read(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer, 0, buffer.Length).TrimEnd('\0');
        }
    }
}