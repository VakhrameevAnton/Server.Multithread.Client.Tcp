using System;

namespace Chat.Client
{
    internal class Program
    {
        private const int Port = 4000;
        private const string Server = "localhost";

        private static void Main(string[] args)
        {
            Console.Clear();
            Client.Start(Server, Port);
        }
    }
}