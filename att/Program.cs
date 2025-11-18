using System;

namespace PhotoStudio
{
    /// <summary>
    /// Точка входа в консольное приложение фотостудии.
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8; // корректное отображение кириллицы
            var app = new PhotoStudioConsoleApp();
            app.Run();
        }
    }
}


