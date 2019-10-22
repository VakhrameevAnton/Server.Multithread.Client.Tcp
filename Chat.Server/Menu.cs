using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace Chat.Server
{
    public class ActionsMenu
    {
        private static readonly Dictionary<string, Func<string>> MenuTemplate = new Dictionary<string, Func<string>>
        {
            {"Сколько активных", ValueOfActiveThreads},
            {"Который час", GetCurrentDateTime},
            {"Пока", Exit}
        };

        public static IEnumerable<string> Get()
        {
            return MenuTemplate.Keys.ToArray();
        }

        public static string PointExecute(string request)
        {
            return MenuTemplate[request].Invoke();
        }

        private static string ValueOfActiveThreads()
        {
            ThreadPool.GetAvailableThreads(out var workerThreads, out var completionPortThreads);
            ThreadPool.GetMaxThreads(out var maxWorkerThreads, out var maxCompletionPortThreads);
                
            Console.WriteLine(maxWorkerThreads-workerThreads);

            return (maxWorkerThreads-workerThreads).ToString();
        }

        private static string GetCurrentDateTime()
        {
            return DateTime.Now.ToString(CultureInfo.CurrentCulture);
        }

        private static string Exit()
        {
            return "Пока";
        }
    }
}