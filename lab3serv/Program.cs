using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;

class Arbiter
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        
        TcpListener server = new TcpListener(IPAddress.Any, 5000);
        server.Start();
        Console.WriteLine("Арбитр запущен. Ожидание игроков...");

        // Принимаем подключение двух игроков
        TcpClient player1 = server.AcceptTcpClient();
        Console.WriteLine("Игрок 1 подключен.");
        TcpClient player2 = server.AcceptTcpClient();
        Console.WriteLine("Игрок 2 подключен.");

        // Создаем потоки для обмена сообщениями (используем UTF8)
        var stream1 = player1.GetStream();
        var reader1 = new StreamReader(stream1, Encoding.UTF8);
        var writer1 = new StreamWriter(stream1, Encoding.UTF8) { AutoFlush = true };

        var stream2 = player2.GetStream();
        var reader2 = new StreamReader(stream2, Encoding.UTF8);
        var writer2 = new StreamWriter(stream2, Encoding.UTF8) { AutoFlush = true };

        // Отправляем игрокам количество раундов
        int totalRounds = 3;
        writer1.WriteLine(totalRounds);
        writer2.WriteLine(totalRounds);

        int score1 = 0, score2 = 0;

        // Основной цикл игры
        for (int round = 1; round <= totalRounds; round++)
        {
            Console.WriteLine($"\nРаунд {round}");

            // Получаем выборы игроков
            string choice1 = reader1.ReadLine()?.Trim().ToUpper() ?? "";
            string choice2 = reader2.ReadLine()?.Trim().ToUpper() ?? "";

            Console.WriteLine($"Игрок 1 выбрал: \"{choice1}\"");
            Console.WriteLine($"Игрок 2 выбрал: \"{choice2}\"");

            // Определяем победителя
            string result = Evaluate(choice1, choice2);
            if (result == "Игрок 1 победил!")
                score1++;
            else if (result == "Игрок 2 победил!")
                score2++;

            writer1.WriteLine($"Результат: {result}");
            writer2.WriteLine($"Результат: {result}");
        }

        // Вывод итогов игры
        Console.WriteLine("\nИтоги игры:");
        Console.WriteLine($"Игрок 1: {score1} побед.");
        Console.WriteLine($"Игрок 2: {score2} побед.");

        // Закрытие соединений
        player1.Close();
        player2.Close();
        server.Stop();
    }

    // Метод для определения победителя раунда
    static string Evaluate(string c1, string c2)
    {
        if (c1 == c2)
            return "Ничья!";

        // Правила игры:
        // Rock (R) побеждает Scissors (S),
        // Scissors (S) побеждают Paper (P),
        // Paper (P) побеждает Rock (R)
        if ((c1 == "R" && c2 == "S") ||
            (c1 == "S" && c2 == "P") ||
            (c1 == "P" && c2 == "R"))
            return "Игрок 1 победил!";
        else
            return "Игрок 2 победил!";
    }
}
