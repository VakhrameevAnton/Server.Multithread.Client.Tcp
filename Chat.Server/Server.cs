using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Newtonsoft.Json;
using Сhat.Common;

namespace Chat.Server
{
    public class Server
    {
        private static TcpListener _listener; // Объект, принимающий TCP-клиентов

        // Запуск сервера
        public static void Start(int port)
        {
            _listener = new TcpListener(IPAddress.Any, port); // Создаем "слушателя" для указанного порта
            _listener.Start(); // Запускаем его
            
            Console.WriteLine("Ожидаем новое подключение..");

            while (true)
                // Принимаем новых клиентов. После того, как клиент был принят, он передается в новый поток (ClientThread)
                ThreadPool.QueueUserWorkItem(ClientThread, _listener.AcceptTcpClient());
        }

        private static void ClientThread(object stateInfo)
        {
            var client = (TcpClient) stateInfo;
            Console.WriteLine("Подключен клиент. Выполнение запроса...");

            var buffer = new byte[client.ReceiveBufferSize];
            var stream = client.GetStream();

            stream.WriteMessage("Как тебя зовут");

            do
            {
                var userName = stream.ReadMessage(buffer);
                Console.WriteLine($"Пользователь: {userName}, подключен;");
            } while (stream.DataAvailable); // пока данные есть в потоке

            stream.WriteMessage(JsonConvert.SerializeObject(ActionsMenu.Get()));
            
            var active = true;
            while (active)
            {
                // получаем сетевой поток для чтения и записи
                stream = client.GetStream();
                try
                {
                    do
                    {
                        //Получаем запрос пользователя
                        var utfString = stream.ReadMessage(buffer);
                        Console.WriteLine("Получено сообщение: {0}", utfString);
                        
                        //Генерируем ответ в соответствии с выбранным пунктом
                        var response = ActionsMenu.PointExecute(utfString);

                        //Отправляем ответ
                        stream.WriteMessage(response);

                        //Если с нами не попрощались, повторяем цикл
                        if (!response.Equals("Пока"))
                            continue;

                        // закрываем поток
                        stream.Close();
                        // закрываем подключение
                        client.Close();

                        return;
                    } while (stream.DataAvailable); // пока данные есть в потоке
                }
                catch (Exception)
                {
                    Console.WriteLine("Произошла ошибка, закрываем соединение...");

                    active = false;
                    stream.Close();
                    client.Close();
                }
            }
        }

        // Остановка сервера
        ~Server()
        {
            _listener?.Stop();
        }
    }
}