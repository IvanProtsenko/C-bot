using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CSharpVKbot
{
    internal class Program
    {
        /// <summary>
        /// Признак завершения приложения
        /// </summary>
        private static bool finish = false;

        internal static Configuration Cfg;

        /// <summary>
        /// Точка входа в приложение
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            try
            {
             
                // Загрузка конфигурации
                Cfg = Configuration.Load("Configuration.xml");

                // Создание бота и установление соединения
                VKontakteBot bot = new VKontakteBot();
                // Обработчик нажатия на Ctrl+C
                Console.CancelKeyPress += Console_CancelKeyPress;
                Console.Title = "Бот vk.com";
                Console.WriteLine("Бот запущен. Нажмите Ctrl+C для завершения");
                // Пока не нажмем на Ctrl+C
                while (!finish)
                {
                    // Обработка входящих сообщений
                    try
                    {
                        bot.PollServer();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    // Пауза 200 мс
                    System.Threading.Thread.Sleep(200);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }

        /// <summary>
        /// Обработка нажатия на Ctrl+C
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            // Отменить завершение процесса
            e.Cancel = true;
            // Завершение главного цикла ожидания
            finish = true;
        }
    }
}
