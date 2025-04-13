using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;

class Arbiter
{
    static void Main()
    {
        // Настройка кодировки консоли
        Console.OutputEncoding = Encoding.UTF8;
        
        TcpListener server = new TcpListener(IPAddress.Any, 5000);
        server.Start();
        Console.WriteLine("Arbiter started. Waiting for players...");

        // Принимаем подключение двух игроков
        TcpClient player1 = server.AcceptTcpClient();
        Console.WriteLine("Player 1 connected.");
        TcpClient player2 = server.AcceptTcpClient();
        Console.WriteLine("Player 2 connected.");

        // Создаем потоки для обмена сообщениями (используем UTF8)
        var stream1 = player1.GetStream();
        var reader1 = new StreamReader(stream1, Encoding.UTF8);
        var writer1 = new StreamWriter(stream1, Encoding.UTF8) { AutoFlush = true };

        var stream2 = player2.GetStream();
        var reader2 = new StreamReader(stream2, Encoding.UTF8);
        var writer2 = new StreamWriter(stream2, Encoding.UTF8) { AutoFlush = true };

        // Сообщаем игрокам число раундов
        int totalRounds = 3;
        writer1.WriteLine(totalRounds);
        writer2.WriteLine(totalRounds);

        int score1 = 0, score2 = 0;

        for (int round = 1; round <= totalRounds; round++)
        {
            Console.WriteLine($"\nRound {round}");

            // Получаем выборы от игроков
            string choice1 = reader1.ReadLine()?.Trim().ToUpper() ?? "";
            string choice2 = reader2.ReadLine()?.Trim().ToUpper() ?? "";

            Console.WriteLine($"Player 1 chose: \"{choice1}\"");
            Console.WriteLine($"Player 2 chose: \"{choice2}\"");

            // Определяем победителя
            string result = Evaluate(choice1, choice2);
            if (result == "Player 1 wins!")
                score1++;
            else if (result == "Player 2 wins!")
                score2++;

            // Отправляем результат обоим игрокам
            writer1.WriteLine($"Result: {result}");
            writer2.WriteLine($"Result: {result}");
        }

        // Итоговая таблица результатов на сервере
        Console.WriteLine("\nFinal results:");
        Console.WriteLine($"Player 1: {score1} wins.");
        Console.WriteLine($"Player 2: {score2} wins.");

        // Закрываем соединения
        player1.Close();
        player2.Close();
        server.Stop();
    }

    // Метод для определения победителя раунда
    static string Evaluate(string c1, string c2)
    {
        if (c1 == c2)
            return "Draw!";

        // Правила игры:
        // Rock beats Scissors, Scissors beats Paper, Paper beats Rock
        if ((c1 == "R" && c2 == "S") ||
            (c1 == "S" && c2 == "P") ||
            (c1 == "P" && c2 == "R"))
            return "Player 1 wins!";
        else
            return "Player 2 wins!";
    }
}
