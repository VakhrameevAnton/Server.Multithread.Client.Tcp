using System;

namespace Chat.Client
{
    internal class ConsoleMenu
    {
        private readonly string[] _menuItems;
        private int _counter;

        public ConsoleMenu(string[] menuItems)
        {
            this._menuItems = menuItems;
        }

        public string PrintMenu()
        {
            var baseBgColor = Console.BackgroundColor;
            var baseFgColor = Console.ForegroundColor;
            
            ConsoleKeyInfo key;
            var init = true;
            Console.WriteLine("\n");

            do
            {
                if (!init)
                    Console.SetCursorPosition(0, Console.CursorTop - _menuItems.Length);

                for (var i = 0; i < _menuItems.Length; i++)
                    if (_counter == i)
                    {
                        Console.BackgroundColor = ConsoleColor.Cyan;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine(_menuItems[i]);
                        Console.BackgroundColor = baseBgColor;
                        Console.ForegroundColor = baseFgColor;
                    }
                    else
                    {
                        Console.WriteLine(_menuItems[i]);
                    }

                key = Console.ReadKey();

                init = false;
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                    {
                        _counter--;
                        if (_counter == -1) _counter = _menuItems.Length - 1;
                        break;
                    }
                    case ConsoleKey.DownArrow:
                    {
                        _counter++;
                        if (_counter == _menuItems.Length) _counter = 0;
                        break;
                    }
                }
            } while (key.Key != ConsoleKey.Enter);

            return _menuItems[_counter];
        }
    }
}