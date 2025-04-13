using System;
using System.Net.Sockets;
using System.IO;
using System.Text;

class PlayerClient
{
    static void Main()
    {
        // Настройка кодировки консоли
        Console.InputEncoding = Encoding.UTF8;
        Console.OutputEncoding = Encoding.UTF8;
        
        using TcpClient client = new TcpClient("127.0.0.1", 5000);
        var stream = client.GetStream();
        using StreamReader reader = new StreamReader(stream, Encoding.UTF8);
        using StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

        // Принимаем количество раундов от сервера
        string roundsStr = reader.ReadLine() ?? "0";
        if (!int.TryParse(roundsStr, out int rounds))
        {
            Console.WriteLine("Invalid number of rounds received.");
            return;
        }
        Console.WriteLine($"The game will be played in {rounds} rounds.");

        // Каждый раунд: ввод выбора, отправка на сервер и получение результата
        for (int i = 1; i <= rounds; i++)
        {
            Console.Write($"Round {i}: Enter your choice (R - Rock, S - Scissors, P - Paper): ");
            string? input = Console.ReadLine();
            // Если ввод пуст, выбираем по умолчанию Rock (R)
            string choice = (input ?? "R").Trim().ToUpper();
            writer.WriteLine(choice);

            string result = reader.ReadLine() ?? "No response from server.";
            Console.WriteLine(result);
        }
    }
}
