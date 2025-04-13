using System;
using System.Net.Sockets;
using System.IO;
using System.Text;

class PlayerClient
{
    static void Main()
    {
        Console.InputEncoding = Encoding.UTF8;
        Console.OutputEncoding = Encoding.UTF8;
        
        using TcpClient client = new TcpClient("127.0.0.1", 5000);
        var stream = client.GetStream();
        using StreamReader reader = new StreamReader(stream, Encoding.UTF8);
        using StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

        // Получаем от сервера количество раундов
        string roundsStr = reader.ReadLine() ?? "0";
        if (!int.TryParse(roundsStr, out int rounds))
        {
            Console.WriteLine("Неверное значение количества раундов, полученное от сервера.");
            return;
        }
        Console.WriteLine($"Будет сыграно {rounds} раундов.");

        // Каждый раунд: ввод выбора, отправка и получение результата
        for (int i = 1; i <= rounds; i++)
        {
            Console.Write($"Раунд {i}: Enter your choice (R - Rock, S - Scissors, P - Paper): ");
            string? input = Console.ReadLine();
            // Если ввод пустой, выбираем по умолчанию "R"
            string choice = (input ?? "R").Trim().ToUpper();
            writer.WriteLine(choice);

            string result = reader.ReadLine() ?? "Нет ответа от сервера.";
            Console.WriteLine(result);
        }
    }
}
