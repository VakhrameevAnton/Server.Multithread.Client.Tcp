using System;
using System.Net.Sockets;
using Newtonsoft.Json;
using Сhat.Common;

namespace Chat.Client
{
    public static class Client
    {
        public static void Start(string serverUrl, int serverPort)
        {
            try
            {
                var client = new TcpClient();
                client.Connect(serverUrl, serverPort);

                Console.WriteLine("Клиент запущен..");

                var buffer = new byte[256];
                var stream = client.GetStream();

                do
                {
                    //Получаем запрос имени
                    var greeting = stream.ReadMessage(buffer);
                    Console.WriteLine(greeting);

                    stream.WriteMessage(Console.ReadLine());
                } while (stream.DataAvailable);

                string[] menu;
                do
                {
                    //Получаем возможные пункты меню с сервера
                    var menuJson = stream.ReadMessage(buffer);
                    menu = JsonConvert.DeserializeObject<string[]>(menuJson);
                } while (stream.DataAvailable);

                var consoleMenu = new ConsoleMenu(menu);

                var active = true;
                while (active)
                {
                    string menuResult;
                    do
                    {
                        //Рисуем меню в консоли и ожидаем выбор пункта
                        menuResult = consoleMenu.PrintMenu();

                        try
                        {
                            //Отправляем выбранный пункт
                            stream.WriteMessage(menuResult);

                            do
                            {
                                //Получаем ответ
                                var response = stream.ReadMessage(buffer);

                                Console.WriteLine("\n");
                                Console.WriteLine(response);
                            } while (stream.DataAvailable); // пока данные есть в потоке
                            
                            //если пользователь не прощается, повторяем цикл
                            if (!menuResult.Equals("Пока"))
                                continue;

                            //в ином случае прерываем цикл
                            active = false;

                            // Закрываем потоки
                            stream.Close();
                            client.Close();
                            break;
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Сервер временно недоступен");

                            active = false;

                            // Закрываем потоки
                            stream.Close();
                            client.Close();
                        }
                    } while (menuResult.IndexOf(menuResult, StringComparison.Ordinal) != menu.Length - 1);
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
            }

            Console.WriteLine("Диалог завершен...");
            Console.WriteLine("Для выхода нажмите любую клавишу...");
            Console.Read();
        }
    }
}