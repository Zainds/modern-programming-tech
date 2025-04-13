using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

public class RockPaperScissorsClient
{
    public static void Start()
    {
        try
        {
            TcpClient client = new TcpClient("127.0.0.1", 5000);
            StreamReader reader = new StreamReader(client.GetStream(), Encoding.UTF8);
            StreamWriter writer = new StreamWriter(client.GetStream(), Encoding.UTF8) { AutoFlush = true };

            Console.WriteLine("Подключено к серверу. Ожидание начала игры...");

            string whoAmI = reader.ReadLine();
            Console.WriteLine(whoAmI);

            string roundsStr = reader.ReadLine();
            int rounds = int.Parse(roundsStr);
            Console.WriteLine($"Количество раундов: {rounds}");

            for (int i = 0; i < rounds; i++)
            {
                Console.WriteLine($"\nРаунд {i + 1}");

                // Ожидание сигнала от сервера для начала ввода
                string serverSignal = reader.ReadLine();
                if (serverSignal != "Ваш ход")
                {
                    Console.WriteLine("Ошибка синхронизации с сервером");
                    break;
                }

                string move;
                do
                {
                    Console.Write("Введите ваш ход (К - камень, Н - ножницы, Б - бумага): ");
                    move = Console.ReadLine()?.ToUpper().Trim();
                } while (string.IsNullOrEmpty(move) || !"КНБ".Contains(move));

                writer.WriteLine(move);

                string result = reader.ReadLine();
                if (result == "Ошибка")
                {
                    Console.WriteLine("Произошла ошибка в игре");
                    continue;
                }

                Console.WriteLine("Результат партии: " +
                    (result == "1" ? "Вы выиграли" :
                     result == "2" ? "Вы проиграли" :
                     "Ничья"));
            }

            Console.WriteLine("\nИгра завершена");
            client.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка: " + ex.Message);
        }
    }
}