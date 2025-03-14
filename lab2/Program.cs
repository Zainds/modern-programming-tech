using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;

class Program {
    static int size = int.MaxValue / 2;
    static int[] arr;
    static int threadCount = 6;
    static ConcurrentDictionary<int, int> threadsNegativeCount = new ConcurrentDictionary<int, int>();

    static int[] GenerateRandomArray(int size) {
        Random random = new Random();
        int[] randomArr = new int[size];
        for (int i = 0; i < size; i++) {
            randomArr[i] = random.Next(-100, 100); // Генерация чисел в диапазоне [-100, 100]
        }
        return randomArr;
    }

    // Подсчет отрицательных чисел в одном потоке
    static void CountNegativeNumbersThread(object param) {
        int threadNumber = (int)param;
        int chunk = size / threadCount;
        int start = threadNumber * chunk;
        int end = (threadNumber == threadCount - 1) ? size : start + chunk;

        int count = 0;
        for (int i = start; i < end; i++) {
            if (arr[i] < 0) {
                count++;
            }
        }

        threadsNegativeCount[threadNumber] = count;
        Console.WriteLine($"Поток {threadNumber} завершил работу. Найдено отрицательных чисел: {count}");
    }

    // Подсчет отрицательных чисел без потоков
    static int CountNegativeNumbers(int[] arr) {
        return arr.Count(num => num < 0);
    }

    // Подсчет отрицательных чисел с потоками
    static void CountNegativeNumbersWithThreads(int[] arr, int threadCount) {
        Thread[] threadArray = new Thread[threadCount];
        for (int i = 0; i < threadCount; i++) {
            threadArray[i] = new Thread(new ParameterizedThreadStart(CountNegativeNumbersThread));
            threadArray[i].Start(i);
        }

        for (int i = 0; i < threadCount; i++) {
            threadArray[i].Join();
        }
    }
    
    static void Main(string[] args) {
        Console.WriteLine("Генерация массива...");
        arr = GenerateRandomArray(size);
        Console.WriteLine("Массив сгенерирован.");

        Console.WriteLine("\nПодсчет отрицательных чисел без потоков...");
        DateTime sqStart = DateTime.Now;
        int negativeCount = CountNegativeNumbers(arr);
        DateTime sqEnd = DateTime.Now;
        Console.WriteLine("Подсчет завершен. Время выполнения: " + (sqEnd - sqStart));
        Console.WriteLine("Число отрицательных чисел: " + negativeCount);

        Console.WriteLine("\nПодсчет отрицательных чисел с потоками...");
        DateTime mtStart = DateTime.Now;
        CountNegativeNumbersWithThreads(arr, threadCount);
        DateTime mtEnd = DateTime.Now;
        Console.WriteLine("Подсчет завершен. Время выполнения: " + (mtEnd - mtStart));

        int totalNegativeCount = threadsNegativeCount.Values.Sum();
        Console.WriteLine("Общее число отрицательных чисел: " + totalNegativeCount);
    }
}
