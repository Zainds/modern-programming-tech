using System;
using System.Net.Sockets;
using System.Text;

class PlayerClient
{
    static void Main()
    {
        TcpClient client = new TcpClient("127.0.0.1", 5000);
        NetworkStream stream = client.GetStream();

        string roundsMsg = ReadFromServer(stream);
        int rounds = int.Parse(roundsMsg);
        Console.WriteLine($"Будет сыграно {rounds} партий.");

        for (int i = 0; i < rounds; i++)
        {
            Console.Write($"Раунд {i + 1}: Введите ваш выбор (К-камень, Н-ножницы, Б-бумага): ");
            string choice = Console.ReadLine()?.ToUpper().Trim();

            SendToServer(stream, choice);
            string result = ReadFromServer(stream);
            Console.WriteLine($"Результат: {result}\n");
        }

        stream.Close();
        client.Close();
    }

    static void SendToServer(NetworkStream stream, string message)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(message);
        stream.Write(buffer, 0, buffer.Length);
    }

    static string ReadFromServer(NetworkStream stream)
    {
        byte[] buffer = new byte[256];
        int bytes = stream.Read(buffer, 0, buffer.Length);
        return Encoding.UTF8.GetString(buffer, 0, bytes).Trim();
    }
}
