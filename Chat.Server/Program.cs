using System;
using System.Threading;

namespace Chat.Server
{
    internal class Program
    {
        private const int Port = 4000; // порт для прослушивания подключений

        private static void Main(string[] args)
        {
            // Определим нужное максимальное количество потоков
            // Пусть будет по 4 на каждый процессор
            var maxThreadsCount = Environment.ProcessorCount * 4;
            // Установим максимальное количество рабочих потоков
            ThreadPool.SetMaxThreads(maxThreadsCount, maxThreadsCount);
            // Установим минимальное количество рабочих потоков
            ThreadPool.SetMinThreads(2, 2);
            // Создадим новый сервер на порту 80
            Server.Start(Port);
        }
    }
}