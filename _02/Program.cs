using System.Security.Cryptography;

namespace _02
{
    // Задание 4. Реализуйте метод который  уменьшит каждое
    // значение коллекции целых чисел на 5.
    // Выполните метод 3 раза в разных потоках асинхронно. 
    internal class Program
    {
        static int[] arr = new int[] { 11, 14, 28, 23, 26, 29 };
        static int val = -5;
        static void Main(string[] args)
        {
            Console.WriteLine("Array at start:");
            foreach(int i in arr)
            {
                Console.Write(i + "\t");
            }

            // создаем массив из 3ех тасков
            Task[] tasks = new Task[3];

            // цикл по таскам
            for(int i = 0; i < tasks.Length; i++)
            {
                // создаем таск с помощью анонимного делегата
                tasks[i] = new Task(delegate ()
                {
                    // цикл по массиву
                    for(int j = 0; j < arr.Length; j++)
                    {
                        // статический метод Add класса Interlocked увеличивает эл-ты массива на val
                        Interlocked.Add(ref arr[j], val);
                    }
                });
                tasks[i].Start();
            }

            for(int i = 0; i < tasks.Length; i++)
            {
                // вызываем метод Wait(), чтобы главный поток дождался завершения выполнения тасков
                tasks[i].Wait();
            }

            Console.WriteLine("\nArray at the end:");
            foreach (int i in arr)
            {
                Console.Write(i + "\t");
            }
        }
    }
}
