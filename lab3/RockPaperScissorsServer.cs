using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class RockPaperScissorsServer
{
    private static TcpListener server;
    private static TcpClient player1, player2;
    private static StreamReader reader1, reader2;
    private static StreamWriter writer1, writer2;
    private static int rounds = 3;
    private static int score1 = 0, score2 = 0;

    public static void Start()
    {
        try
        {
            server = new TcpListener(IPAddress.Any, 5000);
            server.Start();
            Console.WriteLine("Сервер запущен...");

            player1 = server.AcceptTcpClient();
            Console.WriteLine("Игрок 1 IP: " + ((IPEndPoint)player1.Client.RemoteEndPoint).Address);
            reader1 = new StreamReader(player1.GetStream(), Encoding.UTF8);
            writer1 = new StreamWriter(player1.GetStream(), Encoding.UTF8) { AutoFlush = true };
            writer1.WriteLine("Вы Игрок 1");

            player2 = server.AcceptTcpClient();
            Console.WriteLine("Игрок 2 IP: " + ((IPEndPoint)player2.Client.RemoteEndPoint).Address);
            reader2 = new StreamReader(player2.GetStream(), Encoding.UTF8);
            writer2 = new StreamWriter(player2.GetStream(), Encoding.UTF8) { AutoFlush = true };
            writer2.WriteLine("Вы Игрок 2");

            Console.WriteLine($"Начинаем игру на {rounds} раундов");
            writer1.WriteLine(rounds);
            writer2.WriteLine(rounds);

            for (int i = 0; i < rounds; i++)
            {
                Console.WriteLine($"\nРаунд {i + 1}");
                string move1 = null;
                string move2 = null;

                // Синхронизация потоков для ожидания ввода от обоих игроков
                ManualResetEvent move1Ready = new ManualResetEvent(false);
                ManualResetEvent move2Ready = new ManualResetEvent(false);

                Thread t1 = new Thread(() =>
                {
                    try
                    {
                        writer1.WriteLine("Ваш ход");
                        move1 = reader1.ReadLine()?.Trim().ToUpper();
                        Console.WriteLine($"Игрок 1 выбрал: {move1}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка чтения от Игрока 1: {ex.Message}");
                    }
                    finally
                    {
                        move1Ready.Set(); // Сигнализируем, что Игрок 1 сделал выбор
                    }
                });

                Thread t2 = new Thread(() =>
                {
                    try
                    {
                        writer2.WriteLine("Ваш ход");
                        move2 = reader2.ReadLine()?.Trim().ToUpper();
                        Console.WriteLine($"Игрок 2 выбрал: {move2}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка чтения от Игрока 2: {ex.Message}");
                    }
                    finally
                    {
                        move2Ready.Set(); // Сигнализируем, что Игрок 2 сделал выбор
                    }
                });

                t1.Start();
                t2.Start();

                // Ждем, пока оба игрока сделают выбор
                WaitHandle.WaitAll(new WaitHandle[] { move1Ready, move2Ready });

                Console.WriteLine($"Получено: Игрок 1 - {move1}, Игрок 2 - {move2}");

                if (string.IsNullOrEmpty(move1) || string.IsNullOrEmpty(move2) || !"КНБ".Contains(move1) || !"КНБ".Contains(move2))
                {
                    Console.WriteLine("Ошибка: один из игроков ввел некорректный ход");
                    writer1.WriteLine("Ошибка");
                    writer2.WriteLine("Ошибка");
                    continue;
                }

                int result = DetermineWinner(move1, move2);
                if (result == 1) score1++;
                if (result == 2) score2++;

                Console.WriteLine($"Раунд {i + 1}: Игрок 1 выбрал {move1}, Игрок 2 выбрал {move2}");
                Console.WriteLine($"Результат раунда: {(result == 0 ? "Ничья" : result == 1 ? "Победил Игрок 1" : "Победил Игрок 2")}");

                writer1.WriteLine(result);
                writer2.WriteLine(result);
            }

            Console.WriteLine($"\nИтоговый счёт: Игрок 1 - {score1} | Игрок 2 - {score2}");

            writer1.WriteLine($"Игра завершена. Итоговый счёт: Игрок 1 - {score1}, Игрок 2 - {score2}");
            writer2.WriteLine($"Игра завершена. Итоговый счёт: Игрок 1 - {score1}, Игрок 2 - {score2}");

            player1.Close();
            player2.Close();
            server.Stop();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка сервера: {ex.Message}");
        }
    }

    private static int DetermineWinner(string move1, string move2)
    {
        Console.WriteLine($"Сравнение ходов: {move1} vs {move2}");
        if (move1 == move2) return 0;
        if ((move1 == "К" && move2 == "Н") || (move1 == "Н" && move2 == "Б") || (move1 == "Б" && move2 == "К"))
            return 1;
        return 2;
    }
}