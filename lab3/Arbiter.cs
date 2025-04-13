using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Arbiter
{
    static void Main()
    {
        TcpListener server = new TcpListener(IPAddress.Any, 5000);
        server.Start();
        Console.WriteLine("Арбитр запущен. Ожидание игроков...");

        TcpClient player1 = server.AcceptTcpClient();
        Console.WriteLine("Игрок 1 подключен.");
        TcpClient player2 = server.AcceptTcpClient();
        Console.WriteLine("Игрок 2 подключен.");

        NetworkStream stream1 = player1.GetStream();
        NetworkStream stream2 = player2.GetStream();

        int rounds = 3;
        string roundsMsg = rounds.ToString();

        stream1.Write(Encoding.UTF8.GetBytes(roundsMsg), 0, roundsMsg.Length);
        stream2.Write(Encoding.UTF8.GetBytes(roundsMsg), 0, roundsMsg.Length);

        int score1 = 0, score2 = 0;

        for (int i = 0; i < rounds; i++)
        {
            Console.WriteLine($"Раунд {i + 1}");

            string choice1 = ReadFromPlayer(stream1);
            string choice2 = ReadFromPlayer(stream2);

            Console.WriteLine($"Игрок 1 выбрал: {choice1}");
            Console.WriteLine($"Игрок 2 выбрал: {choice2}");

            int result = DetermineWinner(choice1, choice2);

            string resultMsg = "";
            if (result == 1)
            {
                score1++;
                resultMsg = "Игрок 1 выиграл раунд!";
            }
            else if (result == 2)
            {
                score2++;
                resultMsg = "Игрок 2 выиграл раунд!";
            }
            else
            {
                resultMsg = "Ничья!";
            }

            SendToPlayer(stream1, resultMsg);
            SendToPlayer(stream2, resultMsg);
        }

        Console.WriteLine("\nИтоги игры:");
        Console.WriteLine($"Игрок 1: {score1} побед.");
        Console.WriteLine($"Игрок 2: {score2} побед.");

        stream1.Close();
        stream2.Close();
        player1.Close();
        player2.Close();
        server.Stop();
    }

    static string ReadFromPlayer(NetworkStream stream)
    {
        byte[] buffer = new byte[256];
        int bytes = stream.Read(buffer, 0, buffer.Length);
        return Encoding.UTF8.GetString(buffer, 0, bytes).Trim().ToUpper();
    }

    static void SendToPlayer(NetworkStream stream, string message)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(message);
        stream.Write(buffer, 0, buffer.Length);
    }

    static int DetermineWinner(string c1, string c2)
    {
        if (c1 == c2) return 0;
        if ((c1 == "К" && c2 == "Н") || (c1 == "Н" && c2 == "Б") || (c1 == "Б" && c2 == "К")) return 1;
        return 2;
    }
}
